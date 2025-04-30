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
    public async Task OpenImageViewer(string imageUrl)
    {
        await Shell.Current.GoToAsync($"imageviewer?imageUrl={Uri.EscapeDataString(imageUrl)}");
    }

    [RelayCommand]
    public async Task CopyToClipboard(string text)
    {
        await Clipboard.SetTextAsync(text);
        await ToastUtil.ShowToastAsync("Szöveg vágólapra másolva");
    }

    [RelayCommand]
    public async Task OpenEmailApp((string subject, string to) args)
    {
        var message = new EmailMessage
        {
            Subject = args.subject,
            To = new List<string> { args.to }
        };

        await Email.ComposeAsync(message);
        await ToastUtil.ShowToastAsync("Email megnyitva");
    }

    [RelayCommand]
    public async Task OpenMapApp(string searchterm)
    {
        var locationUri = $"https://www.google.com/maps/search/?api=1&query={Uri.EscapeDataString(searchterm)}";
        await Launcher.Default.OpenAsync(locationUri);
        await ToastUtil.ShowToastAsync("Térkép megnyitva");
    }
}
