using System.Text.Json.Serialization;

namespace FreyaMarketplace.Model;

public class Stage
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }
}


[JsonSerializable(typeof(List<Stage>))]
internal sealed partial class StageContext : JsonSerializerContext
{
}
