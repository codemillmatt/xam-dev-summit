using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using PartlyNewsy.Core;
using Xamarin.Essentials;
using Microsoft.AppCenter.Crashes;
using Microsoft.AppCenter.Auth;

[assembly: Dependency(typeof(AppCenterAuthService))]
namespace PartlyNewsy.Core
{
    public class AppCenterAuthService : IAuthenticationService
    {
        public AppCenterAuthService()
        {
        }

        public bool IsLoggedIn()
        {
            if (!Preferences.ContainsKey(Constants.PreferencesAccountKey) ||
                string.IsNullOrWhiteSpace(Preferences.Get(Constants.PreferencesAccountKey, string.Empty)))
            {
                return false;
            }

            System.Diagnostics.Debug.WriteLine($"Logged in as {Preferences.Get(Constants.PreferencesAccountKey, string.Empty)}");

            return true;
        }

        public async Task<bool> Login()
        {
            try
            {
                var userInfo = await Auth.SignInAsync();

                await SecureStorage.SetAsync(Constants.PreferencesAccountAccessToken,
                    userInfo.AccessToken);

                System.Diagnostics.Debug.WriteLine($"Access token: {userInfo.AccessToken}");

                Preferences.Set(Constants.PreferencesAccountIdToken,
                    userInfo.IdToken, string.Empty);

                Preferences.Set(Constants.PreferencesAccountKey,
                    userInfo.AccountId, string.Empty);                
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);

                SecureStorage.Remove(Constants.PreferencesAccountAccessToken);
                Preferences.Remove(Constants.PreferencesAccountIdToken);
                Preferences.Remove(Constants.PreferencesAccountKey);

                return false;
            }

            return true;
        }        
    }
}
