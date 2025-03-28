using System.Net.Http.Json;

namespace FreyaMarketplace.Services;

public class ListingService
{
    HttpClient httpClient;
    public ListingService()
    {
        this.httpClient = new HttpClient();
    }

    List<Listing> listings;

    public async Task<List<Listing>> GetListings(string query = "")
    {
        var url = $"{AppSettings.ApiBaseUrl}listings/search?all";
        if (!string.IsNullOrWhiteSpace(query))
        {
            url += $"&q={Uri.EscapeDataString(query)}";
        }

        var response = await httpClient.GetAsync(url);
        if (response.IsSuccessStatusCode)
        {
            var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse<List<Listing>>>();

            if (apiResponse?.Status == 200)
            {
                listings = apiResponse.Data;

            }
            else
            {
                // If the status is not 200, display the error message
                Console.WriteLine($"Error: {apiResponse?.Message}");

            }
        }
        return listings;
    }
}