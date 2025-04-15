using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreyaMarketplace.ViewModel;

public partial class ProfileViewModel : BaseViewModel
{
    private readonly ProfileService profileService;
    private readonly ExceptionHandlerUtil exceptionHandlerUtil;

    [ObservableProperty] private string profileUsername;
    [ObservableProperty] private string profileEmail;
    [ObservableProperty] private string profileCity;
    [ObservableProperty] private string profileBirthdate;
    [ObservableProperty] private string profileDescription;

    [ObservableProperty] private string title;
    [ObservableProperty][NotifyPropertyChangedFor(nameof(IsProfileUsernameErrorVisible))] private string profileUsernameError;
    [ObservableProperty][NotifyPropertyChangedFor(nameof(IsProfileEmailErrorVisible))] private string profileEmailError;
    [ObservableProperty][NotifyPropertyChangedFor(nameof(IsProfileCityErrorVisible))] private string profileCityError;
    [ObservableProperty][NotifyPropertyChangedFor(nameof(IsProfileBirthdateErrorVisible))] private string profileBirthdateError;

    [ObservableProperty] private User user;

    public bool IsProfileUsernameErrorVisible => !string.IsNullOrEmpty(ProfileUsernameError);
    public bool IsProfileEmailErrorVisible => !string.IsNullOrEmpty(ProfileEmailError);
    public bool IsProfileCityErrorVisible => !string.IsNullOrEmpty(ProfileCityError);
    public bool IsProfileBirthdateErrorVisible => !string.IsNullOrEmpty(ProfileBirthdateError);

    public ProfileViewModel(ProfileService profileService, ExceptionHandlerUtil exceptionHandlerUtil)
    {
        this.profileService = profileService;
        this.exceptionHandlerUtil = exceptionHandlerUtil;

        User savedUser = JsonSerializer.Deserialize<User>(Preferences.Get("current_user", null));
        ProfileUsername = savedUser.Username;
        ProfileEmail = savedUser.Email;
        ProfileCity = savedUser.City;
        ProfileBirthdate = savedUser.Birthdate;
        ProfileDescription = savedUser.Description;
    }

    [RelayCommand]
    private async Task SaveProfileAsync()
    {
        if (IsBusy) return;
        try
        {
            IsBusy = true;
            ProfileEmailError = null;
            ProfileCityError = null;
            ProfileUsernameError = null;
            ProfileBirthdateError = null;

            var result = await profileService.SaveProfileAsync(ProfileUsername, ProfileEmail, ProfileCity, ProfileBirthdate, ProfileDescription);
            if (result == null) return;
            if (result.Data is ProfileSuccessData successData)
            {
                User = successData.User;
                Debug.Write($"User: {JsonSerializer.Serialize(User)}\n");
                string newDataJson = JsonSerializer.Serialize(User);
                Preferences.Set("current_user", newDataJson);
            }
            else if (result.Data is EmptyProfileData)
            {
                await exceptionHandlerUtil.HandleExceptionAsync(new Exception(result.Message), "asdasd");
            }
            else if (result.Data is ProfileValidationErrorData errorData)
            {

                if (errorData.Errors.ContainsKey("email"))
                {
                    ProfileEmailError = string.Join("\n", errorData.Errors["email"]);
                    OnPropertyChanged(nameof(IsProfileEmailErrorVisible));
                }

                if (errorData.Errors.ContainsKey("city"))
                {
                    ProfileCityError = string.Join("\n", errorData.Errors["city"]);
                    OnPropertyChanged(nameof(IsProfileCityErrorVisible));

                }

                if (errorData.Errors.ContainsKey("birthdate"))
                {
                    ProfileBirthdateError = string.Join("\n", errorData.Errors["birthdate"]);
                    OnPropertyChanged(nameof(IsProfileBirthdateErrorVisible));

                }

                if (errorData.Errors.ContainsKey("username"))
                {
                    ProfileCityError = string.Join("\n", errorData.Errors["username"]);
                    OnPropertyChanged(nameof(IsProfileCityErrorVisible));

                }
            }
            else
            {
                await exceptionHandlerUtil.HandleExceptionAsync(new Exception(result.Message), "Hiba adódott mentés során.");
            }
        }
        catch (Exception ex)
        {
            await exceptionHandlerUtil.HandleExceptionAsync(ex, "Hiba adódott a mentés során.");
        }
        finally
        {
            IsBusy = false;
        }
    }
}
