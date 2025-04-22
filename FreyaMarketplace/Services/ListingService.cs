using System.Net.Http.Json;
using System.Text;

namespace FreyaMarketplace.Services;

public class ListingService
{
     HttpClient httpClient;
     JsonSerializerOptions jsonOptions;
     ExceptionHandlerUtil exceptionHandlerUtil;
    public ListingService(ExceptionHandlerUtil exceptionHandlerUtil)
    {
        this.httpClient = new HttpClient();

        this.jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,  // Allow flexible casing
            AllowTrailingCommas = false,        // No extra commas allowed
            DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.Never // Require all fields
        };

        this.exceptionHandlerUtil = exceptionHandlerUtil;
    }

    List<Listing> listings;

    //TODO: option to get paginated results, e.g only first 4 listings (homepage and profilepage sneak peak of userslistings, or e.g. same city listings)
    public async Task<List<Listing>> SearchListings(string query = "", string username = "")
    {

        //constructing the url
        var url = $"{AppSettings.ApiBaseUrl}listings?pageSize=all";

        if (!string.IsNullOrWhiteSpace(query))
        {
            url += $"&q={Uri.EscapeDataString(query)}";
        }
        if (!string.IsNullOrWhiteSpace(username))
        {
            url += $"&user={Uri.EscapeDataString(username)}";
        }
        //TODO: implement filters. (q will be rewritten probably, because it is handled in a similar way to filters. probably a loop of some kind

        try
        {
            var response = await httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                var responseText = await response.Content.ReadAsStringAsync();
                Debug.WriteLine($"\n\nGET Listings request sent to API.\nRaw response: {responseText}");
                var listingsApiResponse = JsonSerializer.Deserialize<ListingsApiResponse>(responseText, jsonOptions);
                Debug.WriteLine($"Deserialized response: \n\tcontent:{JsonSerializer.Serialize(listingsApiResponse)}");

                listings = listingsApiResponse.Data;
            }

            else {

                //TODO: ez valamiért breakeli az appot, és a uion nem jelenik meg az üzenet, csak a debug windowban.
                await exceptionHandlerUtil.HandleExceptionAsync( new Exception($"GET Listings request sent to API.\nResponse status: {response.StatusCode}"), "Nem sikerült lekérni a hirdetéseket, mert az API nem 200 (OK) választ adot vissza.");
            }

        }
        catch (JsonException ex)
        {
            await exceptionHandlerUtil.HandleExceptionAsync(ex, "Hibás válaszformátum az API-tól.");
        }
        catch (Exception ex)
        {
            await exceptionHandlerUtil.HandleExceptionAsync(ex, "Váratlan hiba történt a hirdetések lekérése közben.");
        }

        return listings;

    }

}

//TODO: If you want to inject HttpClient properly for testing/DI, you can later refactor it using IHttpClientFactory.