using FreyaMarketplace.Services;

namespace FreyaMarketplace.ViewModel;

public partial class ListingsViewModel : BaseViewModel
{
    List<Listing> allListings = new();
    int PageSize = 20;

    //public ObservableCollection<Listing> Listings { get; } = new();
    public ObservableRangeCollection<Listing> Listings { get; set; } = new ObservableRangeCollection<Listing>();
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

            allListings = await listingService.SearchListings(SearchQuery);

            if (allListings == null) return;

            //Listings.Clear();
            //foreach (var listing in listings)
            //    Listings.Add(listing);

            Listings.AddRange(allListings.Take(PageSize));

        }
        catch (Exception ex)
        {
            await exceptionHandlerUtil.HandleExceptionAsync(ex, "Hiba adódott a hirdetések lekérése során.");
        }

        finally
        {
            IsBusy = false;
            IsRefreshing = false;
        }
    }

    //TODO: implement filters


    [RelayCommand]
    public async Task GetNextListings()
    {
        if (IsBusy)
            return;
        try
        {
            if (Listings.Count > 0)
            {
                Listings.AddRange(allListings.Skip(Listings.Count).Take(PageSize));
            }
        }
        catch (Exception ex)
        {
            await exceptionHandlerUtil.HandleExceptionAsync(ex, "Hiba adódott a következő hirdetések lekérése során.");
        }
        finally
        {
            IsBusy = false;
            IsRefreshing = false;
        }
    }

}