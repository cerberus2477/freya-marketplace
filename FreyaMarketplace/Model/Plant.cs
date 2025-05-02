namespace FreyaMarketplace.Model;

public class Plant
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("latin_name")]
    public string LatinName { get; set; }

    [JsonPropertyName("type")]
    public PlantType Type { get; set; }
}

public class PlantType
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }
}


[JsonSerializable(typeof(List<Plant>))]
internal sealed partial class PlantContext : JsonSerializerContext
{
}
