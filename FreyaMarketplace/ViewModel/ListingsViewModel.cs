using FreyaMarketplace.ViewModel;
using FreyaMarketplace.Services;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Reflection;

namespace FreyaMarketplace.ViewModel;

public partial class ListingsViewModel : BaseViewModel
{
    public ObservableCollection<Listing> Listings { get; } = new();
    ListingService ListingService;
    public ListingsViewModel(ListingService ListingService)
    {
        Title = "Listing Finder";
        this.ListingService = ListingService;
    }

    [RelayCommand]
    async Task GetListingsAsync()
    {
        if (IsBusy)
            return;

        try
        {
            IsBusy = true;
            var listingsData = await ListingService.GetListings(); // Rename variable

            if (Listings.Count != 0)
                Listings.Clear(); // Correctly clear the ObservableCollection

            foreach (var listing in listingsData)
                Listings.Add(listing); // Add fetched listings

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