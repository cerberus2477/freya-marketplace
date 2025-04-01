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


    public async Task<LoginApiResponse> LoginAsync(string userEmail, string userPassword)
    {
        var url = $"{AppSettings.ApiBaseUrl}login";
        var requestData = new { email = userEmail, password = userPassword };
        var content = new StringContent(JsonSerializer.Serialize(requestData), Encoding.UTF8, "application/json");

        var response = await httpClient.PostAsync(url, content);
        var responseText = await response.Content.ReadAsStringAsync();
        Debug.WriteLine($"Raw API Response: {responseText}");

        try
        {
            var loginApiResponse = JsonSerializer.Deserialize<LoginApiResponse>(responseText, jsonOptions);

            if (loginApiResponse != null)
            {
                Debug.WriteLine($"Received API Response: {JsonSerializer.Serialize(loginApiResponse)}");

                if (loginApiResponse.Data is ValidationErrorData errorData)
                {
                    foreach (var error in errorData.Errors)
                    {
                        Debug.WriteLine($"Field: {error.Key}, Errors: {string.Join(", ", error.Value)}");
                    }
                }

                return loginApiResponse;
            }
            else
            {
                return new LoginApiResponse(500, "Hibás válaszformátum az API-tól");
            }
        }
        catch (JsonException ex)
        {
            Debug.WriteLine($"JSON feldolgozási hiba: {ex.Message}");
            return new LoginApiResponse(500, "Hibás válaszformátum az API-tól");
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Váratlan hiba: {ex.Message}");
            return new LoginApiResponse(500, "Váratlan hiba történt a bejelentkezés során") ;
        }
    }
}

//try interpreting the response as either succesfull or unsiccesful login,
//catch it validation errors are returned (422), because the format of the data is incorrect (e.g. too short password or not an actual email address)
//generic error to be displayed in view. might be useful to uncomment and implemtent the errorhandling function in baseviewmodel



// Deserialize status & message first
//var baseResponse = JsonSerializer.Deserialize<LoginApiResponse<object>>(responseText, jsonOptions);
//if (baseResponse == null)
//{
//    return UnexpectedResponse;
//}

//try
//{
//    switch (response.StatusCode)
//    {
//        case HttpStatusCode.OK:
//            //return JsonSerializer.Deserialize<ApiResponse<LoginData>>(responseText, jsonOptions);
//            var successData = JsonSerializer.Deserialize<LoginApiResponse<LoginSuccessData>>(responseText, jsonOptions);
//            Debug.WriteLine($"Success Response Type: {successData.GetType()}");
//            Debug.WriteLine($"Success Response Data: {JsonSerializer.Serialize(successData)}");
//            return successData;

//        case HttpStatusCode.Unauthorized:
//            //return new ApiResponse<object>
//            //{
//            //    Status = baseResponse.Status,
//            //    Message = baseResponse.Message,
//            //    Data = null
//            //};
//            Debug.WriteLine($"Unauthorized Response: {JsonSerializer.Serialize(baseResponse)}");
//            return new ApiResponse<object>(
//                 baseResponse?.Status ?? 401,
//                baseResponse?.Message ?? "Unauthorized");



//        case HttpStatusCode.UnprocessableEntity:
//            //return JsonSerializer.Deserialize<ApiResponse<ValidationErrorData>>(responseText, jsonOptions);
//            var errorData = JsonSerializer.Deserialize<LoginApiResponse<ValidationErrorData>>(responseText, jsonOptions);
//            Debug.WriteLine($"Validation Error Response Type: {errorData.GetType()}");
//            Debug.WriteLine($"Validation Error Response Data: {JsonSerializer.Serialize(errorData)}");

//            if (errorData?.Data?.Errors != null)
//            {
//                foreach (var error in errorData.Data.Errors)
//                {
//                    Debug.WriteLine($"Field: {error.Key}, Errors: {string.Join(", ", error.Value)}");
//                }
//            }

//            return errorData;

//        default:
//            return new LoginApiResponse<object>
//            (
//                 (int)response.StatusCode,
//                "Váratlan válasz érkezett az API-tól");

//    }
//}








    //failed to convert apiresponse to useful data. structure differs from expected
    //private readonly LoginApiResponse UnexpectedResponse = new LoginApiResponse(500, "Nem várt válasz");