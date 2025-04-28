using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreyaMarketplace.Utils
{
    public class ExceptionHandlerUtil
    {
        // Helper method for common error handling
        public async Task HandleExceptionAsync(Exception ex, string message = "A kért kérés teljesítése közben hiba állt fel", string title = "Sikertelen művelet", bool displayExMessage = true, [CallerMemberName] string caller = null)
        {
            Debug.WriteLine($"Error in {caller}: {ex.Message}\nException:{ex}");
            if (displayExMessage)
            {
                message += ex.Message;
            }
            try
            {
                if (MainThread.IsMainThread)
                {
                    await Shell.Current.DisplayAlert(title, message, "OK");
                }
                else
                {
                    MainThread.BeginInvokeOnMainThread(async () =>
                    {
                        await Shell.Current.DisplayAlert(title, message, "OK");
                    });
                }
            }
            catch (Exception innerEx)
            {
                Debug.WriteLine($"Failed to show DisplayAlert: {innerEx.Message}");
                // Even if displaying alert fails, don't crash the app
            }
        }
    }
}
