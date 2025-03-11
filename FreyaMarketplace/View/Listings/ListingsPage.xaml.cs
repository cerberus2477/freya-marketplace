namespace FreyaMarketplace.View.Listings;

public partial class ListingsPage : ContentPage
{
    public ListingsPage(ListingsViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}