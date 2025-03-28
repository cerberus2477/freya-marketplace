namespace FreyaMarketplace.ViewModel;

public partial class BaseViewModel : ObservableObject
{
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsNotBusy))]
    bool isBusy;

    public bool IsNotBusy => !isBusy;

    [ObservableProperty]
    string title;

    //[ObservableProperty]
    //[NotifyPropertyChangedFor(nameof(HasNoError))]
    //string errorMessage;


    //public bool HasNoError => string.IsNullOrEmpty(errorMessage);

    // Helper method for common error handling
    //protected async Task HandleExceptionAsync(Exception ex,
    //    string customMessage = null,
    //    [CallerMemberName] string caller = null)
    //{
    //    errorMessage = customMessage ?? "An error occurred";
    //    Debug.WriteLine($"Error in {caller}: {ex}");
    //    await Shell.Current.DisplayAlert("Error", errorMessage, "OK");
    //}
}