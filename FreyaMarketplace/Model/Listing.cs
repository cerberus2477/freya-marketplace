using System.Text.Json.Serialization;

namespace FreyaMarketplace.Model
{
    public class Listing
    {
        //datafields of listings, so not all of them
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Media { get; set; }

        [JsonPropertyName("sell")]
        public int SellInt { get; set; }

        [JsonIgnore]
        public bool Sell
        {
            get => SellInt == 1;
            set => SellInt = value ? 1 : 0;
        }

        public decimal Price { get; set; }
        public DateTime CreatedAt { get; set; }
        public string User { get; set; }
        public string Plant { get; set; }
        public string Type { get; set; }
        public string Stage { get; set; }
    }

    [JsonSerializable(typeof(List<Listing>))]
    internal sealed partial class ListingContext : JsonSerializerContext
    {

    }
}
