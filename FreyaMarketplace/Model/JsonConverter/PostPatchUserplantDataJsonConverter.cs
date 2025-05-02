namespace FreyaMarketplace.Model;

internal class PostPatchUserplantDataJsonConverter : JsonConverter<IPostPatchUserplantData>
{
    public override IPostPatchUserplantData Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        using (JsonDocument doc = JsonDocument.ParseValue(ref reader))
        {
            var data = doc.RootElement;
            Debug.WriteLine($"Root: {data}");

            // If "Data" is an empty array or object, return an EmptyPostPatchUserplantData instance
            if ((data.ValueKind == JsonValueKind.Object && !data.EnumerateObject().Any())
                || (data.ValueKind == JsonValueKind.Array && data.GetArrayLength() == 0))
            {
                return new EmptyPostPatchUserplantData();
            }

            // If "Errors" key exists, it's ValidationErrorData
            if (data.TryGetProperty("errors", out _))
            {
                return JsonSerializer.Deserialize<PostPatchUserplantValidationErrorData>(data.GetRawText(), options);
            }

            // If "id" key exists, it's PostPatchUserplantSuccessData
            else if (data.TryGetProperty("id", out _))
            {
                try
                {
                    // Deserialize the Userplant data directly
                    var Userplant = JsonSerializer.Deserialize<Userplant>(data.GetRawText(), options);
                    return new PostPatchUserplantSuccessData { Userplant = Userplant };
                }
                catch (JsonException ex)
                {
                    Debug.WriteLine($"Error deserializing UserplantApiData data: {ex}");
                    return new EmptyPostPatchUserplantData();
                }
            }
        }

        // Default case
        return new EmptyPostPatchUserplantData();
    }

    public override void Write(Utf8JsonWriter writer, IPostPatchUserplantData value, JsonSerializerOptions options)
    {
        JsonSerializer.Serialize(writer, value, value.GetType(), options);
    }
}
