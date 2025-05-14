namespace FreyaMarketplace.ViewModel;

public partial class CreateListingViewModel : BaseViewModel
{
    private readonly ListingService listingService;
    private readonly UserplantService userplantService;
    private readonly StageService stageService;
    private readonly PlantService plantService;
    private readonly ExceptionHandlerUtil exceptionHandlerUtil;
    private readonly UserSessionService userSessionService;
    private readonly INavigationGuardService navigationGuardService;

    private bool hasUnsavedChanges;
    private int? savedUserplantId;
    private int? lastSubmittedPlantId;
    private int? lastSubmittedStageId;
    private int? lastSubmittedCount;


    //Listing fields
    [ObservableProperty] private string listingTitle;
    [ObservableProperty] private string description;
    [ObservableProperty] private string city;
    [ObservableProperty] private int price;
    //images
    private ObservableCollection<FileResult> _pickedFiles = new();
    public ReadOnlyObservableCollection<FileResult> PickedFiles { get; }

    [ObservableProperty] private string titleError;
    [ObservableProperty] private string descriptionError;
    [ObservableProperty] private string cityError;
    [ObservableProperty] private string priceError;
    [ObservableProperty] private string imageError;

    public bool IsTitleErrorVisible => !string.IsNullOrEmpty(TitleError);
    public bool IsDescriptionErrorVisible => !string.IsNullOrEmpty(DescriptionError);
    public bool IsCityErrorVisible => !string.IsNullOrEmpty(CityError);
    public bool IsPriceErrorVisible => !string.IsNullOrEmpty(PriceError);
    public bool IsImageErrorVisible => !string.IsNullOrEmpty(ImageError);


    //Userplant fields
    [ObservableProperty] private Plant selectedPlant;
    [ObservableProperty] private Stage selectedStage;
    [ObservableProperty] private int count;
    // Options for the dropdowns
    // TODO: make this work lmao
    [ObservableProperty] private ObservableRangeCollection<Stage> allStages = new();
    [ObservableProperty] private ObservableRangeCollection<Plant> allPlants = new();


    [ObservableProperty] private string plantError;
    [ObservableProperty] private string stageError;
    [ObservableProperty] private string countError;

    public bool IsPlantErrorVisible => !string.IsNullOrEmpty(PlantError);
    public bool IsStageErrorVisible => !string.IsNullOrEmpty(StageError);
    public bool IsCountErrorVisible => !string.IsNullOrEmpty(CountError);


    public CreateListingViewModel(ListingService listingService, ExceptionHandlerUtil exceptionHandlerUtil, UserSessionService userSessionService, StageService stageService, PlantService plantService, UserplantService userplantService, INavigationGuardService navigationGuardService)
    {
        this.userplantService = userplantService;
        this.listingService = listingService;
        this.exceptionHandlerUtil = exceptionHandlerUtil;
        this.userSessionService = userSessionService;
        this.stageService = stageService;
        this.plantService = plantService;
        Title = "Új hirdetés hozzáadása";

        PickedFiles = new ReadOnlyObservableCollection<FileResult>(_pickedFiles);
        this.navigationGuardService = navigationGuardService;

        navigationGuardService.SetNavigationGuard(async () =>
        {
            if (!hasUnsavedChanges)
                return true;

            bool confirm = await exceptionHandlerUtil.ConfirmNavigationWithUnsavedChangesAsync();

            if (confirm)
            {
                await ResetFormAsync();
            }

            return confirm;
        });
    }


    // Image handling
    public async Task AddImagesAsync()
    {
        var newFiles = await ImagePickerUtil.PickImagesAsync(PickedFiles.Count);
        foreach (var file in newFiles)
            _pickedFiles.Add(file);
    }

    public void AddPickedFile(FileResult file) => _pickedFiles.Add(file);
    public void RemovePickedFile(FileResult file) => _pickedFiles.Remove(file);


    // API calls
    [RelayCommand]
    public async Task GetStagesAsync()
    {
        if (IsBusy)
            return;

        try
        {
            IsBusy = true;
            var stages = await stageService.GetStages();

            AllStages.Clear();
            AllStages.AddRange(stages);
            Debug.WriteLine($"📄 Loaded stages.");
        }
        catch (Exception ex)
        {
            await exceptionHandlerUtil.HandleExceptionAsync(new Exception(ex.Message), "Hiba a növények növekedési stádiumainak lekérése során:");
        }
        finally
        {
            IsBusy = false;
        }
    }


