public partial class LoginViewModel : BaseViewModel
{
    private readonly IAuthService _authService;

    [ObservableProperty]
    string email;

    [ObservableProperty]
    string password;

    public LoginViewModel(IAuthService authService)
    {
        _authService = authService;
        title = "Login";
    }

    [RelayCommand]
    private async Task LoginAsync()
    {
        if (isBusy) return;

        try
        {
            isBusy = true;
            errorMessage = string.Empty;

            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                errorMessage = "Please enter both email and password";
                return;
            }

            var result = await _authService.LoginAsync(email, password);

            if (result.Status == 200 && result.Data != null)
            {
                await SecureStorage.SetAsync("auth_token", result.Data.Token);
                await NavigateToAsync("//main");
            }
            else
            {
                errorMessage = result.Message ?? "Login failed";
            }
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(ex);
        }
        finally
        {
            isBusy = false;
        }
    }
}