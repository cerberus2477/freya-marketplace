using System.Buffers.Text;
using System.Net.Http.Json;
using System.Text;
using System.Net;


namespace FreyaMarketplace.Services;

public class AuthenticationService
{
    HttpClient httpClient;
    private readonly JsonSerializerOptions jsonOptions;

    public AuthenticationService()
    {
        this.httpClient = new HttpClient();
        httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
        httpClient.DefaultRequestHeaders.Add("Accept-Encoding", "gzip, deflate, br");
        httpClient.DefaultRequestHeaders.Add("Connection", "keep-alive");

        this.jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,  // Allow flexible casing
            AllowTrailingCommas = false,        // No extra commas allowed
            DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.Never // Require all fields
        };
    }


    //failed to convert apiresponse to useful data. structure differs from expected
    private readonly ApiResponse<object> UnexpectedResponse = new ApiResponse<object> { Status = 500, Message = "Nem várt válasz" };


    public async Task<IApiResponse> LoginAsync(string userEmail, string userPassword)
    {
        var url = $"{AppSettings.ApiBaseUrl}login";
        var requestData = new { email = userEmail, password = userPassword };
        var content = new StringContent(JsonSerializer.Serialize(requestData), Encoding.UTF8, "application/json");

        var response = await httpClient.PostAsync(url, content);
        var responseText = await response.Content.ReadAsStringAsync();


        // Deserialize status & message first
        var baseResponse = JsonSerializer.Deserialize<ApiResponse<object>>(responseText, jsonOptions);
        if (baseResponse == null)
        {
            return UnexpectedResponse;
        }

        try
        {
            switch (response.StatusCode)
            {
                case HttpStatusCode.OK:
                    return JsonSerializer.Deserialize<ApiResponse<LoginData>>(responseText, jsonOptions);

                case HttpStatusCode.Unauthorized:
:
                    return new ApiResponse<object>
                    {
                        Status = baseResponse.Status,
                        Message = baseResponse.Message,
                        Data = null
                    };

                case HttpStatusCode.UnprocessableEntity:
                    return JsonSerializer.Deserialize<ApiResponse<ValidationErrorData>>(responseText, jsonOptions);


                default:
                    return new ApiResponse<object>
                    {
                        Status = (int)response.StatusCode,
                        Message = "Váratlan válasz érkezett az API-tól",
                        Data = null
                    };
            }
        }
        catch (JsonException ex)
        {
            Debug.WriteLine($"JSON feldolgozási hiba: {ex.Message}");
            return new ApiResponse<object> { Status = 500, Message = "Hibás válaszformátum az API-tól", Data = null };
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Váratlan hiba: {ex.Message}");
            return new ApiResponse<object> { Status = 500, Message = "Váratlan hiba történt a bejelentkezés során", Data = null };
        }
    }
}

            //try interpreting the response as either succesfull or unsiccesful login,
            //catch it validation errors are returned (422), because the format of the data is incorrect (e.g. too short password or not an actual email address)
            //generic error to be displayed in view. might be useful to uncomment and implemtent the errorhandling function in baseviewmodel