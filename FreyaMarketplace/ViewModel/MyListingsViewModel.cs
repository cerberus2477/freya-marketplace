using FreyaMarketplace.Services;

namespace FreyaMarketplace.ViewModel;

public partial class MyListingsViewModel : BaseViewModel
{
    public ObservableRangeCollection<Listing> MyListings { get; set; } = new ObservableRangeCollection<Listing>();
    private readonly ListingService listingService;
    private readonly ExceptionHandlerUtil exceptionHandlerUtil;
    private readonly UserSessionService userSessionService;

    private bool _isCountInitialized = false;
    [ObservableProperty]
    private int activeListingsCount;

    [ObservableProperty]
    bool isRefreshing;
    //TODO: isbusy kell??

    [ObservableProperty]
    private string searchQuery = string.Empty;

    string username;

    //when the searchtext changes, send a get request to the api to get the matching listings. load the first page into the UI.
    partial void OnSearchQueryChanged(string value)
    {
        SearchMyListingsCommand.Execute(null);
    }

    public MyListingsViewModel(ListingService listingService, ExceptionHandlerUtil exceptionHandlerUtil, UserSessionService userSessionService)
    {
        Title = "Saját hirdetések";
        this.listingService = listingService;
        this.exceptionHandlerUtil = exceptionHandlerUtil;
        this.userSessionService = userSessionService;
        username = userSessionService.GetCurrentUsername();
        //load the listings automatically when navigated to the page
        Task.Run(SearchMyListingsAsync);
    }


    [RelayCommand]
    async Task SearchMyListingsAsync()
    {
        if (IsBusy) return;

        try
        {
            IsBusy = true;

            var listings = await listingService.SearchListings(SearchQuery, username);

            MyListings.Clear();
            MyListings.AddRange(listings);
            // Display the total number of the users listings
            if (!_isCountInitialized)
            {
                ActiveListingsCount = listings.Count;
                _isCountInitialized = true;
            }
            Debug.WriteLine($"📄 Loaded mylistings.");
        }
        catch (Exception ex)
        {
            await exceptionHandlerUtil.HandleExceptionAsync(ex, "Hiba adódott a saját hirdetések lekérése során.");
        }
        finally
        {
            IsBusy = false;
            IsRefreshing = false;
        }
    }
    //TODO: i have duplicated of delete and modify

    [RelayCommand]
    async Task DeleteListing(Listing listing)
    {
        if (IsBusy) return;

        try
        {
            if (listing == null) return;

            bool confirm = await Shell.Current.DisplayAlert("Törlés megerősítése", $"Biztosan törölni szeretnéd a(z) {listing.Title} hirdetést?", "Igen", "Mégsem");

            if (!confirm) return;

            //TODO
            Debug.WriteLine($"Deleting listing {listing.Id}...");
            //await listingService.DeleteListing(listing.Id);
            MyListings.Remove(listing);
            ActiveListingsCount = MyListings.Count;
        }
        catch (Exception ex)
        {
            await exceptionHandlerUtil.HandleExceptionAsync(ex, "Hiba történt a hirdetés törlésekor.");
        }
    }


    //This code checks to see if the selected item is non-null
    //and then uses the built in Shell Navigation API to push a new page
    //with the listing as a parameter and then deselects the item.


    //TODO: should these be relaycommands?
    //TODO: updatelistingpage is not recognised
    private async void OnEdit(Listing listing)
    {
        if (listing == null) return;
        await Shell.Current.GoToAsync(nameof(UpdateListingPage), true,
            new Dictionary<string, object> { ["Listing"] = listing });
    }

    async Task GoToUpdateListing(Listing listing)
    {
        if (listing == null)
            return;

        await Shell.Current.GoToAsync("UpdateListingPage", true, new Dictionary<string, object>
        {
            {"Listing", listing }
        });
    }

    private async void OnDelete(Listing listing)
    {
        if (listing == null) return;

        bool confirm = await Shell.Current.DisplayAlert("Törlés", "Biztos vagy benne, hogy törölni szeretnéd?", "Igen", "Mégse");
        if (!confirm) return;

        await listingService.DeleteListingAsync(listing.Id);
        await LoadListingsAsync(); // Or remove from ObservableCollection
    }


}