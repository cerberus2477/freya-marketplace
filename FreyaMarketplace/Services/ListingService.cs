using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http.Json;
using FreyaMarketplace.Model;


namespace FreyaMarketplace.Services;

public class ListingService
{
    HttpClient _httpClient;
    public ListingService()
    {
        _httpClient = new HttpClient();
    }

    List<Listing> listingList;

    public async Task<List<Listing>> GetListings()
    {
        if (listingList?.Count > 0)
            return listingList;

        // Call API for listings
        var response = await _httpClient.GetAsync($"{AppSettings.ApiBaseUrl}listings?all");

        if (response.IsSuccessStatusCode)
        {
            var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse>();

            // Check if the response status is 200 (success)
            if (apiResponse?.Status == 200)
            {
                listingList = apiResponse.Data;
                return listingList;
            }
            else
            {
                // If the status is not 200, display the error message
                Console.WriteLine($"Error: {apiResponse?.Message}");
                return new List<Listing>(); // Return empty list if error
            }
        }
        else
        {
            // If the response was not successful
            Console.WriteLine("Error: Unable to fetch listings.");
            return new List<Listing>(); // Return empty list on failure
        }
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



