
using System.Text.Json.Serialization;

namespace FreyaMarketplace.Model;

public class PostPatchUserplantApiResponse : IApiResponse
{
    public int Status { get; set; }
    public string Message { get; set; }

    [JsonConverter(typeof(PostPatchUserplantDataJsonConverter))] // Use custom converter to convert either to ProfileData
    public IPostPatchUserplantData Data { get; set; }

    public PostPatchUserplantApiResponse(int status, string message, IPostPatchUserplantData data = null)
    {
        Status = status;
        Message = message;
        Data = data ?? new EmptyPostPatchUserplantData();
    }
}

public interface IPostPatchUserplantData : IData
{
}


public class PostPatchUserplantSuccessData : IPostPatchUserplantData
{
    [JsonInclude]
    public Userplant Userplant { get; set; }
}

public class EmptyPostPatchUserplantData : IPostPatchUserplantData
{
}

public class PostPatchUserplantValidationErrorData : IPostPatchUserplantData
{
    public Dictionary<string, List<string>> Errors { get; set; }
}
