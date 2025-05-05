namespace FreyaMarketplace.View;
public partial class FullImageViewerPage : ContentPage
{
    private readonly List<ImageSource> _images;
    private int _currentIndex;

    public FullImageViewerPage(List<ImageSource> images, ImageSource selected)
    {
        InitializeComponent();
        _images = images;
        _currentIndex = _images.IndexOf(selected);
        LoadImage();
    }

    private void LoadImage()
    {
        FullImage.Source = _images[_currentIndex];
        PrevButton.IsVisible = _currentIndex > 0;
        NextButton.IsVisible = _currentIndex < _images.Count - 1;
    }

    private void Previous_Clicked(object sender, EventArgs e)
    {
        if (_currentIndex > 0)
        {
            _currentIndex--;
            LoadImage();
        }
    }

    private void Next_Clicked(object sender, EventArgs e)
    {
        if (_currentIndex < _images.Count - 1)
        {
            _currentIndex++;
            LoadImage();
        }
    }

    private async void Close_Clicked(object sender, EventArgs e)
    {
        await Navigation.PopModalAsync();
    }
}
