using FreyaMarketplace.Services;
using System.Reflection.Metadata;
using System.Windows.Input;

namespace FreyaMarketplace.ViewModel;

public partial class AuthViewModel : BaseViewModel
{
    AuthenticationService authService;

    [ObservableProperty]
    string email;

    [ObservableProperty]
    string password;

    //TODO. mind legyen private vagy egyiksem, töbi viewmodelben is. lehet egy sorba is írni
    [ObservableProperty]
    private string title;

    public AuthViewModel(AuthenticationService authService)
    {
        this.authService = authService;
    }

    [RelayCommand]
    private async Task LoginAsync()
    {
        if (IsBusy) return;

        try
        {
            IsBusy = true;

            var result = await authService.LoginAsync(Email, Password);

            if (result.Status == 200 && result.Data != null)
            {
                await SecureStorage.SetAsync("auth_token", result.Data.Token);
                await Shell.Current.GoToAsync("HomePage");
            }
            else
            {
                //TODO:
                // Handle different error messages
                //if (result.Status == 401)
                //{
                //    await Shell.Current.DisplayAlert("Bejelentkezési hiba", "Helytelen hitelesítő adatok", "OK");
                //}
                //else
                //{
                //    await Shell.Current.DisplayAlert("Hiba", result.Message, "OK");
                //}

                //vagy

                var errorResponse = await result.Content.ReadAsStringAsync();
                await Shell.Current.DisplayAlert("Hibás adatok!", errorResponse, "OK");
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Hiba a bejelentkezés során: {ex.Message}");
            await Shell.Current.DisplayAlert("Hiba a bejelentkezés során:", ex.Message, "OK");
        }
        finally
        {
            IsBusy = false;
        }

    }

}