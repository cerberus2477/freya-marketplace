//namespace FreyaMarketplace.Model;

//public class ApiResponse<T> : IApiResponse
//{
//    public int Status { get; set; }
//    public string Message { get; set; }
//    public T Data { get; set; }
//}

using System.Text.Json.Serialization;

namespace FreyaMarketplace.Model;

public class LoginApiResponse : IApiResponse
{
    public int Status { get; set; }
    public string Message { get; set; }

    [JsonConverter(typeof(LoginDataJsonConverter))] // Use custom converter to convert either to LoginData or ValidationErrorData
    public ILoginData Data { get; set; }

    public LoginApiResponse(int status, string message, ILoginData data = null)
    {
        Status = status;
        Message = message;
        Data = data ?? new EmptyLoginData();
    }
}
