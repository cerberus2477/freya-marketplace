using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Text.Json.Serialization;
using Windows.Networking.NetworkOperators;

namespace FreyaMarketplace.Model;


public class ProfileApiResponse : IApiResponse
{
    public int Status { get; set; }
    public string Message { get; set; }

    [JsonConverter(typeof(ProfileDataJsonConverter))] // Use custom converter to convert either to ProfileData
    public IProfileData Data { get; set; }

    public ProfileApiResponse(int status, string message, IProfileData data = null)
    {
        Status = status;
        Message = message;
        Data = data ?? new EmptyProfileData();
    }
}

public interface IProfileData : IData
{
}


public class ProfileSuccessData : IProfileData
{
    [JsonInclude]
    public User User { get; set; }
}

public class EmptyProfileData : IProfileData
{
}

public class ProfileValidationErrorData : IProfileData
{
    public Dictionary<string, List<string>> Errors { get; set; }
}