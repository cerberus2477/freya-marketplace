using System.Text.Json.Serialization;

namespace FreyaMarketplace.Model;

public class SignupApiResponse : IApiResponse
{
    public int Status { get; set; }
    public string Message { get; set; }

    [JsonConverter(typeof(SignupDataJsonConverter))] // Use custom converter to convert either to SignupSuccessData or ValidationErrorData
    public ISignupData Data { get; set; }

    public SignupApiResponse(int status, string message, ISignupData data = null)
    {
        Status = status;
        Message = message;
        Data = data;
        //Data = data ?? new EmptyLoginData();
        //TODO: kell ide az emtpy login data? elvileg ez nem adhat vissza ilyet
    }
}


public interface ISignupData : IData
{
}


public class SignupSuccessData : ISignupData
{
    public User User { get; set; }
    public string Token { get; set; }
}

public class SignupValidationErrorData : ISignupData
{
    public Dictionary<string, List<string>> Errors { get; set; }
}