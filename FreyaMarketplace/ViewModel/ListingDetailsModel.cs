using FreyaMarketplace.Services;
using System.Reflection;

namespace FreyaMarketplace.ViewModel;

[QueryProperty(nameof(Listing), "Listing")]
public partial class ListingDetailsViewModel : BaseViewModel
{
    public ObservableCollection<Listing> Listings { get; } = new();
    ListingService ListingService;
    public ListingDetailsViewModel(ListingService ListingService)
    {
        Title = "Listing Finder";
        this.ListingService = ListingService;
    }

    [ObservableProperty]
    Listing listing;
}