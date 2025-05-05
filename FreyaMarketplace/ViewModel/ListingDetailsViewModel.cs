using FreyaMarketplace.View.Listings;

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

    //This function is triggered when the listing is injected into the page. Initializing variables here makes sure that the page is fully loaded first and Listing exists.
    partial void OnListingChanged(Listing value)
    {
        // If user does not have city, don't show the reveal city button. Set the iscityrevealed accordingly
        IsCityRevealButtonVisible = !string.IsNullOrWhiteSpace(value?.User?.City);
        EmailCommandParameter = (
            $"Freya Marketplace – Hirdetés: {value?.Title ?? "Hirdetés"}",
            value?.User?.Email ?? string.Empty
        );

        // Render images once the listing is set
        // Ensure we are on ListingDetailsPage and retrieve the ImageContainer by name
        if (Shell.Current.CurrentPage is ListingDetailsPage page && page.FindByName<StackLayout>("ImageContainer") is StackLayout container && value?.Media is List<string> media)
        {
            ImageDisplayHelperUtil.RenderImages(container, media, newFiles: null, onAddClicked: null, isEditable: false);
        }
    }

}
