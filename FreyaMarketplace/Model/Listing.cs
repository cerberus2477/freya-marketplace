namespace FreyaMarketplace.Model;

public class Listing
{
    [JsonPropertyName("listing_id")]
    public int Id { get; set; }

    [JsonPropertyName("title")]
    public string Title { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; }

    [JsonPropertyName("city")]
    public string City { get; set; }

    [JsonPropertyName("media")]
    public List<string> Media { get; set; } = new();

    //get the first image of the image list to display as the thumnail (when viewing listings in a list view).
    public string Thumbnail => Media?.FirstOrDefault();

    [JsonPropertyName("price")]
    public int Price { get; set; }

    [JsonPropertyName("created_at")]
    public DateTime CreatedAt { get; set; }

    [JsonIgnore]
    public string CreatedAtFormatted => ConverterUtil.GetRelativeTime(CreatedAt);

    [JsonIgnore]
    public string CreatedAtFormattedLong => ConverterUtil.GetRelativeDateTime(CreatedAt);

    [JsonPropertyName("updated_at")]
    public DateTime UpdatedAt { get; set; }

    [JsonIgnore]
    public string UpdatedAtFormatted => ConverterUtil.GetRelativeTime(UpdatedAt);

    [JsonIgnore]
    public string UpdatedAtFormattedLong => ConverterUtil.GetRelativeDateTime(UpdatedAt);

    [JsonPropertyName("user")]
    public ListingUser User { get; set; }

    [JsonIgnore]
    public string Username => User?.Username;

    [JsonPropertyName("plant")]
    public ListingPlant Plant { get; set; }

    [JsonIgnore]
    public string PlantName => Plant?.Name ?? "";

    [JsonIgnore]
    public string PlantType => Plant?.Type ?? "";

    [JsonPropertyName("stage")]
    public Stage Stage { get; set; }

    [JsonIgnore]
    public string StageName => Stage?.Name ?? "";

    [JsonPropertyName("user_plant")]
    public ListingUserplant Userplant { get; set; }

    [JsonIgnore]
    public int Count => Userplant.Count;


}

public class ListingUser
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("username")]
    public string Username { get; set; }

    [JsonPropertyName("city")]
    public string City { get; set; }

    [JsonPropertyName("email")]
    public string Email { get; set; }
}

public class ListingUserplant
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("count")]
    public int Count { get; set; }

}

public class ListingPlant
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("type")]
    public string Type { get; set; }
}



[JsonSerializable(typeof(List<Listing>))]
internal sealed partial class ListingContext : JsonSerializerContext
{
}
