namespace FreyaMarketplace.View.Listings;

public partial class ListingDetailsPage : ContentPage
{
	public ListingDetailsPage(ListingDetailsViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = viewModel;
    }
}