using FreyaMarketplace.Services;

namespace FreyaMarketplace.ViewModel;

public partial class LoginViewModel : BaseViewModel
{
    AuthenticationService authService;

    [ObservableProperty]
    string email;

    [ObservableProperty]
    string password;

    public LoginViewModel(AuthenticationService authService)
    {
        this.authService = authService;
        Title = "Login";
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