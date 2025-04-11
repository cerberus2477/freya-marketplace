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
        var url = $"{AppSettings.ApiBaseUrl}listings/search?all";
        if (!string.IsNullOrWhiteSpace(query))
        {
            url += $"&q={Uri.EscapeDataString(query)}";
        }

        try
        {
            var response = await httpClient.GetAsync(url);
            var responseText = await response.Content.ReadFromJsonAsync(ApiResponse);
            Debug.WriteLine($"\n\nGET Listings request sent to API.\nRaw response: {responseText}");


            if (!response.IsSuccessStatusCode)
            {
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

        //if (response.IsSuccessStatusCode)
        //{
            //TODO: make costum parser or just rewrite this 
            //var apiResponse = await response.Content.ReadFromJsonAsync<LoginApiResponse<List<Listing>>>();

            //if (apiResponse?.Status == 200)
            //{
            //    listings = apiResponse.Data;

            //}
            //else
            //{
            //    // If the status is not 200, display the error message
            //    Console.WriteLine($"Error: {apiResponse?.Message}");

            //}
        //}
       
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