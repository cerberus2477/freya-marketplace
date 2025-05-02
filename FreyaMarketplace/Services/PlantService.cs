namespace FreyaMarketplace.Services;

public class PlantService
{
    private readonly HttpClient httpClient;
    private readonly JsonSerializerOptions jsonOptions;
    private readonly ExceptionHandlerUtil exceptionHandlerUtil;
    public PlantService(HttpClient httpClient, JsonSerializerOptions jsonSerializerOptions, ExceptionHandlerUtil exceptionHandlerUtil)
    {
        this.httpClient = httpClient;
        this.jsonOptions = jsonSerializerOptions;
        this.exceptionHandlerUtil = exceptionHandlerUtil;
    }

    List<Plant> plants;

    public async Task<List<Plant>> GetPlants()
    {
        var url = $"{AppSettings.ApiBaseUrl}plants";
        try
        {
            var response = await httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                var responseText = await response.Content.ReadAsStringAsync();
                Debug.WriteLine($"\n\nGET Plants request sent to API.\nRaw response: {responseText}");
                var plantsApiResponse = JsonSerializer.Deserialize<PlantsApiResponse>(responseText, jsonOptions);
                Debug.WriteLine($"Deserialized response: \n\tcontent:{JsonSerializer.Serialize(plantsApiResponse)}");

                plants = plantsApiResponse.Data;
            }

            else
            {
                await exceptionHandlerUtil.HandleExceptionAsync(new Exception($"GET Plants request sent to API.\nResponse status: {response.StatusCode}"), "Nem sikerült lekérni a növényeket, mert az API nem 200 (OK) választ adott vissza.");
            }
        }
        catch (Exception ex)
        {
            await exceptionHandlerUtil.HandleExceptionAsync(ex, "Váratlan hiba történt a növények lekérése közben.");
        }

        return plants;

    }

}