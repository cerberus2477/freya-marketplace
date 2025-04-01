using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Text.Json;
using static System.Runtime.InteropServices.JavaScript.JSType;
namespace FreyaMarketplace.Model;

//a converter that deserializes Data into the correct type(LoginData or ValidationErrorData).
//this is needed becaese our apiresponses data can be of both types, we dont know which before deserialising.

internal class LoginDataJsonConverter : JsonConverter<ILoginData>
{
    public override ILoginData Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        using (JsonDocument doc = JsonDocument.ParseValue(ref reader))
        {
            var root = doc.RootElement;

            // If "Errors" key exists, it's ValidationErrorData
            if (root.TryGetProperty("errors", out _))
            {
                return JsonSerializer.Deserialize<ValidationErrorData>(root.GetRawText(), options);
            }
            // If "User" key exists, it's LoginSuccessData
            else if (root.TryGetProperty("user", out _))
            {
                return JsonSerializer.Deserialize<LoginSuccessData>(root.GetRawText(), options);
            }
            // If "Data" is an empty array or object, return an EmptyLoginData instance
            else if (root.ValueKind == JsonValueKind.Array && root.GetArrayLength() == 0)
            {
                return new EmptyLoginData();
            }
            else if (root.ValueKind == JsonValueKind.Object && !root.EnumerateObject().Any())
            {
                return new EmptyLoginData();
            }
        }
        return new EmptyLoginData(); // Default case
    }

    public override void Write(Utf8JsonWriter writer, ILoginData value, JsonSerializerOptions options)
    {
        JsonSerializer.Serialize(writer, value, value.GetType(), options);
    }
}



