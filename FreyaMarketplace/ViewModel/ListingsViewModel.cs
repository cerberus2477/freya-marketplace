using FreyaMarketplace.Services;

namespace FreyaMarketplace.ViewModel;

public partial class ListingsViewModel : BaseViewModel
{
    public ObservableCollection<Listing> Listings { get; } = new();
    ListingService listingService;
    public ListingsViewModel(ListingService listingService)
    {
        Title = "Listing Finder";
        this.listingService = listingService;
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