using FreyaMarketplace.Services;
using Microsoft.Maui.ApplicationModel.Communication;
using System.Reflection.Metadata;
using System.Windows.Input;

namespace FreyaMarketplace.ViewModel;

public partial class AuthViewModel : BaseViewModel
{
    AuthenticationService authService;

    [ObservableProperty] private string userEmail;
    [ObservableProperty] private string userPassword;
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

            var result = await authService.LoginAsync(UserEmail, UserPassword);


            if (result is ApiResponse<LoginData> successResponse)
            {
                await SecureStorage.SetAsync("auth_token", successResponse.Data.Token);
                // Store user information
                User = successResponse.Data.User;
                Preferences.Set("user_id", User.Id);
                Preferences.Set("username", User.Username);
                Preferences.Set("UserEmail", User.Email);

                await Shell.Current.GoToAsync("///HomePage");
            }
            else if (result.Status == 401)
            {
                await Shell.Current.DisplayAlert("Bejelentkezési hiba", result.Message, "OK");
            }
            else if (result is ApiResponse<ValidationErrorData> errorResponse)
            {
                // Handle validation errors
                if (errorResponse.Data.Errors.ContainsKey("email"))
                    EmailError = string.Join("\n", errorResponse.Data.Errors["email"]);
                if (errorResponse.Data.Errors.ContainsKey("password"))
                    PasswordError = string.Join("\n", errorResponse.Data.Errors["password"]);
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