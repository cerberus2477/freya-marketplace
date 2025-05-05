using Microsoft.Maui.Controls.Shapes;

public static class ImageDisplayHelperUtil
{
    public static void RenderImages(
        StackLayout container,
        List<string> existingUrls,
        List<FileResult> newFiles,
        Action? onAddClicked = null,
        bool isEditable = false,
        int maxImages = 10,
        Action<string>? onDeleteUrl = null,
        Action<FileResult>? onDeleteFile = null)

    {
        container.Children.Clear();

        int totalCount = (existingUrls?.Count ?? 0) + (newFiles?.Count ?? 0);

        // 1. Render existing URL images
        foreach (var url in existingUrls)
        {
            var image = new Image
            {
                Source = ImageSource.FromUri(new Uri(url)),
                Aspect = Aspect.AspectFill
            };

            // Add tap to fullscreen
            var tapGesture = new TapGestureRecognizer();
            tapGesture.Tapped += (s, e) =>
            {
                var allImages = existingUrls.Select(u => (ImageSource)ImageSource.FromUri(new Uri(u))).ToList();
                ShowFullScreenImage(container, allImages, image.Source);
            };
            image.GestureRecognizers.Add(tapGesture);

            var imageContainer = CreateImageWithDelete(image, isEditable, () => ConfirmDelete(() => onDeleteUrl?.Invoke(url)));

            container.Children.Add(imageContainer);
        }

        // 2. Render local FileResult images
        foreach (var file in newFiles)
        {
            var image = new Image
            {
                Source = ImageSource.FromFile(file.FullPath),
                Aspect = Aspect.AspectFill
            };

            // Add tap to fullscreen
            var tapGesture = new TapGestureRecognizer();
            tapGesture.Tapped += (s, e) =>
            {
                var allImages = newFiles.Select(f => (ImageSource)ImageSource.FromFile(f.FullPath)).ToList();
                ShowFullScreenImage(container, allImages, image.Source);
            };
            image.GestureRecognizers.Add(tapGesture);

            var imageContainer = CreateImageWithDelete(image, isEditable, () => ConfirmDelete(() => onDeleteFile?.Invoke(file)));

            container.Children.Add(imageContainer);
        }

        // 3. Add "+" button
        if (isEditable && totalCount < maxImages)
        {
            var addBox = new Border
            {
                WidthRequest = 100,
                HeightRequest = 100,
                Stroke = Colors.Gray,
                StrokeThickness = 1,
                StrokeShape = new RoundRectangle { CornerRadius = 8 },
                Content = new Grid
                {
                    Children = {
                        new Label
                        {
                            Text = "+",
                            FontSize = 30,
                            HorizontalOptions = LayoutOptions.Center,
                            VerticalOptions = LayoutOptions.Center,
                            HorizontalTextAlignment = TextAlignment.Center,
                            VerticalTextAlignment = TextAlignment.Center
                        }
                    }
                }
            };

            var tap = new TapGestureRecognizer();
            tap.Tapped += (s, e) => onAddClicked?.Invoke();
            addBox.GestureRecognizers.Add(tap);

            container.Children.Add(addBox);
        }
    }

    private static View CreateImageWithDelete(Image image, bool isEditable, Action onDelete)
    {
        var imageBorder = new Border
        {
            WidthRequest = 100,
            HeightRequest = 100,
            Stroke = Colors.Gray,
            StrokeThickness = 1,
            StrokeShape = new RoundRectangle { CornerRadius = 8 },
            Content = image
        };

        var grid = new Grid();
        grid.Children.Add(imageBorder);

        if (isEditable)
        {
            var deleteButton = new Button
            {
                Text = "🗑️",
                Style = (Style)Application.Current.Resources["WarningButton"],
                Padding = new Thickness(5),
                FontSize = 14,
                HeightRequest = 28,
                WidthRequest = 28,
                HorizontalOptions = LayoutOptions.End,
                VerticalOptions = LayoutOptions.Start,
                Margin = new Thickness(0, 2, 2, 0)
            };
            deleteButton.Clicked += (s, e) => onDelete();
            grid.Children.Add(deleteButton);
        }

        return grid;
    }


    private static async void ConfirmDelete(Action onConfirmed)
    {
        bool confirmed = await Shell.Current.DisplayAlert("Törlés", "Biztos hogy törölni szeretnéd ezt a képet?", "Igen", "Mégse");
        if (confirmed)
            onConfirmed();
    }

    // This is a simplified version of RenderImages specifically for CreateListingPage, where we don't have to deal with loading initial images (url) like in UpdateListingPage.
    public static void RenderPickedImagesOnly(
        StackLayout container,
        List<FileResult> pickedFiles,
        Action onAddClicked,
        bool isEditable,
        int maxImages = 10,
        Action<FileResult>? onDeleteFile = null)
    {
        container.Children.Clear();

        int totalCount = pickedFiles?.Count ?? 0;

        // 1. Render picked file images
        foreach (var file in pickedFiles)
        {
            var image = new Image
            {
                Source = ImageSource.FromFile(file.FullPath),
                Aspect = Aspect.AspectFill
            };

            var imageContainer = CreateImageWithDelete(image, isEditable, () => ConfirmDelete(() => onDeleteFile?.Invoke(file)));
            container.Children.Add(imageContainer);
        }

        // 2. Add "+" button if below max
        if (isEditable && totalCount < maxImages)
        {
            var addBox = new Border
            {
                WidthRequest = 100,
                HeightRequest = 100,
                Stroke = Colors.Gray,
                StrokeThickness = 1,
                StrokeShape = new RoundRectangle { CornerRadius = 8 },
                Content = new Grid
                {
                    Children = {
                    new Label
                    {
                        Text = "+",
                        FontSize = 30,
                        HorizontalOptions = LayoutOptions.Center,
                        VerticalOptions = LayoutOptions.Center,
                        HorizontalTextAlignment = TextAlignment.Center,
                        VerticalTextAlignment = TextAlignment.Center
                    }
                }
                }
            };

            var tap = new TapGestureRecognizer();
            tap.Tapped += (s, e) => onAddClicked?.Invoke();
            addBox.GestureRecognizers.Add(tap);

            container.Children.Add(addBox);
        }
    }


    public static async void ShowFullScreenImage(StackLayout container, List<ImageSource> allImages, ImageSource current)
    {
        await Shell.Current.Navigation.PushModalAsync(new FullImageViewerPage(allImages, current));
    }
}
