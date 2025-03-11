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

    public async Task<List<Listing>> GetListings()
    {
        if (listings?.Count > 0)
            return listings;

        // Call API for listings
        var response = await httpClient.GetAsync($"{AppSettings.ApiBaseUrl}listings?all");
        if (response.IsSuccessStatusCode)
        {
            var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse>();

            // Check if the response status is 200 (success)
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

public class ApiResponse
{
    public int Status { get; set; }
    public string Message { get; set; }
    public List<Listing> Data { get; set; }
}



// Offline - use for testing without api
/*using var stream = await FileSystem.OpenAppPackageFileAsync("Listingdata.json");
using var reader = new StreamReader(stream);
var contents = await reader.ReadToEndAsync();
listings = JsonSerializer.Deserialize(contents, ListingContext.Default.ListListing);

return listings;*/



