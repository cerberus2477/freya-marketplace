using Microsoft.Maui.Controls;

namespace FreyaMarketplace.View.StartingPages
{
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();
        }

        private async void Login_Clicked(object sender, EventArgs e)
        {
            string userName = txtUserName.Text;
            string password = txtPassword.Text;

            if (string.IsNullOrWhiteSpace(userName) || string.IsNullOrWhiteSpace(password))
            {
                await DisplayAlert("Error", "Missing username or password", "OK");
                return;
            }

            // Authentication logic here (to be replaced)
            bool loginSuccess = true;

            if (loginSuccess)
            {
                Preferences.Set("IsLoggedIn", true);
                //if (Shell.Current is AppShell shell)
                //{
                //    shell.NavigateToHomePage();
                //}
            }
            else
            {
                await DisplayAlert("Error", "Login failed. Please try again.", "OK");
            }
        }

        private void OnRegisterClicked(object sender, EventArgs e)
        {
            //if (Shell.Current is AppShell shell)
            //{
            //    shell.NavigateToRegisterPage(); 
            //}
        }

        private async void ForgotPassword_Tapped(object sender, EventArgs e)
        {
            await DisplayAlert("Forgot Password - not implemened", "Password reset functionality not implemented yet.", "OK");
        }
    }
}

    

        //chatgpt mondta, idk, levi a login rad lesz bizva xd



        //var client = new HttpClient();
        //var url = ApiBaseUrl + "login";  // Your login route

        //var loginData = new
        //{
        //    email = userName,
        //    password = password
        //};

        //var jsonContent = JsonConvert.SerializeObject(loginData);
        //var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

        //try
        //{
        //    var response = await client.PostAsync(url, content);
        //    if (response.IsSuccessStatusCode)
        //    {
        //        var result = await response.Content.ReadAsStringAsync();
        //        // You can handle the success response here
        //        // For example, store the token
        //        await DisplayAlert("Success", "Logged in successfully!", "OK");
        //        // Save the token securely (using SecureStorage in Maui)
        //        await SecureStorage.SetAsync("user_token", result.token);  // assuming the token is in the response body
        //    }
        //    else
        //    {
        //        // Handle error response here (invalid credentials, etc.)
        //        var errorResponse = await response.Content.ReadAsStringAsync();
        //        await DisplayAlert("Error", "Login failed: " + errorResponse, "OK");
        //    }
        //}
        //catch (Exception ex)
        //{
        //    await DisplayAlert("Error", "An error occurred: " + ex.Message, "OK");
        //}
        //}



