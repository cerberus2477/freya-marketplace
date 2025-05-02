namespace FreyaMarketplace.View.Listings
{
    public partial class CreateListingPage : ContentPage
    {
        List<string> uploadedImages = new List<string>();

        public CreateListingPage()
        {
            InitializeComponent();
        }

        private async void OnUploadPhotosClicked(object sender, EventArgs e)
        {
            try
            {
                var results = await FilePicker.PickMultipleAsync(new PickOptions
                {
                    PickerTitle = "Válaszd ki a képeket",
                    //TODO: milyen típusok lehetnek az apiban?
                    // TODO: max 10 db legyen
                    FileTypes = FilePickerFileType.Images
                });

                if (results != null)
                {
                    foreach (var file in results)
                    {
                        var stream = await file.OpenReadAsync();
                        uploadedImages.Add(file.FullPath);

                        // make a new image that is fit to size
                        var image = new Image
                        {
                            Source = ImageSource.FromStream(() => stream),
                            HeightRequest = 100,
                            WidthRequest = 100
                        };

                        ImageContainer.Children.Add(image);
                    }
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", "Failed to upload photos: " + ex.Message, "OK");
                    // TODO exceptionhandler
            }
        }


        private async void OnMyListingsClicked(object sender, EventArgs e)
        {
            // Navigate to MyListingsPage
            await Shell.Current.GoToAsync("MyListingsPage");
        }
    }
}
