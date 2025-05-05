namespace FreyaMarketplace.Services;

public class ListingService
{
    private readonly HttpClient httpClient;
    private readonly JsonSerializerOptions jsonOptions;
    private readonly ExceptionHandlerUtil exceptionHandlerUtil;
    private readonly UserSessionService userSessionService;
    public ListingService(HttpClient httpClient, JsonSerializerOptions jsonSerializerOptions, ExceptionHandlerUtil exceptionHandlerUtil, UserSessionService userSessionService)
    {
        this.httpClient = httpClient;
        this.jsonOptions = jsonSerializerOptions;
        this.exceptionHandlerUtil = exceptionHandlerUtil;
        this.userSessionService = userSessionService;
    }

    List<Listing> listings;

    //TODO: option to get paginated results, e.g only first 4 listings (homepage and profilepage sneak peak of userslistings, or e.g. same city listings)
    public async Task<List<Listing>> SearchListings(string query = "", string username = "")
    {
        // Constructing the url
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

        // Sending the request
        try
        {
            var response = await httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                var responseText = await response.Content.ReadAsStringAsync();
                Debug.WriteLine($"\n\nGET Listings request sent to API.\nRaw response: {responseText}");
                var listingsApiResponse = JsonSerializer.Deserialize<ListingsApiResponse>(responseText, jsonOptions);

                listings = listingsApiResponse.Data;
            }

            else
            {
                //TODO: put this in exceptionhelper 
                await exceptionHandlerUtil.HandleExceptionAsync(new Exception($"GET Listings request sent to API.\nResponse status: {response.StatusCode}"), "Nem sikerült lekérni a hirdetéseket, mert az API nem 200 (OK) választ adott vissza.");
            }

        }
        catch (Exception ex)
        {
            await exceptionHandlerUtil.HandleExceptionAsync(ex, "Váratlan hiba történt a hirdetések lekérése közben.");
        }

