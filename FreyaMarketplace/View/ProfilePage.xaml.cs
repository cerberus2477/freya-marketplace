using Microsoft.Maui.Controls;

namespace FreyaMarketplace.View
{
    public partial class ProfilePage : ContentPage
    {

        public ProfilePage(ProfileViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
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

                //TODO: make sure to (clear the navigation stack), hide the nav
                await Shell.Current.GoToAsync("LoginPage");
            }
            catch (Exception ex)
            {
                // Log or handle errors
                Debug.WriteLine($"Logout failed: {ex.Message}");
                throw; // Or handle gracefully
            }

            
            
        }
    }
}