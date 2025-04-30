using Microsoft.Maui.Controls;
using FreyaMarketplace.Services;

namespace FreyaMarketplace.View.StartingPages
{
    public partial class LoginPage : ContentPage
    {
        public LoginPage(AuthViewModel viewModel)
        {
            InitializeComponent();
            viewModel.Title = "Bejelentkezés";
            BindingContext = viewModel;
        }

        private async void OnSignupClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("SignupPage");
        }

        private async void ForgotPassword_Tapped(object sender, EventArgs e)
        {
            await DisplayAlert("Forgot Password - not implemened", "Password reset functionality not implemented yet.", "OK");
        }


        protected override void OnAppearing()
        {
            base.OnAppearing();

            double screenWidth = DeviceDisplay.MainDisplayInfo.Width / DeviceDisplay.MainDisplayInfo.Density;
            ((AuthViewModel)BindingContext).IsWideScreen = screenWidth > 720;
        }

    }
}