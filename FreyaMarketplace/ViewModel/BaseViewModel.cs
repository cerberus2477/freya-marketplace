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
    protected async Task HandleExceptionAsync(Exception ex, string customMessage = null, [CallerMemberName] string caller = null)
    {
        Debug.WriteLine($"Error in {caller}: {ex}");
        await Shell.Current.DisplayAlert("Váratlan hiba", customMessage ?? "A kért kérés teljesítésae közben hiba állt fel", "OK");
    }
}