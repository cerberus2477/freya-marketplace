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
    //todo:
    //ez az újabb, proóbálja kezelni a 3 féle responset (amiből kettő logikus is.)
    //errort visszaad,nem display, hogy a viewmodel kezelje.

    //public async Task<ApiResponse<LoginData>> LoginAsync(string email, string password)
    //{
    //    var url = $"{AppSettings.ApiBaseUrl}login";
    //    var request = new { Email = email, Password = password };
    //    var content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");

    //    var response = await httpClient.PostAsync(url, content);
    //    var responseText = await response.Content.ReadAsStringAsync();

    //    try
    //    {
    //        var apiResponse = JsonSerializer.Deserialize<ApiResponse<LoginData>>(responseText, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

    //        if (apiResponse != null && apiResponse.Status == 200 && apiResponse.Data != null)
    //        {
    //            return apiResponse;
    //        }
    //        else
    //        {
    //            return new ApiResponse<LoginData> { Status = response.StatusCode, Message = apiResponse?.Message ?? "Unknown error" };
    //        }
    //    }
    //    catch (JsonException)
    //    {
    //        // Handle validation errors (response does not have "status" field)
    //        var errorResponse = JsonSerializer.Deserialize<ApiValidationError>(responseText, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
    //        return new ApiResponse<LoginData> { Status = response.StatusCode, Message = errorResponse?.Message ?? "Validation error" };
    //    }
    //}

    //public class ApiValidationError
    //{
    //    public string Message { get; set; }
    //    public Dictionary<string, string[]> Errors { get; set; }
    //}



    //clean lenne, de nem kezeli a sokféle responsunkot

    //public async Task<ApiResponse<LoginData>> LoginAsync(string email, string password)
    //{
    //    var url = $"{AppSettings.ApiBaseUrl}login";
    //    var request = new { Email = email, Password = password };
    //    var content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");

    //    var response = await httpClient.PostAsync(url, content);
    //    var responseText = await response.Content.ReadAsStringAsync();

    //    try
    //    {
    //        var apiResponse = JsonSerializer.Deserialize<ApiResponse<LoginData>>(responseText, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

    //        if (apiResponse != null && apiResponse.Status == 200 && apiResponse.Data != null)
    //        {
    //            return apiResponse;
    //        }
    //        else
    //        {
    //            return new ApiResponse<LoginData> { Status = response.StatusCode, Message = apiResponse?.Message ?? "Unknown error" };
    //        }
    //    }
    //    catch (JsonException)
    //    {
    //        // Handle validation errors (response does not have "status" field)
    //        var errorResponse = JsonSerializer.Deserialize<ApiValidationError>(responseText, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
    //        return new ApiResponse<LoginData> { Status = response.StatusCode, Message = errorResponse?.Message ?? "Validation error" };
    //    }
    //}




    //ez amit én frankensteineltem össze az andrás-levi féléből
    //uj, regebbi, legregebbi sorban
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
    //might need this
    //builder.Services.AddSingleton<HttpClient>(); 
    public class LoginData
    {
        public User User { get; set; }
        public string Token { get; set; }
    }

    
}