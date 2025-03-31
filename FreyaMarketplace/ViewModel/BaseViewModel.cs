namespace FreyaMarketplace.ViewModel;

public partial class BaseViewModel : ObservableObject
{
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsNotBusy))]
    bool isBusy;

    public bool IsNotBusy => !isBusy;

    [ObservableProperty]
    string title;

    // Helper method for common error handling
    protected async Task HandleExceptionAsync(Exception ex, string customMessage = null, bool displayExMessage = true, [CallerMemberName] string caller = null)
    {
        Debug.WriteLine($"Error in {caller}: {ex.Message}\nException:{ex}");
        string message = customMessage ?? "A kért kérés teljesítése közben hiba állt fel.";
        if (displayExMessage)
        {
            message += $"\nHibaüzenet: {ex.Message}";
        }
        await Shell.Current.DisplayAlert("Váratlan hiba", message, "OK");
    }
}