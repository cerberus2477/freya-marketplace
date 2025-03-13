using FreyaMarketplace.View;
using FreyaMarketplace.View.StartingPages;
using FreyaMarketplace.View.Listings;

namespace FreyaMarketplace;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();

        Routing.RegisterRoute(nameof(ListingsPage), typeof(ListingsPage));
        Routing.RegisterRoute(nameof(ListingDetailsPage), typeof(ListingDetailsPage));
        Routing.RegisterRoute(nameof(NewListingPage), typeof(NewListingPage));
        Routing.RegisterRoute(nameof(LoginPage), typeof(LoginPage));
        Routing.RegisterRoute(nameof(RegisterPage), typeof(RegisterPage));
        Routing.RegisterRoute(nameof(GreetingPage), typeof(GreetingPage));
        Routing.RegisterRoute(nameof(HomePage), typeof(HomePage));
        Routing.RegisterRoute(nameof(ProfilePage), typeof(ProfilePage));
        Routing.RegisterRoute(nameof(DetailsPage), typeof(DetailsPage));
        Routing.RegisterRoute(nameof(MainPage), typeof(MainPage));

        //DetermineStartPage();
        //AdjustNavigationBar();
    }

    private void DetermineStartPage()
    {
        bool isLoggedIn = Preferences.Get("IsLoggedIn", false);
        SetFlyoutBehavior(this, isLoggedIn ? FlyoutBehavior.Locked : FlyoutBehavior.Disabled);
        GoToAsync(isLoggedIn ? "HomePage" : "GreetingPage");
    }

    private void AdjustNavigationBar()
    {
        bool isMobile = DeviceDisplay.MainDisplayInfo.Width < 800;
        Shell.SetFlyoutBehavior(this, isMobile ? FlyoutBehavior.Disabled : FlyoutBehavior.Locked);
        Shell.SetTabBarIsVisible(this, true);
    }

    //protected override void OnNavigated(ShellNavigatedEventArgs args)
    //{
    //    base.OnNavigated(args);
    //    bool hideTabBar = args.Current.Location.OriginalString.Contains("GreetingPage") ||
    //                      args.Current.Location.OriginalString.Contains("LoginPage") ||
    //                      args.Current.Location.OriginalString.Contains("RegisterPage");

    //    //MainTabBar.IsVisible = !hideTabBar;
    //}
}