        return listings;
    }


    public async Task<PostPatchListingApiResponse> UpdateListingAsync(Listing oldListing, string title, string description, string city, decimal price, List<FileResult> localFiles, List<string> remoteUrls)
    {
        var url = $"{AppSettings.ApiBaseUrl}listings/{oldListing.Id}";

        // Checking whether login_token is valid
        var token = await userSessionService.GetAuthTokenAsync();
        if (string.IsNullOrEmpty(token))
        {
            return new PostPatchListingApiResponse(401, "Kérjük jelentkezz be újra.");
        }

        // Constructing the request content (only the fields that are different from the old ones are sent)
        var content = new MultipartFormDataContent();

        if(title != oldListing.Title)
            content.Add(new StringContent(title), "title");

        if (description != oldListing.Description)
            content.Add(new StringContent(description), "description");

        if (city != oldListing.City)
            content.Add(new StringContent(city), "city");

        if (price != oldListing.Price)
            content.Add(new StringContent(price.ToString()), "price");

        // If no fields changed, no images uploaded, remote images stayed the same (no deletion) skip the request
        bool remoteImagesUnchanged = oldListing.Media.OrderBy(x => x).SequenceEqual((remoteUrls ?? new()).OrderBy(x => x));

        if (!content.Any() && (localFiles?.Count ?? 0) == 0 && remoteImagesUnchanged)
        {
            return new PostPatchListingApiResponse(200, "Nem történt változás, frissítés kihagyva.");
        }

        // Adding both the newly uploaded images and the previous undeleted images (url) to the request content as files
        await AddImagesToMultipartContentAsync(content, localFiles, remoteUrls);

        //TODO: remove debug
        Debug.WriteLine("Multipart Content:");

        foreach (var part in content)
        {
            if (part is StringContent stringContent)
            {
                Debug.WriteLine($"Field: {part.Headers.ContentDisposition?.Name} = {await stringContent.ReadAsStringAsync()}");
            }
            else if (part is StreamContent streamContent)
            {
                var name = part.Headers.ContentDisposition?.Name;
                var fileName = part.Headers.ContentDisposition?.FileName;
                Debug.WriteLine($"File: {name}, Filename: {fileName}, Content-Type: {part.Headers.ContentType}");
            }
        }

        // Forming the request
        var request = new HttpRequestMessage(HttpMethod.Patch, url)
        {
            Headers = { Authorization = new AuthenticationHeaderValue("Bearer", token) },
            Content = content
        };

        // Sending the request
        try
        {
            var response = await httpClient.SendAsync(request);
            var responseText = await response.Content.ReadAsStringAsync();
            Debug.WriteLine($"\n\nhListing patch request sent to API.\nRaw response: {responseText}");

            return JsonSerializer.Deserialize<PostPatchListingApiResponse>(responseText, jsonOptions);
        }
        catch (Exception ex)
        {
            return new PostPatchListingApiResponse(500, ExceptionHelperUtil.GetFriendlyMessage(ex) ?? $"Váratlan hiba történt a hirdetés növény/státusz módosítása során. ({ex.Message})");
        }
    }


    public async Task<PostPatchListingApiResponse> CreateListingAsync(int userplantId, string title, string description, string city, decimal price, List<FileResult> localFiles)
    {
        var url = $"{AppSettings.ApiBaseUrl}listings";

        // Checking whether login_token is valid
        var token = await userSessionService.GetAuthTokenAsync();
        if (string.IsNullOrEmpty(token))
        {
            return new PostPatchListingApiResponse(401, "Kérjük jelentkezz be újra.");
        }

        // Constructing the request content (multipart/form-data)
        using var content = new MultipartFormDataContent
        {
            { new StringContent(userplantId.ToString()), "user_plants_id" },
            { new StringContent(title), "title" },
            { new StringContent(description), "description" },
            { new StringContent(city), "city" },
            { new StringContent(price.ToString()), "price" }
        };

        // Adding the images to the request content
        await AddImagesToMultipartContentAsync(content, localFiles);

        //TODO: remove log
        Debug.WriteLine("Multipart Content:");

        foreach (var part in content)
        {
            if (part is StringContent stringContent)
            {
                Debug.WriteLine($"Field: {part.Headers.ContentDisposition?.Name} = {await stringContent.ReadAsStringAsync()}");
            }
            else if (part is StreamContent streamContent)
            {
                var name = part.Headers.ContentDisposition?.Name;
                var fileName = part.Headers.ContentDisposition?.FileName;
                Debug.WriteLine($"File: {name}, Filename: {fileName}, Content-Type: {part.Headers.ContentType}");
            }
        }

        // Forming the request
        var request = new HttpRequestMessage(HttpMethod.Post, url)
        {
            Headers = { Authorization = new AuthenticationHeaderValue("Bearer", token) },
            Content = content
        };
        
        // Sending the request
        try
        {
            var response = await httpClient.SendAsync(request);
            var responseText = await response.Content.ReadAsStringAsync();
            Debug.WriteLine($"\n\nPOST (Create) Listing request sent to API.\nRaw response: {responseText}");

            return JsonSerializer.Deserialize<PostPatchListingApiResponse>(responseText, jsonOptions);
        }
        catch (Exception ex)
        {
            return new PostPatchListingApiResponse(500, ExceptionHelperUtil.GetFriendlyMessage(ex) ?? $"Váratlan hiba történt a hirdetés létrehozása során. ({ex.Message})");
        }
    }


    private async Task AddImagesToMultipartContentAsync(
        MultipartFormDataContent content,
        List<FileResult> localFiles,
        List<string>? remoteUrls = null)
    {
        // Add local images
        if (localFiles != null)
        {
            foreach (var file in localFiles)
            {
                var stream = await file.OpenReadAsync();
                var streamContent = new StreamContent(stream);

                var extension = Path.GetExtension(file.FileName)?.ToLowerInvariant();
                var mimeType = extension switch
                {
                    ".jpg" or ".jpeg" => "image/jpeg",
                    ".png" => "image/png",
                    ".webp" => "image/webp",
                    ".gif" => "image/gif",
                    _ => "application/octet-stream"
                };

                streamContent.Headers.ContentType = new MediaTypeHeaderValue(mimeType);
                content.Add(streamContent, "media[]", file.FileName);
            }
        }

        // Add remote images (optional)
        if (remoteUrls != null && remoteUrls.Any())
        {
            foreach (var imgUrl in remoteUrls)
            {
                try
                {
                    var stream = await httpClient.GetStreamAsync(imgUrl);
                    var streamContent = new StreamContent(stream);

                    var extension = Path.GetExtension(imgUrl)?.ToLowerInvariant();
                    var mimeType = extension switch
                    {
                        ".jpg" or ".jpeg" => "image/jpeg",
                        ".png" => "image/png",
                        ".webp" => "image/webp",
                        ".gif" => "image/gif",
                        _ => "application/octet-stream"
                    };

                    streamContent.Headers.ContentType = new MediaTypeHeaderValue(mimeType);
                    var fileName = Path.GetFileName(new Uri(imgUrl).AbsolutePath);
                    content.Add(streamContent, "media[]", fileName);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Failed to fetch remote image: {imgUrl} — {ex.Message}");
                }
            }
        }
    }


}