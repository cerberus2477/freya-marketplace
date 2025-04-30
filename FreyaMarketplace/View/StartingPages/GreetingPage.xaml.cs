namespace FreyaMarketplace.View.StartingPages
{
    public partial class GreetingPage : ContentPage
    {
        public GreetingPage()
        {
            InitializeComponent();
            NavigationPage.SetHasBackButton(this, false);
        }

        private async void OnLoginClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("LoginPage");
        }

        private async void OnSignupClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("SignupPage");
        }

    }
}
