using Microsoft.Maui.Controls;
using System;
using Microsoft.Maui;

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
            await Shell.Current.GoToAsync("///ListingsPage");
        }

        private async void OnNewListingClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("///NewListingPage");
        }
        public void ShowToast(string message)
        {
            MainThread.BeginInvokeOnMainThread(async () =>
            {
                var toast = new Label
                {
                    Text = message,
                    BackgroundColor = Color.FromArgb("#CC000000"),
                    TextColor = Colors.White,
                    HorizontalOptions = LayoutOptions.Center,
                    VerticalOptions = LayoutOptions.End,
                    Margin = new Thickness(20, 0, 20, 100),
                    Padding = new Thickness(20),
                    Opacity = 0
                };

                if (this.Content is Grid grid)
                {
                    grid.Children.Add(toast);
                }
                else
                {
                    var newGrid = new Grid();
                    newGrid.Children.Add(this.Content);
                    newGrid.Children.Add(toast);
                    this.Content = newGrid;
                }

                await toast.FadeTo(1, 200);
                await Task.Delay(3000);
                await toast.FadeTo(0, 200);

                if (this.Content is Grid g && g.Children.Contains(toast))
                {
                    g.Children.Remove(toast);
                }
            });
        }
    }
}