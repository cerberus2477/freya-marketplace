using FreyaMarketplace.Services;

namespace FreyaMarketplace.ViewModel;

public partial class ListingsViewModel : BaseViewModel
{
    public ObservableCollection<Listing> Listings { get; } = new();
    ListingService listingService;
    public ListingsViewModel(ListingService listingService)
    {
        Title = "Listings";
        this.listingService = listingService;
    }

    //This code checks to see if the selected item is non-null
    //and then uses the built in Shell Navigation API to push a new page
    //with the listing as a parameter and then deselects the item.

    [RelayCommand]
    async Task GoToListingDetails(Listing listing)
    {
        if (listing == null)
            return;

        await Shell.Current.GoToAsync("ListingsPage", true, new Dictionary<string, object>
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

            if (Listings.Count != 0)
                Listings.Clear(); 

            foreach (var listing in listings)
                Listings.Add(listing); 

        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Unable to get Listings: {ex.Message}");
            await Shell.Current.DisplayAlert("Error!", ex.Message, "OK");
        }
        finally
        {
            IsBusy = false;
        }
    }


}