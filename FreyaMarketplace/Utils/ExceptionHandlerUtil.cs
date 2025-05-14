namespace FreyaMarketplace.Utils;
public class ExceptionHandlerUtil
{
    // Helper method for common error handling
    // Debugging to output window and showing user the error in a uniform popup
    public async Task HandleExceptionAsync(Exception ex, string message = "A kért kérés teljesítése közben hiba állt fel", string title = "Sikertelen művelet", bool displayExMessage = true, [CallerMemberName] string caller = null)
    {
        Debug.WriteLine($"Error in {caller}: {ex.Message}\nException: {ex}");

        // Detect specific error types and provide user-readable messages
        string userFriendlyMessage = ExceptionHelperUtil.GetFriendlyMessage(ex);

        if (!string.IsNullOrEmpty(userFriendlyMessage))
        {
            message = userFriendlyMessage;
            displayExMessage = false;
        }
        else if (displayExMessage)
        {
            message += $" ({ex.Message})";
        }

        try
        {
            //UI modifications can only be done on the main thread
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
        }
    }

    public async Task<bool> ConfirmNavigationWithUnsavedChangesAsync()
    {
        return await Shell.Current.DisplayAlert(
            "El nem mentett módosítások",
            "Biztosan el szeretnél navigálni? A mentetlen módosításaid elvesznek.",
            "Igen", "Mégsem");
    }
}
