namespace FreyaMarketplace.Services;

public class AuthenticationService
{
    private readonly HttpClient httpClient;
    private readonly JsonSerializerOptions jsonOptions;

    public AuthenticationService(HttpClient httpClient, JsonSerializerOptions jsonSerializerOptions)
    {
        this.httpClient = httpClient;
        jsonOptions = jsonSerializerOptions;
    }


    public async Task<LoginApiResponse> LoginAsync(string userEmail, string userPassword)
    {
        var url = $"{AppSettings.ApiBaseUrl}login";
        var requestData = new { email = userEmail, password = userPassword };
        var content = new StringContent(JsonSerializer.Serialize(requestData), Encoding.UTF8, "application/json");

        // Sending the request
        try
        {
            var response = await httpClient.PostAsync(url, content);
            var responseText = await response.Content.ReadAsStringAsync();
            Debug.WriteLine($"\n\nPOST Login request sent to API.");

            return JsonSerializer.Deserialize<LoginApiResponse>(responseText, jsonOptions);
        }
        catch (Exception ex)
        {
            return new LoginApiResponse(500, ExceptionHelperUtil.GetFriendlyMessage(ex) ?? $"Váratlan hiba történt a bejelentkezés során. ({ex.Message})", new ExceptionLoginData());
        }
    }


    public async Task<SignupApiResponse> SignupAsync(string username, string userEmail, string userPassword, string userPasswordConfirmation)
    {
        var url = $"{AppSettings.ApiBaseUrl}register";
        var requestData = new {username = username, email = userEmail, password = userPassword, password_confirmation = userPasswordConfirmation};
        var content = new StringContent(JsonSerializer.Serialize(requestData), Encoding.UTF8, "application/json");

        // Sending the request
        try
        {
            var response = await httpClient.PostAsync(url, content);
            var responseText = await response.Content.ReadAsStringAsync();
            Debug.WriteLine($"\n\nPOST Register request sent to API.");
            
            return JsonSerializer.Deserialize<SignupApiResponse>(responseText, jsonOptions);
        }
        catch (Exception ex)
        {
            return new SignupApiResponse(500, ExceptionHelperUtil.GetFriendlyMessage(ex) ?? $"Váratlan hiba történt a regisztráció során. ({ex.Message})");
        }
    }
}