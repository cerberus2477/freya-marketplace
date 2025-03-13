using Microsoft.Maui.Controls;

namespace FreyaMarketplace.View
{
    public partial class HomePage : ContentPage
    {
        public HomePage()
        {
            InitializeComponent();
        }

        private async void OnListingsClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("ListingsPage");
        }

        private async void OnNewListingClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("NewListingPage");
        }

    }
}