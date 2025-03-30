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

        //private async void Login_Clicked(object sender, EventArgs e)
        //{
        //    string userEmail = txtEmail.Text;
        //    string password = txtPassword.Text;

        //    if (string.IsNullOrWhiteSpace(userEmail) || string.IsNullOrWhiteSpace(password))
        //    {
        //        await DisplayAlert("Error", "Missing email or password", "OK");
        //        return;
        //    }

        //    if (BindingContext is AuthViewModel viewModel)
        //    {
        //        viewModel.Password = password;
        //        viewModel.Email = userEmail;
        //        viewModel.LoginCommand.Execute(null);
        //    }
        //}

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