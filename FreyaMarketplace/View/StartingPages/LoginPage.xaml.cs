using Microsoft.Maui.Controls;
using FreyaMarketplace.Services;

namespace FreyaMarketplace.View.StartingPages
{
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();
        }

        private async void Login_Clicked(object sender, EventArgs e)
        {
            string userEmail = txtEmail.Text;
            string password = txtPassword.Text;

            if (string.IsNullOrWhiteSpace(userEmail) || string.IsNullOrWhiteSpace(password))
            {
                await DisplayAlert("Error", "Missing email or password", "OK");
                return;
            }

            // Authentication logic here (to be replaced)
            AuthenticationService.Login(userEmail, password);
            bool loginSuccess = true;

            if (loginSuccess)
            {
                Preferences.Set("IsLoggedIn", true);
                await Shell.Current.GoToAsync("HomePage");
            }
            else
            {
                await DisplayAlert("Error", "Login failed. Please try again.", "OK");
            }
        }

        private async void OnRegisterClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("RegisterPage");
        }

        private async void ForgotPassword_Tapped(object sender, EventArgs e)
        {
            await DisplayAlert("Forgot Password - not implemened", "Password reset functionality not implemented yet.", "OK");
        }
    }
}