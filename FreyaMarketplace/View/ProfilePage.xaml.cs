using Microsoft.Maui.Controls;
using FreyaMarketplace.View;
using FreyaMarketplace.View.StartingPages;

namespace FreyaMarketplace.View
{
    public partial class ProfilePage : ContentPage
    {
        private UserSessionService userSessionService;
        public ProfilePage(ProfileViewModel viewModel, UserSessionService userSessionService)
        {
            InitializeComponent();
            BindingContext = viewModel;
            this.userSessionService = userSessionService;
        }

        //todo: ezek jó helyen vannak itt vagy menjenek a viewmodelbe?
        private void EditProfile_Clicked(object sender, EventArgs e)
        {
            // Enable editing
            UsernameEntry.IsEnabled = true;
            EmailEntry.IsEnabled = true;
            CityEntry.IsEnabled = true;
            BirthdateEntry.IsEnabled = true;
            DescriptionEntry.IsEnabled = true;

            // Show Update button, hide Edit button
            EditButton.IsVisible = false;
            UpdateButton.IsVisible = true;
        }

        private void UpdateProfile_Clicked(object sender, EventArgs e)
        {
            // Disable editing
            UsernameEntry.IsEnabled = false;
            EmailEntry.IsEnabled = false;
            CityEntry.IsEnabled = false;
            BirthdateEntry.IsEnabled = false;
            DescriptionEntry.IsEnabled = false;

            

            // Show Edit button, hide Update button
            EditButton.IsVisible = true;
            UpdateButton.IsVisible = false;
        }

        //should this be here or in the viewmodel?
        private async void Logout_Clicked(object sender, EventArgs e)
        {
            try
            {

                userSessionService.Logout();
                await Shell.Current.GoToAsync("/LoginPage");
            }
            catch (Exception ex)
            {
                // use exceptionhandler
                // Log or handle errors
                Debug.WriteLine($"Logout failed: {ex.Message}");
                throw; // Or handle gracefully
            }



        }


        private async void MyListings_Clicked(object sender, EventArgs e)
        {
            // Navigate to MyListingsPage
            await Shell.Current.GoToAsync("MyListingsPage");
        }
    }
}