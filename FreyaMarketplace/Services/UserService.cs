using System.Buffers.Text;
using System.Net.Http.Json;
using System.Text;
using static FreyaMarketplace.Services.UserService;

namespace FreyaMarketplace.Services;

public interface IAuthService
{
    Task<ApiResponse<LoginData>> Login(string email, string password);
}

public class UserService : IAuthService
{
    HttpClient httpClient;
    public UserService()
    {
        this.httpClient = new HttpClient();
        httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
        httpClient.DefaultRequestHeaders.Add("Accept-Encoding", "gzip, deflate, br");
        httpClient.DefaultRequestHeaders.Add("Connection", "keep-alive");
    }


        public async Task<ApiResponse<LoginData>> Login(string email, string password)
    {
        var url = $"{AppSettings.ApiBaseUrl}login";

        try
        {
            var request = new LoginRequest
            {
                Email = email,
                Password = password
            };

            var json = JsonSerializer.Serialize(request);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await httpClient.PostAsync(url, content);

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<ApiResponse<LoginData>>(responseContent);
                return result;
            }

            return new ApiResponse<LoginData>
            {
                Status = (int)response.StatusCode,
                Message = $"Login failed: {response.RequestMessage}"
            };
        }
        catch (Exception ex)
        {
            return new ApiResponse<LoginData>
            {
                Status = 500,
                Message = $"An error occurred: {ex.Message}"
            };
        }
    }

    public class ApiResponse<T>
    {
        public int Status { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }
    }

    public class LoginData
    {
        public User User { get; set; }
        public string Token { get; set; }
    }

    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string City { get; set; }
        public string Birthdate { get; set; }
        public int RoleId { get; set; }
        public string Picture { get; set; }
        public string Description { get; set; }
    }

    public class LoginRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }




}