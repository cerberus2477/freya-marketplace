namespace FreyaMarketplace.ViewModel;

public partial class BaseViewModel : ObservableObject
{
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsNotBusy))]
    bool isBusy;

    public bool IsNotBusy => !IsBusy;

    [ObservableProperty]
    string title;

    [RelayCommand]
    private async Task OpenImageViewer(string imageUrl)
    {
        await Shell.Current.GoToAsync($"imageviewer?imageUrl={Uri.EscapeDataString(imageUrl)}");
    }
}