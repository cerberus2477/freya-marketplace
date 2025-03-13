using FreyaMarketplace.View;
using FreyaMarketplace.View.StartingPages;
using FreyaMarketplace.View.Listings;

namespace FreyaMarketplace;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();
        // Register the routes whoch are not directly inside <ShellContent> tags
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


        //Determine start page based on login state
        if (IsUserLoggedIn())
        {
            NavigateToHomePage();
        }
        else
        {
            NavigateToGreetingPage();
        }

        AdjustNavigationBar();
    }

    private bool IsUserLoggedIn()
    {
        //TODO: Replace with actual login check (e.g., SecureStorage, Preferences, or API)
        return Preferences.Get("IsLoggedIn", false);
    }

    public void NavigateToHomePage()
    {
        MainTabBar.IsVisible = true;
        GoToAsync("HomePage");
    }

    public void NavigateToGreetingPage()
    {
        MainTabBar.IsVisible = false;
        GoToAsync("GreetingPage");
    }

    private void AdjustNavigationBar()
    {
        if (DeviceInfo.Platform == DevicePlatform.WinUI)
        {
            Shell.SetTabBarIsVisible(this, false); // Hide bottom bar
            Shell.SetFlyoutBehavior(this, FlyoutBehavior.Flyout); // Show side menu
        }
        else
        {
            Shell.SetFlyoutBehavior(this, FlyoutBehavior.Disabled); // Disable sidebar
            Shell.SetTabBarIsVisible(this, true); // Show bottom bar
        }
    }
}