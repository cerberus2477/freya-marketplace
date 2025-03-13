using Microsoft.Extensions.Logging;
using FreyaMarketplace.Services;
using FreyaMarketplace.View;

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
		builder.Services.AddSingleton<MonkeyService>();
		builder.Services.AddSingleton<MonkeysViewModel>();
        builder.Services.AddTransient<ListingDetailsViewModel>();
        builder.Services.AddTransient<ListingsViewModel>();
        builder.Services.AddSingleton<ListingService>();


        return builder.Build();
	}
}
