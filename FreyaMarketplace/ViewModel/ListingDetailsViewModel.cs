namespace FreyaMarketplace.ViewModel;

[QueryProperty(nameof(Listing), "Listing")]
public partial class ListingDetailsViewModel : BaseViewModel
{
    public ListingDetailsViewModel()
    {
    }

    [ObservableProperty]
    Listing listing;

    [ObservableProperty]
    private bool isEmailRevealed;

    [ObservableProperty]
    private bool isCityRevealed;

    [ObservableProperty]
    private bool isCityRevealButtonVisible;

    [ObservableProperty]
    private (string subject, string to) emailCommandParameter;

    [RelayCommand]
    private void RevealEmail() => IsEmailRevealed = true;

    [RelayCommand]
    private void RevealCity() => IsCityRevealed = true;

    // if user does not have city, don't show the reveal city button.
    // when listing is injected into the page, set the iscityrevealed accordingly
    partial void OnListingChanged(Listing value)
    {
        IsCityRevealButtonVisible = !string.IsNullOrWhiteSpace(value?.User?.City);
        EmailCommandParameter = (
            $"Freya Marketplace – Hirdetés: {value?.Title ?? "Hirdetés"}",
            value?.User?.Email ?? string.Empty
        );
    }

}
