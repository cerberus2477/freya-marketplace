using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using System.Threading.Tasks;

namespace FreyaMarketplace.Utils;

public static class ToastUtil
{
    public static async Task ShowToastAsync(string message, bool warning = false)
    {
        var prefix = warning ? "❌ " : "";
        var snackbar = Snackbar.Make(
            prefix + message,
            duration: TimeSpan.FromSeconds(2),
            visualOptions: new SnackbarOptions
            {
                BackgroundColor = warning ? Color.FromArgb("#D84D49") : Color.FromArgb("#426b1f"),
                TextColor = Colors.White,
                CornerRadius = 8
            }
        );

        await snackbar.Show();
    }
}
