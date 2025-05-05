namespace FreyaMarketplace.Model;

internal class PostPatchUserplantDataJsonConverter : JsonConverter<IPostPatchUserplantData>
{
    public override IPostPatchUserplantData Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        using (JsonDocument doc = JsonDocument.ParseValue(ref reader))
        {
            var data = doc.RootElement;
            Debug.WriteLine($"Root JSON: {data}");

            try
            {
                // Check for validation errors first
                if (data.TryGetProperty("errors", out _))
                {
                    var errorData = JsonSerializer.Deserialize<PostPatchUserplantValidationErrorData>(data.GetRawText(), options);
                    Debug.WriteLine($"Deserialized to: {errorData?.GetType().Name ?? "null"}");
                    return errorData;
                }

                // Check for success (id = userplant)
                if (data.TryGetProperty("id", out _))
                {
                    var userplant = JsonSerializer.Deserialize<Userplant>(data.GetRawText(), options);
                    var successData = new PostPatchUserplantSuccessData { Userplant = userplant };
                    Debug.WriteLine($"Deserialized to: {successData.GetType().Name}");
                    return successData;
                }

                // Empty object or array fallback
                if ((data.ValueKind == JsonValueKind.Object && !data.EnumerateObject().Any()) ||
                    (data.ValueKind == JsonValueKind.Array && data.GetArrayLength() == 0))
                {
                    Debug.WriteLine("Deserialized to: EmptyPostPatchUserplantData (empty object/array)");
                    return new EmptyPostPatchUserplantData();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Exception during deserialization: {ex}");
            }

            Debug.WriteLine("Deserialized to: EmptyPostPatchUserplantData (no matching keys)");
            return new EmptyPostPatchUserplantData();
        }
    }

    public override void Write(Utf8JsonWriter writer, IPostPatchUserplantData value, JsonSerializerOptions options)
    {
        JsonSerializer.Serialize(writer, value, value.GetType(), options);
    }
}