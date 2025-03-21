namespace FreyaMarketplace.View.Listings;

public partial class ListingsPage : ContentPage
{
    public ListingsPage(ListingsViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    private void OnSearchTextChanged(object sender, TextChangedEventArgs e)
    {
        if (BindingContext is ListingsViewModel viewModel)
        {
            viewModel.SearchQuery = e.NewTextValue;
            viewModel.SearchListingsCommand.Execute(null);
        }
    }

    private async void OnFilterButtonClicked(object sender, EventArgs e)
    {
        await Shell.Current.DisplayAlert("Szûrõk", "Itt lesznek a szûrõk!", "OK");
    }
}