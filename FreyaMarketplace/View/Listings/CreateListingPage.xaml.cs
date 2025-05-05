using Microsoft.Maui.Controls;

namespace FreyaMarketplace.View.Listings
{
    public partial class CreateListingPage : ContentPage
    {
        private CreateListingViewModel ViewModel => (CreateListingViewModel)BindingContext;

        public CreateListingPage(CreateListingViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
            RefreshImageGrid(); // Initial call to show "+" box even if empty
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            if (BindingContext is CreateListingViewModel vm)
            {
                await vm.GetStagesAsync();
                await vm.GetPlantsAsync();
            }
        }

        private async void OnAddImageClicked()
        {
            await ViewModel.AddImagesAsync();
            RefreshImageGrid();
        }
        private void RefreshImageGrid()
        {
            ImageDisplayHelperUtil.RenderPickedImagesOnly(
                ImageContainer,
                ViewModel.PickedFiles.ToList(),
                OnAddImageClicked,
                isEditable: true,
                maxImages: 10,
                onDeleteFile: file =>
                {
                    ViewModel.RemovePickedFile(file);
                    RefreshImageGrid();
                }
            );
        }

        private async void OnMyListingsClicked(object sender, EventArgs e)
        {
            // Navigate to MyListingsPage
            await Shell.Current.GoToAsync("MyListingsPage");
        }
    }
}

