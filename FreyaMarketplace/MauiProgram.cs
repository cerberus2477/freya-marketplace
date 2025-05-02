using Microsoft.Extensions.Logging;
using CommunityToolkit.Maui;
using System.Net.Http;

namespace FreyaMarketplace;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();

        builder
            .UseMauiApp<App>()
            .UseMauiCommunityToolkit(options =>
            {
#if WINDOWS
                options.SetShouldEnableSnackbarOnWindows(true);
#endif
            })
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("FontAwesome.ttf", "FontAwesome");
                fonts.AddFont("NocturneSerif-Regular.ttf", "Nocturne");
                fonts.AddFont("Inter_18pt-Regular.ttf", "Inter");
            });

#if DEBUG
        builder.Logging.AddDebug();
#endif

        //view model is Transient so a new view model is created each time the page is navigated to

        //ViewModels
        //Listings
        builder.Services.AddTransient<ListingsViewModel>(); 
        builder.Services.AddTransient<ListingDetailsViewModel>();
        builder.Services.AddTransient<MyListingsViewModel>();
        builder.Services.AddTransient<UpdateListingViewModel>();
        builder.Services.AddTransient<CreateListingViewModel>();

        builder.Services.AddTransient<ProfileViewModel>();
        builder.Services.AddTransient<AuthViewModel>();

        // Services
        // JsonSerializer is used by the services
        builder.Services.AddSingleton(new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,  // Allow flexible casing
            AllowTrailingCommas = false,        // No extra commas allowed
            DefaultIgnoreCondition = JsonIgnoreCondition.Never // Require all fields
        });

        // Register services with default headers
        AddConfiguredHttpClient<AuthenticationService>(builder.Services);
        AddConfiguredHttpClient<ListingService>(builder.Services);
        AddConfiguredHttpClient<ProfileService>(builder.Services);
        AddConfiguredHttpClient<UserplantService>(builder.Services);

        // Register basic clients (no headers, only GET requests)
        builder.Services.AddHttpClient<PlantService>();
        builder.Services.AddHttpClient<StageService>();

        // This one does not communicate with the API, therefore no need for HttpClient
        builder.Services.AddSingleton<UserSessionService>();

        // Utils
        builder.Services.AddSingleton<ExceptionHandlerUtil>();

        return builder.Build();
    }

    private static void AddConfiguredHttpClient<T>(IServiceCollection services) where T : class
    {
        services.AddHttpClient<T>(client =>
        {
            client.DefaultRequestHeaders.Add("Accept", "application/json");
            client.DefaultRequestHeaders.Add("Accept-Encoding", "gzip, deflate, br");
            client.DefaultRequestHeaders.Add("Connection", "keep-alive");
        });
    }
}