    [RelayCommand]
    public async Task GetPlantsAsync()
    {
        if (IsBusy)
            return;

        try
        {
            IsBusy = true;
            var plants = await plantService.GetPlants();

            AllPlants.Clear();
            AllPlants.AddRange(plants);
            Debug.WriteLine($"📄 Loaded plants.");
        }
        catch (Exception ex)
        {
            await exceptionHandlerUtil.HandleExceptionAsync(new Exception(ex.Message), "Hiba a növények lekérése során:");
        }
        finally
        {
            IsBusy = false;
        }
    }

    // Track unsaved changes for navigation guard
    partial void OnListingTitleChanged(string value) => hasUnsavedChanges = true;
    partial void OnDescriptionChanged(string value) => hasUnsavedChanges = true;
    partial void OnCityChanged(string value) => hasUnsavedChanges = true;
    partial void OnPriceChanged(int value) => hasUnsavedChanges = true;
    partial void OnSelectedPlantChanged(Plant value) => hasUnsavedChanges = true;
    partial void OnSelectedStageChanged(Stage value) => hasUnsavedChanges = true;
    partial void OnCountChanged(int value) => hasUnsavedChanges = true;

    private async Task ResetFormAsync()
    {
        ListingTitle = Description = City = "";
        Price = 0;
        _pickedFiles.Clear();

        SelectedPlant = null;
        SelectedStage = null;
        Count = 0;

        ClearValidationErrors();
        hasUnsavedChanges = false;

        if (savedUserplantId.HasValue)
        {
            //await userplantService.DeleteUserplantAsync(savedUserplantId.Value);
            Debug.WriteLine($"Deleting old userplant with id {savedUserplantId.Value} from db.");
            //TODO
            savedUserplantId = null;
        }
    }


    [RelayCommand]
    private async Task CreateListingWithUserplantAsync()
    {
        if (IsBusy) return;

        try
        {
            IsBusy = true;
            ClearValidationErrors();

            // Checking whether we have a plant and a stage
            if (!ValidateUserplantFields())
            {
                IsBusy = false;
                return;
            }

            // Delete old unused userplant from db if data has changed
            if (savedUserplantId.HasValue && HasUserplantChanged())
            {
                //TODO
                //await userplantService.DeleteUserplantAsync(savedUserplantId.Value);
                Debug.WriteLine($"Deleting old userplant with id {savedUserplantId.Value} from db.");
                savedUserplantId = null;
            }

            // Create new userplant if none saved yet
            if (!savedUserplantId.HasValue)
            {
                var resultUplant = await userplantService.CreateUserplantAsync(
                    SelectedPlant.Id, SelectedStage.Id, Count);

                // Userplant can't be added, display the validation errors.
                if (resultUplant.Data is PostPatchUserplantValidationErrorData uplantError)
                {
                    ShowUserplantValidationErrors(uplantError);
                    return;
                }

                if (resultUplant.Data is not PostPatchUserplantSuccessData uplantSuccess)
                {
                    await exceptionHandlerUtil.HandleExceptionAsync(
                        new Exception(resultUplant.Message),
                        "Növény hozzáadása sikertelen.");
                    return;
                }

                // Save created userplant data locally for potential reuse
                // (listing creation fails first, then new listing created with same userplant) 
                savedUserplantId = uplantSuccess.Userplant.Id;
                lastSubmittedPlantId = SelectedPlant.Id;
                lastSubmittedStageId = SelectedStage.Id;
                lastSubmittedCount = Count;
            }

            // Userplant creation was successfull, now we create the listing with the created userplantid
            // Replace 'null' values with empty strings before passing to the API to avoid errors.
            var resultListing = await listingService.CreateListingAsync(savedUserplantId.Value, ListingTitle ?? "", Description ?? "", City ?? "", Price, PickedFiles.ToList());

            // Listing can't be added, display the validation errors.
            if (resultListing.Data is PostPatchListingValidationErrorData listingError)
            {
                ShowListingValidationErrors(listingError);
                return;
            }

            // Adding listing has been successfull. Deleting local userplant data so we don't reuse it anymore.
            if (resultListing.Data is PostPatchListingSuccessData)
            {
                await ToastUtil.ShowToastAsync("Hirdetés sikeresen hozzáadva");
                savedUserplantId = null;
                hasUnsavedChanges = false;
                navigationGuardService.ClearNavigationGuard();
                await Shell.Current.GoToAsync("..");
            }

            else
            {
                await exceptionHandlerUtil.HandleExceptionAsync(
                    new Exception(resultListing.Message),
                    "Hirdetés hozzáadása sikertelen.");
            }
        }

        catch (Exception ex)
        {
            await exceptionHandlerUtil.HandleExceptionAsync(ex, "Hiba történt.");
        }
        finally
        {
            IsBusy = false;
        }
    }


