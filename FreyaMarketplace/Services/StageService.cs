namespace FreyaMarketplace.Services;

public class StageService
{
    private readonly HttpClient httpClient;
    private readonly JsonSerializerOptions jsonOptions;
    private readonly ExceptionHandlerUtil exceptionHandlerUtil;
    public StageService(HttpClient httpClient, JsonSerializerOptions jsonSerializerOptions, ExceptionHandlerUtil exceptionHandlerUtil)
    {
        this.httpClient = httpClient;
        this.jsonOptions = jsonSerializerOptions;
        this.exceptionHandlerUtil = exceptionHandlerUtil;
    }

    List<Stage> stages;

    public async Task<List<Stage>> GetStages()
    {
        var url = $"{AppSettings.ApiBaseUrl}stages";
        try
        {
            var response = await httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                var responseText = await response.Content.ReadAsStringAsync();
                Debug.WriteLine($"\n\nGET Stages request sent to API.\nRaw response: {responseText}");
                var stagesApiResponse = JsonSerializer.Deserialize<StagesApiResponse>(responseText, jsonOptions);
                Debug.WriteLine($"Deserialized response: \n\tcontent:{JsonSerializer.Serialize(stagesApiResponse)}");

                stages = stagesApiResponse.Data;
            }

            else
            {
                await exceptionHandlerUtil.HandleExceptionAsync(new Exception($"GET Stages request sent to API.\nResponse status: {response.StatusCode}"), "Nem sikerült lekérni a növények növekedési fázisait, mert az API nem 200 (OK) választ adott vissza.");
            }

        }
        catch (Exception ex)
        {
            await exceptionHandlerUtil.HandleExceptionAsync(ex, "Váratlan hiba történt a növények növekedési fázisainak lekérése közben.");
        }

        return stages;

    }

}