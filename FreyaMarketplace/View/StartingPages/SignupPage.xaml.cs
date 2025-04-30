namespace FreyaMarketplace.View.StartingPages
{
    public partial class SignupPage : ContentPage
    {
        public SignupPage(AuthViewModel viewModel)
        {
            InitializeComponent();
            viewModel.Title = "Regisztráció"; 
            BindingContext = viewModel;
        }

        private async void OnLoginClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("LoginPage");
        }
    }
}
