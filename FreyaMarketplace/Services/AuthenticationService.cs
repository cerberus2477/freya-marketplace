using System.Buffers.Text;
using System.Net.Http.Json;
using System.Text;

namespace FreyaMarketplace.Services;

public class AuthenticationService
{
    HttpClient httpClient;
    public AuthenticationService()
    {
        this.httpClient = new HttpClient();
        httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
        httpClient.DefaultRequestHeaders.Add("Accept-Encoding", "gzip, deflate, br");
        httpClient.DefaultRequestHeaders.Add("Connection", "keep-alive");
    }

    LoginData logindata;

    public async Task<LoginData> LoginAsync(string email, string password)
    {
        var url = $"{AppSettings.ApiBaseUrl}login";

        var request = new
        {
            Email = email,
            Password = password
        };

        //var json = JsonConvert.SerializeObject(loginData);
        var json = JsonSerializer.Serialize(request);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await httpClient.PostAsync(url, content);
        if (response.IsSuccessStatusCode)
        {
            var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse<LoginData>>();

            if (apiResponse?.Status == 200)
            {
                logindata = apiResponse.Data;

            }
            else
            {
                // If the status is not 200, display the error message
                Console.WriteLine($"Error: {apiResponse?.Message}");

            }
        }

        return logindata;
    }


    public class LoginData
    {
        public User User { get; set; }
        public string Token { get; set; }
    }

    
}