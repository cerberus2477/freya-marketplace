using Microsoft.Maui.Controls;

namespace FreyaMarketplace.View
{
    public partial class ProfilePage : ContentPage
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string City { get; set; }
        public string Birthdate { get; set; }

        public ProfilePage()
        {
            InitializeComponent();
            LoadUserData();
        }

        private void LoadUserData()
        {
            // Simulating loading user data (Replace this with actual API call)
            Username = Preferences.Get("Username", "admin");
            Email = Preferences.Get("Email", "nacsalevi@gmail.com");
            City = Preferences.Get("City", "bottyan");
            Birthdate = Preferences.Get("Birthdate", "1972-05-30");

            // Bind to UI
            UsernameEntry.Text = Username;
            EmailEntry.Text = Email;
            CityEntry.Text = City;
            BirthdateEntry.Text = Birthdate;
        }

        private void EditProfile_Clicked(object sender, EventArgs e)
        {
            // Enable editing
            UsernameEntry.IsEnabled = true;
            EmailEntry.IsEnabled = true;
            CityEntry.IsEnabled = true;
            BirthdateEntry.IsEnabled = true;

            // Show Save button, hide Edit button
            EditButton.IsVisible = false;
            SaveButton.IsVisible = true;
        }

        private void SaveProfile_Clicked(object sender, EventArgs e)
        {
            // Save changes (Replace with actual API call)
            Preferences.Set("Username", UsernameEntry.Text);
            Preferences.Set("Email", EmailEntry.Text);
            Preferences.Set("City", CityEntry.Text);
            Preferences.Set("Birthdate", BirthdateEntry.Text);

            // Disable editing
            UsernameEntry.IsEnabled = false;
            EmailEntry.IsEnabled = false;
            CityEntry.IsEnabled = false;
            BirthdateEntry.IsEnabled = false;

            // Show Edit button, hide Save button
            EditButton.IsVisible = true;
            SaveButton.IsVisible = false;
        }

        private async void Logout_Clicked(object sender, EventArgs e)
        {
            Preferences.Set("IsLoggedIn", false);

            //TODO: make sure to (clear the navigation stack), hide the nav
            await Shell.Current.GoToAsync("LoginPage");
        }
    }
}