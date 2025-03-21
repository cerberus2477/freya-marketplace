using FreyaMarketplace.Services;

namespace FreyaMarketplace.ViewModel;

public partial class ListingsViewModel : BaseViewModel
{
    public ObservableCollection<Listing> Listings { get; } = new();
    ListingService listingService;

    [ObservableProperty]
    bool isRefreshing;
    public ListingsViewModel(ListingService listingService)
    {
        Title = "Listings";
        this.listingService = listingService;
        //load the listings automatically when navigated to the page
        Task.Run(GetListingsAsync);
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

    [RelayCommand]
    async Task GetListingsAsync()
    {
        if (IsBusy)
            return;

        try
        {
            IsBusy = true;
            var listings = await listingService.GetListings();

                Listings.Clear();

            foreach (var listing in listings)
                Listings.Add(listing);

        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Hiba a keresés során: {ex.Message}");
            await Shell.Current.DisplayAlert("Hiba a keresés során:", ex.Message, "OK");
        }
        finally
        {
            IsBusy = false;
            IsRefreshing = false;
        }
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
            var listings = await listingService.GetListings(SearchQuery);

            Listings.Clear();
            foreach (var listing in listings)
                Listings.Add(listing);
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Hiba a keresés során: {ex.Message}");
            await Shell.Current.DisplayAlert("Error!", ex.Message, "OK");
        }
        finally
        {
            IsBusy = false;
            IsRefreshing = false;
        }
    }



}