    // Helper methods
    private void ClearValidationErrors()
    {
        TitleError = DescriptionError = CityError = PriceError = ImageError =
            PlantError = StageError = CountError = null;

        OnPropertyChanged(nameof(IsTitleErrorVisible));
        OnPropertyChanged(nameof(IsDescriptionErrorVisible));
        OnPropertyChanged(nameof(IsCityErrorVisible));
        OnPropertyChanged(nameof(IsPriceErrorVisible));
        OnPropertyChanged(nameof(IsImageErrorVisible));
        OnPropertyChanged(nameof(IsPlantErrorVisible));
        OnPropertyChanged(nameof(IsStageErrorVisible));
        OnPropertyChanged(nameof(IsCountErrorVisible));
    }

    private bool HasUserplantChanged()
    {
        return lastSubmittedPlantId != SelectedPlant?.Id ||
               lastSubmittedStageId != SelectedStage?.Id ||
               lastSubmittedCount != Count;
    }

    private bool ValidateUserplantFields()
    {
        bool hasError = false;

        if (SelectedPlant == null)
        {
            PlantError = "Kérlek válassz növényt.";
            OnPropertyChanged(nameof(IsPlantErrorVisible));
            hasError = true;
        }
        if (SelectedStage == null)
        {
            StageError = "Kérlek válassz növekedési fázist.";
            OnPropertyChanged(nameof(IsStageErrorVisible));
            hasError = true;
        }

        return !hasError;
    }

    private void ShowUserplantValidationErrors(PostPatchUserplantValidationErrorData error)
    {
        if (error.Errors.TryGetValue("plant_id", out var plantErrors))
        {
            PlantError = string.Join("\n", plantErrors);
            OnPropertyChanged(nameof(IsPlantErrorVisible));
        }
        if (error.Errors.TryGetValue("stage_id", out var stageErrors))
        {
            StageError = string.Join("\n", stageErrors);
            OnPropertyChanged(nameof(IsStageErrorVisible));
        }
        if (error.Errors.TryGetValue("count", out var countErrors))
        {
            CountError = string.Join("\n", countErrors);
            OnPropertyChanged(nameof(IsCountErrorVisible));
        }
    }

    private void ShowListingValidationErrors(PostPatchListingValidationErrorData error)
    {
        if (error.Errors.TryGetValue("title", out var titleErrors))
        {
            TitleError = string.Join("\n", titleErrors);
            OnPropertyChanged(nameof(IsTitleErrorVisible));
        }
        if (error.Errors.TryGetValue("description", out var descriptionErrors))
        {
            DescriptionError = string.Join("\n", descriptionErrors);
            OnPropertyChanged(nameof(IsDescriptionErrorVisible));
        }
        if (error.Errors.TryGetValue("city", out var cityErrors))
        {
            CityError = string.Join("\n", cityErrors);
            OnPropertyChanged(nameof(IsCityErrorVisible));
        }
        if (error.Errors.TryGetValue("price", out var priceErrors))
        {
            PriceError = string.Join("\n", priceErrors);
            OnPropertyChanged(nameof(IsPriceErrorVisible));
        }
        if (error.Errors.TryGetValue("media", out var imageErrors))
        {
            ImageError = string.Join("\n", imageErrors);
            OnPropertyChanged(nameof(IsImageErrorVisible));
        }
    }

}


