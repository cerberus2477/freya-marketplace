using FreyaMarketplace.Services;
using Microsoft.Maui.ApplicationModel.Communication;
using System.Reflection.Metadata;
using System.Windows.Input;

namespace FreyaMarketplace.ViewModel;

public partial class AuthViewModel : BaseViewModel
{
    private readonly AuthenticationService authService;
    private readonly ExceptionHandlerUtil exceptionHandlerUtil;

    [ObservableProperty] private string userEmail;
    [ObservableProperty] private string userPassword;
    [ObservableProperty] private string title;
    [ObservableProperty] [NotifyPropertyChangedFor(nameof(IsEmailErrorVisible))] private string emailError;
    [ObservableProperty] [NotifyPropertyChangedFor(nameof(IsPasswordErrorVisible))] private string passwordError;
    [ObservableProperty] private User user;

    public bool IsEmailErrorVisible => !string.IsNullOrEmpty(EmailError);
    public bool IsPasswordErrorVisible => !string.IsNullOrEmpty(PasswordError);


    public AuthViewModel(AuthenticationService authService, ExceptionHandlerUtil exceptionHandlerUtil)
    {
        this.authService = authService;
        this.exceptionHandlerUtil = exceptionHandlerUtil;
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
            //Debug.WriteLine($"Received API Response: {JsonSerializer.Serialize<LoginApiResponse>(result)}");
            //Debug.WriteLine($"\tReceived Data: {result.Data} {JsonSerializer.Serialize(result.Data)}");
            if (result == null) return;
            if (result.Data is LoginSuccessData successData)
            {
                await SecureStorage.SetAsync("auth_token", successData.Token);

                //var retrievedToken = await SecureStorage.GetAsync("auth_token");
                //Debug.WriteLine($"Stored token: {retrievedToken}");
                // Store user information
                User = successData.User;
                Debug.Write($"User: {JsonSerializer.Serialize(User)}");
                Preferences.Set("user_id", User.Id);
                Preferences.Set("username", User.Username);
                Preferences.Set("UserEmail", User.Email);

                await Shell.Current.GoToAsync("///HomePage");
            }
            else if (result.Data is EmptyLoginData)
            {
                await exceptionHandlerUtil.HandleExceptionAsync(new Exception(result.Message), "Sikertelen bejelentkezés:");
            }
            else if (result.Data is ValidationErrorData errorData)
            {

                if (errorData.Errors.ContainsKey("email"))
                {
                    EmailError = string.Join("\n", errorData.Errors["email"]);
                    OnPropertyChanged(nameof(IsEmailErrorVisible)); 
                }

                if (errorData.Errors.ContainsKey("password"))
                {
                    PasswordError = string.Join("\n", errorData.Errors["password"]);
                    OnPropertyChanged(nameof(IsPasswordErrorVisible)); 

                }
            }
            else
            {
                await exceptionHandlerUtil.HandleExceptionAsync(new Exception(result.Message), "Hiba a bejelentkezés során:");
            }
        }
        catch (Exception ex)
        {
            await exceptionHandlerUtil.HandleExceptionAsync(ex, "Hiba a bejelentkezés során:");
        }
        finally
        {
            IsBusy = false;
        }
    }


}