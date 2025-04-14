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
            // Fetching the User object
            string userJson = Preferences.Get("current_user", null);
            if (userJson != null)
            {
                User user = JsonSerializer.Deserialize<User>(userJson);

                // Bind to UI
                UsernameEntry.Text = user.Username;
                EmailEntry.Text = user.Email;
                CityEntry.Text = user.City;
                BirthdateEntry.Text = user.Birthdate;
                DescriptionEntry.Text = user.Description;
            }
        }

        private void EditProfile_Clicked(object sender, EventArgs e)
        {
            // Enable editing
            UsernameEntry.IsEnabled = true;
            EmailEntry.IsEnabled = true;
            CityEntry.IsEnabled = true;
            BirthdateEntry.IsEnabled = true;
            DescriptionEntry.IsEnabled = true;

            // Show Save button, hide Edit button
            EditButton.IsVisible = false;
            SaveButton.IsVisible = true;
        }

        private void SaveProfile_Clicked(object sender, EventArgs e)
        {
            // Save changes (TODO: Add actual API call)
            User user = new User();
            user.Email = EmailEntry.Text;
            user.Username = UsernameEntry.Text;
            user.City = CityEntry.Text;
            user.Description = DescriptionEntry.Text;
            user.Birthdate = BirthdateEntry.Text;

            string updatedJson = JsonSerializer.Serialize(user);
            Preferences.Set("current_user", updatedJson);

            // Disable editing
            UsernameEntry.IsEnabled = false;
            EmailEntry.IsEnabled = false;
            CityEntry.IsEnabled = false;
            BirthdateEntry.IsEnabled = false;
            DescriptionEntry.IsEnabled = false;

            // Show Edit button, hide Save button
            EditButton.IsVisible = true;
            SaveButton.IsVisible = false;
        }

        private async void Logout_Clicked(object sender, EventArgs e)
        {
            try
            {
                //removing token
                SecureStorage.Remove("auth_token");

                //removing user data
                Preferences.Remove("current_user");

                //removing isloggedin
                Preferences.Set("IsLoggedIn", false);

                await Shell.Current.DisplayAlert("Sikeres kilépés", "", "OK");
            }
            catch (Exception ex)
            {
                // Log or handle errors
                Debug.WriteLine($"Logout failed: {ex.Message}");
                throw; // Or handle gracefully
            }

            //TODO: make sure to (clear the navigation stack), hide the nav
            await Shell.Current.GoToAsync("LoginPage");
        }
    }
}