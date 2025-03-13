namespace FreyaMarketplace.View.StartingPages
{
    public partial class GreetingPage : ContentPage
    {
        public GreetingPage()
        {
            InitializeComponent();
        }

        private async void OnLoginClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("LoginPage");
        }

        private async void OnRegisterClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("RegisterPage");
        }
    }
}
