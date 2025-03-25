namespace FreyaMarketplace.ViewModel;

public partial class BaseViewModel : ObservableObject
{
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsNotBusy))]
    bool isBusy;

    [ObservableProperty]
    string title;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(HasNoError))]
    string errorMessage;

    public bool IsNotBusy => !isBusy;
    public bool HasNoError => string.IsNullOrEmpty(errorMessage);

    // Helper method for common error handling
    protected async Task HandleExceptionAsync(Exception ex,
        string customMessage = null,
        [CallerMemberName] string caller = null)
    {
        errorMessage = customMessage ?? "An error occurred";
        Debug.WriteLine($"Error in {caller}: {ex}");
        await Shell.Current.DisplayAlert("Error", errorMessage, "OK");
    }

    // Navigation helper
    protected async Task NavigateToAsync(string route, bool animate = true)
    {
        try
        {
            await Shell.Current.GoToAsync(route, animate);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(ex, "Navigation failed");
        }
    }
}