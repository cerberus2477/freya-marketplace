
namespace FreyaMarketplace.Services;

public class UserplantService
{
    private readonly HttpClient httpClient;
    private readonly JsonSerializerOptions jsonOptions;
    private readonly ExceptionHandlerUtil exceptionHandlerUtil;
    private readonly UserSessionService userSessionService;
    public UserplantService(HttpClient httpClient, JsonSerializerOptions jsonSerializerOptions, ExceptionHandlerUtil exceptionHandlerUtil, UserSessionService userSessionService)
    {
        this.httpClient = httpClient;
        this.jsonOptions = jsonSerializerOptions;
        this.exceptionHandlerUtil = exceptionHandlerUtil;
        this.userSessionService = userSessionService;
    }


    public async Task<PostPatchUserplantApiResponse> UpdateUserplantAsync(Listing oldListing, int plantId, int stageId, int count)
    {
        var url = $"{AppSettings.ApiBaseUrl}profile/plants/{oldListing.Userplant.Id}";

        // Checking whether the user is logged in
        var token = await userSessionService.GetAuthTokenAsync();
        if (string.IsNullOrEmpty(token))
        {
            return new PostPatchUserplantApiResponse(401, "Kérjük jelentkezz be újra.");
        }

        // Check is plantid and stageid are unchanged. only add them to the request if they are different. if none are different then skip
        var patchData = new Dictionary<string, object>();

        if (plantId != oldListing.Plant.Id)
            patchData["plant_id"] = plantId;

        if (stageId != oldListing.Stage.Id)
            patchData["stage_id"] = stageId;


        if (count != oldListing.Count)
            patchData["count"] = count;

        if (patchData.Count == 0)
        {
            return new PostPatchUserplantApiResponse(200, "Nem történt változás, frissítés kihagyva.");
        }

        //TODO. remove debug
        Debug.WriteLine("content:");
        foreach (var kvp in patchData)
        {
            Debug.WriteLine($"{kvp.Key}: {kvp.Value}");
        }

        // Constructing the request
        var request = new HttpRequestMessage(HttpMethod.Patch, url);
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        request.Content = new StringContent(JsonSerializer.Serialize(patchData), Encoding.UTF8, "application/json");

        // Sending the request
        try
        {
            var response = await httpClient.SendAsync(request);
            var responseText = await response.Content.ReadAsStringAsync();
            Debug.WriteLine($"\n\nPATCH Userplant request sent to API.\nRaw response: {responseText}");

            return JsonSerializer.Deserialize<PostPatchUserplantApiResponse>(responseText, jsonOptions);
        }
        catch (Exception ex)
        {
            return new PostPatchUserplantApiResponse(500, ExceptionHelperUtil.GetFriendlyMessage(ex) ?? $"Váratlan hiba történt a hirdetés növény/státusz/darabszám módosítása során. ({ex.Message})");
        }
    }

    public async Task<PostPatchUserplantApiResponse> CreateUserplantAsync(int plantId, int stageId, int count)
    {
        var url = $"{AppSettings.ApiBaseUrl}profile/plants";

        // Checking whether the user is logged in
        var token = await userSessionService.GetAuthTokenAsync();
        if (string.IsNullOrEmpty(token))
        {
            return new PostPatchUserplantApiResponse(401, "Kérjük jelentkezz be újra.");
        }

        // Build request data
        var postData = new Dictionary<string, object>
        {
            ["plant_id"] = plantId,
            ["stage_id"] = stageId,
            ["count"] = count
        };
        //TODO. remove debug
        Debug.WriteLine("content:");
        foreach (var kvp in postData)
        {
            Debug.WriteLine($"{kvp.Key}: {kvp.Value}");
        }

        // Constructing the request
        var request = new HttpRequestMessage(HttpMethod.Post, url);
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        request.Content = new StringContent(JsonSerializer.Serialize(postData), Encoding.UTF8, "application/json");

        // Sending the request
        try
        {
            var response = await httpClient.SendAsync(request);
            var responseText = await response.Content.ReadAsStringAsync();
            Debug.WriteLine($"\n\nPOST (Create) Userplant request sent to API.\nRaw response: {responseText}");

            return JsonSerializer.Deserialize<PostPatchUserplantApiResponse>(responseText, jsonOptions);
        }
        catch (Exception ex)
        {
            return new PostPatchUserplantApiResponse(500, ExceptionHelperUtil.GetFriendlyMessage(ex) ?? $"Váratlan hiba történt a hirdetés növény/státusz/darabszám hozzádása során. ({ex.Message})");
        }
    }


}