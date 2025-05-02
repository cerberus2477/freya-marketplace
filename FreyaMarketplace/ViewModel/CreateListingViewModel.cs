namespace FreyaMarketplace.ViewModel;

public partial class CreateListingViewModel : BaseViewModel
{
    [ObservableProperty]
    Listing listing;

    private readonly ListingService listingService;
    private readonly UserplantService userplantService;
    private readonly StageService stageService;
    private readonly PlantService plantService;
    private readonly ExceptionHandlerUtil exceptionHandlerUtil;
    private readonly UserSessionService userSessionService;

    //Listing fields
    [ObservableProperty] private string listingTitle;
    [ObservableProperty] private string description;
    [ObservableProperty] private string city;
    [ObservableProperty] private decimal price;
    //image?

    [ObservableProperty] private string titleError;
    [ObservableProperty] private string descriptionError;
    [ObservableProperty] private string cityError;
    [ObservableProperty] private string priceError;
    //[ObservableProperty] private string imageError;

    public bool IsTitleErrorVisible => !string.IsNullOrEmpty(TitleError);
    public bool IsDescriptionErrorVisible => !string.IsNullOrEmpty(DescriptionError);
    public bool IsCityErrorVisible => !string.IsNullOrEmpty(CityError);
    public bool IsPriceErrorVisible => !string.IsNullOrEmpty(PriceError);
    //public bool IsImageErrorVisible => !string.IsNullOrEmpty(ImageError);


    //Userplant fields
    [ObservableProperty] private Plant selectedPlant;
    [ObservableProperty] private Stage selectedStage;
    [ObservableProperty] private int count;
    [ObservableProperty] public ObservableRangeCollection<Stage> allStages = new();
    [ObservableProperty] public ObservableRangeCollection<Plant> allPlants = new();

    [ObservableProperty] private string plantError;
    [ObservableProperty] private string stageError;
    [ObservableProperty] private string countError;

    public bool IsPlantErrorVisible => !string.IsNullOrEmpty(PlantError);
    public bool IsStageErrorVisible => !string.IsNullOrEmpty(StageError);
    public bool IsCountErrorVisible => !string.IsNullOrEmpty(CountError);


    public CreateListingViewModel(ListingService listingService, ExceptionHandlerUtil exceptionHandlerUtil, UserSessionService userSessionService, StageService stageService, PlantService plantService, UserplantService userplantService)
    {
        this.userplantService = userplantService;
        this.listingService = listingService;
        this.exceptionHandlerUtil = exceptionHandlerUtil;
        this.userSessionService = userSessionService;
        this.stageService = stageService;
        this.plantService = plantService;
        Title = "Új hirdetés hozzáadása";

        //load the stages and plants automatically when navigated to the page
        //Directly runs the methods in a background thread.
        Task.Run(GetStagesAsync);
        Task.Run(GetPlantsAsync);
    }


    [RelayCommand]
    async Task GetStagesAsync()
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
    async Task GetPlantsAsync()
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


    [RelayCommand]
    private async Task CreateListingWithUserplantAsync()
    {
        if (IsBusy) return;

        try
        {
            IsBusy = true;
            TitleError = null;
            CityError = null;
            PriceError = null;
            //ImageError = null;
            //TODO: images
            PlantError = null;
            StageError = null;
            CountError = null;

            // Adding the userplant first
            var result_uplant = await userplantService.CreateUserplantAsync(SelectedPlant.Id, SelectedStage.Id, Count);

            // Userplant can't be added, display the validation errors.
            if (result_uplant.Data is PostPatchUserplantValidationErrorData errorData_uplant)
            {
                if (errorData_uplant.Errors.TryGetValue("plant", out var plantErrors))
                {
                    PlantError = string.Join("\n", plantErrors);
                    OnPropertyChanged(nameof(IsPlantErrorVisible));
                }
                if (errorData_uplant.Errors.TryGetValue("stage", out var stageErrors))
                {
                    StageError = string.Join("\n", stageErrors);
                    OnPropertyChanged(nameof(IsStageErrorVisible));
                }
                if (errorData_uplant.Errors.TryGetValue("count", out var countErrors))
                {
                    CountError = string.Join("\n", countErrors);
                    OnPropertyChanged(nameof(IsCountErrorVisible));
                }
            }

            // Adding userplant has been succesfull, adding new listing with the userplant
            else if (result_uplant.Data is PostPatchUserplantSuccessData successData_uplant)
            {
                //TODO: actual images should be sent
                var uplant_id = successData_uplant.Userplant.Id;
                var result_listing = await listingService.CreateListingAsync(uplant_id, ListingTitle, Description, City, Price, new List<FileResult>());

                // Adding listing has been successfull. Display message to user and navigate back.
                if (result_listing.Data is PostPatchListingSuccessData successData)
                {
                    await ToastUtil.ShowToastAsync("Hirdetés sikeresen hozzáadva");
                    await Shell.Current.GoToAsync("..");
                }

                // Listing can't be added, display the validation errors.
                else if (result_listing.Data is PostPatchListingValidationErrorData errorData)
                {
                    if (errorData.Errors.TryGetValue("title", out var titleErrors))
                    {
                        TitleError = string.Join("\n", titleErrors);
                        OnPropertyChanged(nameof(IsTitleErrorVisible));
                    }
                    if (errorData.Errors.TryGetValue("description", out var descriptionErrors))
                    {
                        DescriptionError = string.Join("\n", descriptionErrors);
                        OnPropertyChanged(nameof(IsDescriptionErrorVisible));
                    }
                    if (errorData.Errors.TryGetValue("city", out var cityErrors))
                    {
                        CityError = string.Join("\n", cityErrors);
                        OnPropertyChanged(nameof(IsCityErrorVisible));
                    }
                    if (errorData.Errors.TryGetValue("price", out var priceErrors))
                    {
                        PriceError = string.Join("\n", priceErrors);
                        OnPropertyChanged(nameof(IsPriceErrorVisible));
                    }
                }
                else
                {
                    await exceptionHandlerUtil.HandleExceptionAsync(new Exception(result_uplant.Message), "Hirdetés hozzáadása sikertelen.");
                }
            }
            else
            {
                await exceptionHandlerUtil.HandleExceptionAsync(new Exception(result_uplant.Message), "Hirdetés (növény/státusz/darabszám) hozzáadása sikertelen.");
            }
        }
        catch (Exception ex)
        {
            await exceptionHandlerUtil.HandleExceptionAsync(ex, "Hiba adódott a hirdetés hozzáadása során.");
        }
        finally
        {
            IsBusy = false;
        }
    }
}
