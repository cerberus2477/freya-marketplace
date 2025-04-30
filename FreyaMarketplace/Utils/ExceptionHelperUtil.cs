namespace FreyaMarketplace.Utils;

public static class ExceptionHelperUtil
{
    public static string GetFriendlyMessage(Exception ex)
    {
        var lowerMessage = ex.Message.ToLowerInvariant();
        if ((lowerMessage.Contains("127.0.0.1") && lowerMessage.Contains("visszautasította a kapcsolatot")) ||
    (lowerMessage.Contains("nem hozható létre kapcsolat") && lowerMessage.Contains("127.0.0.1")))
        {
            string port = GetPortFromApiUrl() ?? "8069";
            return $"Nem sikerült csatlakozni az API-hoz. Kérlek, ellenőrizd, hogy az API szerver el van-e indítva a 127.0.0.1:{port} címen.";
        }

        if ((lowerMessage.Contains("connection refused") || lowerMessage.Contains("célszámítógép már visszautasította a kapcsolatot")) ||
            (lowerMessage.Contains("nem hozható létre kapcsolat") && lowerMessage.Contains("2002")))
        {
            return "Nem sikerült csatlakozni az adatbázishoz. Kérlek, ellenőrizd, hogy elindítottad-e a XAMPP kezelőpanelen az Apache és MySQL szolgáltatásokat.";
        }



        if (ex is JsonException)
        {
            return "Hibás válaszformátum az API-tól";
        }
        return null;
    }

    private static string GetPortFromApiUrl()
    {
        try
        {
            var uri = new Uri(AppSettings.ApiBaseUrl);
            return uri.Port.ToString();
        }
        catch
        {
            return null;
        }
    }
}
