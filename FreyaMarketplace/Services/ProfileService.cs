using Microsoft.Maui.Controls.PlatformConfiguration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace FreyaMarketplace.Services
{
    public class ProfileService
    {
        private readonly HttpClient httpClient;
        private readonly UserSessionService userSessionService;
        private readonly JsonSerializerOptions jsonOptions;

        public ProfileService(HttpClient httpClient, JsonSerializerOptions jsonSerializerOptions, UserSessionService userSessionService)
        {
            this.httpClient = httpClient;
            this.jsonOptions = jsonSerializerOptions;
            this.userSessionService = userSessionService;
        }

        public async Task<ProfileApiResponse> UpdateProfileAsync(string username, string userEmail, string userCity, string userBirthdate, string userDescription)
        {
            var url = $"{AppSettings.ApiBaseUrl}profile";

            // Checking whether the user is logged in
            var token = await userSessionService.GetAuthTokenAsync();
            if (string.IsNullOrEmpty(token))
            {
                return new ProfileApiResponse(401, "Kérjük jelentkezz be újra.");
            }

            // Constructing the request (only the fields that are different from the old ones are sent)
            User oldUser = JsonSerializer.Deserialize<User>(Preferences.Get("current_user", null));
            var patchData = new Dictionary<string, string>();

            if (username != oldUser.Username)
            {
                patchData["username"] = username;
            }

            if (userEmail != oldUser.Email)
            {
                patchData["email"] = userEmail;
            }

            if (userCity != oldUser.City)
            {
                patchData["city"] = userCity;
            }

            if (userBirthdate != oldUser.Birthdate)
            {
                patchData["birthdate"] = userBirthdate;
            }

            if (userDescription != oldUser.Description)
            {
                patchData["description"] = userDescription;
            }

            // Skip request if nothing changed
            if (patchData.Count == 0)
            {
                return new ProfileApiResponse(200, "Nem történt változás, frissítés kihagyva.");
            }

            var content = new StringContent(JsonSerializer.Serialize(patchData), Encoding.UTF8, "application/json");
            var request = new HttpRequestMessage(HttpMethod.Patch, url);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            request.Content = content;

            // Sending the request
            try
            {
                var response = await httpClient.SendAsync(request);
                var responseText = await response.Content.ReadAsStringAsync();
                Debug.WriteLine($"\n\nProfile patch request sent to API.\nRaw response: {responseText}");

                return JsonSerializer.Deserialize<ProfileApiResponse>(responseText, jsonOptions);
            }
            catch (Exception ex)
            {
                return new ProfileApiResponse(500, $"Váratlan hiba történt a mentés során. ({ex.Message})");
            }
        }
    }
}
