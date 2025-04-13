using System.Net.Http.Json;
using System.Text;

namespace FreyaMarketplace.Services;

public class ListingService
{
    HttpClient httpClient;
    public ListingService()
    {
        this.httpClient = new HttpClient();
    }

    List<Listing> listings;

    public async Task<List<Listing>> SearchListings(string query = "")
    {
        //constructing the url
        var url = $"{AppSettings.ApiBaseUrl}listings/search?pageSize=all";

        if (!string.IsNullOrWhiteSpace(query))
        {
            url += $"&q={Uri.EscapeDataString(query)}";
        }
        //TODO: implement filters. (q will be rewritten probably, because it is handled in a similar way to filters. probably a loop of some kind

        try
        {
            var response = await httpClient.GetAsync(url);
            var responseText = await response.Content.ReadFromJsonAsync(ApiResponse.Defaul);
            Debug.WriteLine($"\n\nGET Listings request sent to API.\nRaw response: {responseText}");

            var ApiResponse = JsonSerializer.Deserialize<ApiResponse>(responseText);
            if (response.IsSuccessStatusCode)
            {
                //this would be correct if the response was a list of listings, but we have the list of listings wrapped in a "data" field.
                listings = await response.Content.ReadFromJsonAsync(ListingContext.Default.ListListing);
                //i think the response should be decoded into an apiresponse object, which has a status, a message and a data field. 
                //after that we extract the listings.
            }

            else { 
                await exceptionHandlerUtil.HandleExceptionAsync(
                    new Exception($"API válasz: {response.StatusCode}"),
                    "Nem sikerült lekérni a hirdetéseket.");
                return new List<Listing>();
            }


            //var ApiResponse = JsonSerializer.Deserialize<ApiResponse>(responseText);
            //Debug.WriteLine($"Found listings: {});


            //else "Hibás válaszformátum az API-tól"

        }
        catch (JsonException ex)
        {
            
        }
        catch (Exception ex)
        {
            
        }

        
       
    }


}



//    public async Task<List<Listing>> SearchListings(string query = "")
//    {
//        var url = $"{AppSettings.ApiBaseUrl}listings/search?all";
//        if (!string.IsNullOrWhiteSpace(query))
//            url += $"&q={Uri.EscapeDataString(query)}";

//        try
//        {
//            var response = await httpClient.GetAsync(url);
//            var responseText = await response.Content.ReadAsStringAsync();

//            Debug.WriteLine($"\n\nGET Listings request sent to API.\nRaw response: {responseText}");

//            if (!response.IsSuccessStatusCode)
//            {
//                await exceptionHandlerUtil.HandleExceptionAsync(
//                    new Exception($"API válasz: {response.StatusCode}"),
//                    "Nem sikerült lekérni a hirdetéseket.");
//                return new List<Listing>();
//            }

//            try
//            {
//                var listings = JsonSerializer.Deserialize<List<Listing>>(responseText, jsonOptions);

//                if (listings == null)
//                {
//                    await exceptionHandlerUtil.HandleExceptionAsync(
//                        new Exception("Váratlan API válaszformátum"),
//                        "Hiba történt a hirdetések feldolgozása során.");
//                    return new List<Listing>();
//                }

//                return listings;
//            }
//            catch (JsonException)
//            {
//                await exceptionHandlerUtil.HandleExceptionAsync(
//                    new Exception("Váratlan API válaszformátum"),
//                    "Hiba történt a hirdetések feldolgozása során.");
//                return new List<Listing>();
//            }
//        }
//        catch (Exception ex)
//        {
//            await exceptionHandlerUtil.HandleExceptionAsync(
//                ex,
//                "Hiba történt a hirdetések lekérése során.");
//            return new List<Listing>();
//        }
//    }
//}



//TODO: If you want to inject HttpClient properly for testing/DI, you can later refactor it using IHttpClientFactory.