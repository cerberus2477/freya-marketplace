using Microsoft.Extensions.Logging;

namespace FreyaMarketplace;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
			});

#if DEBUG
		builder.Logging.AddDebug();
#endif

        //view model is Transient so a new view model is created each time the page is navigated to
        builder.Services.AddTransient<ListingsViewModel>(); //TODO: can this be a singleton?
        builder.Services.AddSingleton<ListingService>();

        builder.Services.AddTransient<ListingDetailsViewModel>();

        builder.Services.AddTransient<AuthViewModel>();
        builder.Services.AddSingleton<AuthenticationService>();

        builder.Services.AddSingleton<ExceptionHandlerUtil>();
        //builder.Services.AddSingleton<ConverterUtil>();

        builder.Services.AddSingleton<ProfileService>();
        builder.Services.AddTransient<ProfilePage>();
        builder.Services.AddTransient<ProfileViewModel>();

        return builder.Build();
	}
}
