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




    public async Task<ApiResponse<object>> LoginAsync(string user_email, string user_password)
    {
        var url = $"{AppSettings.ApiBaseUrl}login";
        var request = new { email = user_email, password = user_password };
        var content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");

        var response = await httpClient.PostAsync(url, content);
        var responseText = await response.Content.ReadAsStringAsync();

        try
        {
            // Try parsing the response as a successful login (LoginData)
            var apiResponse = JsonSerializer.Deserialize<ApiResponse<LoginData>>(responseText, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            if (apiResponse != null)
            {
                // Return the apiResponse as is (successful login), but assign the Data to object.
                return new ApiResponse<object>
                {
                    Status = apiResponse.Status,
                    Message = apiResponse.Message,
                    Data = apiResponse.Data // this is of type LoginData
                };
            }
        }
        catch (JsonException)
        {
            // Handle validation errors (422)
            var validationResponse = JsonSerializer.Deserialize<ApiResponse<ValidationErrorData>>(responseText, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            if (validationResponse != null)
            {
                return new ApiResponse<object>
                {
                    Status = validationResponse.Status,
                    Message = validationResponse.Message,
                    Data = validationResponse.Data // this is of type ValidationErrorData
                };
            }
        }

        // Return a generic error if deserialization failed
        return new ApiResponse<object>
        {
            Status = 500,
            Message = "Ismeretlen hiba"
        };
    }
}



            //try interpreting the response as either succesfull or unsiccesful login,
            //catch it validation errors are returned (422), because the format of the data is incorrect (e.g. too short password or not an actual email address)
            //generic error to be displayed in view. might be useful to uncomment and implemtent the errorhandling function in baseviewmodel