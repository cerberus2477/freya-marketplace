namespace FreyaMarketplace.View.StartingPages
{
    public partial class RegisterPage : ContentPage
    {
        public RegisterPage()
        {
            InitializeComponent();
        }

        private async void Register_Clicked(object sender, EventArgs e)
        {
            string userName = txtRegisterUserName.Text;
            string email = txtRegisterEmail.Text;
            string password = txtRegisterPassword.Text;
            string confirmPassword = txtRegisterConfirmPassword.Text;

            if (string.IsNullOrWhiteSpace(userName) || string.IsNullOrWhiteSpace(email) ||
                string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(confirmPassword))
            {
                await DisplayAlert("Error", "All fields are required", "OK");
                return;
            }

            if (password != confirmPassword)
            {
                await DisplayAlert("Error", "Passwords do not match", "OK");
                return;
            }

            //TODO: Registration logic here (to be replaced)
            bool registrationSuccess = true;

            if (registrationSuccess)
            {
                await DisplayAlert("Success", "Registration successful", "OK");
                await Shell.Current.GoToAsync("//LoginPage");
            }
            else
            {
                await DisplayAlert("Error", "Registration failed. Please try again.", "OK");
            }
        }

        private async void OnLoginClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//LoginPage");
        }
    }
}
