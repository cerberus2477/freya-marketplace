namespace FreyaMarketplace.Model;

public class Userplant
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("user_id")]
    public int UserId { get; set; }

    [JsonPropertyName("plant_id")]
    public int PlantId { get; set; }

    [JsonPropertyName("stage_id")]
    public int StageId { get; set; }

    [JsonPropertyName("count")]
    public int Count { get; set; }

    [JsonPropertyName("created_at")]
    public DateTime CreatedAt { get; set; }

    [JsonPropertyName("updated_at")]
    public DateTime UpdatedAt { get; set; }
}

[JsonSerializable(typeof(List<Userplant>))]
internal sealed partial class UserplantApiDataContext : JsonSerializerContext
{
}
