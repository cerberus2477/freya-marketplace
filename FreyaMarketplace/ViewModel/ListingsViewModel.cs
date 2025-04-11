using FreyaMarketplace.Services;

namespace FreyaMarketplace.ViewModel;

public partial class ListingsViewModel : BaseViewModel
{
    public ObservableCollection<Listing> Listings { get; } = new();
    private readonly ListingService listingService;
    private readonly ExceptionHandlerUtil exceptionHandlerUtil;

    [ObservableProperty]
    bool isRefreshing;
    public ListingsViewModel(ListingService listingService, ExceptionHandlerUtil exceptionHandlerUtil)
    {
        Title = "Listings";
        this.listingService = listingService;
        this.exceptionHandlerUtil = exceptionHandlerUtil;
        //load the listings automatically when navigated to the page
        Task.Run(SearchListingsAsync);
    }

    //This code checks to see if the selected item is non-null
    //and then uses the built in Shell Navigation API to push a new page
    //with the listing as a parameter and then deselects the item.

    [RelayCommand]
    async Task GoToListingDetails(Listing listing)
    {
        if (listing == null)
            return;

        await Shell.Current.GoToAsync("ListingDetailsPage", true, new Dictionary<string, object>
        {
            {"Listing", listing }
        });
    }


    [ObservableProperty]
    private string searchQuery = string.Empty;

    [RelayCommand]
    async Task SearchListingsAsync()
    {
        if (IsBusy)
            return;
        try
        {
            IsBusy = true;
            var listings = await listingService.SearchListings(SearchQuery);
            //Listings = new ObservableCollection<Listing>(listings ?? []);

            //if (listings == null) return;
            Listings.Clear();
            foreach (var listing in listings)
                Listings.Add(listing);
        }
        catch (Exception ex)
        {
            await exceptionHandlerUtil.HandleExceptionAsync(new Exception(ex.Message), "Hiba adódott a hirdetések lekérése során.");
        }

        finally
        {
            IsBusy = false;
            IsRefreshing = false;
        }
    }

    //TODO: implement filters



}