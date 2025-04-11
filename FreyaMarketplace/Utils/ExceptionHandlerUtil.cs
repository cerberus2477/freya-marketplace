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
        public async Task HandleExceptionAsync(Exception ex, string customMessage = null, bool displayExMessage = true, [CallerMemberName] string caller = null)
        {
            Debug.WriteLine($"Error in {caller}: {ex.Message}\nException:{ex}");
            string message = customMessage ?? "A kért kérés teljesítése közben hiba állt fel.";
            if (displayExMessage)
            {
                message += $"\nHibaüzenet: {ex.Message}";
            }
            await Shell.Current.DisplayAlert("Váratlan hiba", message, "OK");
        }
    }
}
