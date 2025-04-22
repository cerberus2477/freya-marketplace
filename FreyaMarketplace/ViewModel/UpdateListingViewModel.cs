namespace FreyaMarketplace.ViewModel;

[QueryProperty(nameof(Listing), "Listing")]
public partial class UpdateListingViewModel : BaseViewModel
{
    public UpdateListingViewModel()
    {
    }

    [ObservableProperty]
    Listing listing;


    //TODO: make this a toast
    //await DisplayAlert("Success", "Sikeres módosítás", "OK");

    //await Shell.Current.GoToAsync(".."); // Goes back to the previous page in Shell
}