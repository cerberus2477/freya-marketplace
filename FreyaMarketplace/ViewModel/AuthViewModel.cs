using FreyaMarketplace.Services;
using System.Reflection.Metadata;
using System.Windows.Input;

namespace FreyaMarketplace.ViewModel;

public partial class AuthViewModel : BaseViewModel
{
    AuthenticationService authService;

    [ObservableProperty] private string user_email;
    [ObservableProperty] private string user_password;
    [ObservableProperty] private string title;
    [ObservableProperty] private string emailError;
    [ObservableProperty] private string passwordError;
    [ObservableProperty] private User user;


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
            EmailError = null;
            PasswordError = null;

            var result = await authService.LoginAsync(User_email, User_password);

            if (result.Status == 200 && result.Data is LoginData loginData)
            {
                await SecureStorage.SetAsync("auth_token", loginData.Token);
                // Store user information
                User = loginData.User;
                Preferences.Set("user_id", User.Id);
                Preferences.Set("username", User.Username);
                Preferences.Set("user_email", User.Email);

                await Shell.Current.GoToAsync("HomePage");
            }
            else if (result.Status == 401)
            {
                await Shell.Current.DisplayAlert("Bejelentkezési hiba", result.Message, "OK");
            }
            else if (result.Status == 422 && result.Data is ValidationErrorData errorData)
            {
                // Handle validation errors
                if (errorData.Errors.ContainsKey("email"))
                    EmailError = string.Join("\n", errorData.Errors["email"]);
                if (errorData.Errors.ContainsKey("password"))
                    PasswordError = string.Join("\n", errorData.Errors["password"]);
            }
            else
            {
                await HandleExceptionAsync(new Exception(result.Message), "Hiba a bejelentkezés során:");
            }
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(ex, "Hiba a bejelentkezés során:");
        }
        finally
        {
            IsBusy = false;
        }
    }


}