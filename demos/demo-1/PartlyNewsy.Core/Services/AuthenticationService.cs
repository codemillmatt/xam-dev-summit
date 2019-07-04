using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AppCenter.Crashes;
using Microsoft.Identity.Client;
using Newtonsoft.Json.Linq;
using PartlyNewsy.Core;
using Xamarin.Essentials;
using Xamarin.Forms;

[assembly: Dependency(typeof(AuthenticationService))]
namespace  PartlyNewsy.Core
{
    public class AuthenticationService : IAuthenticationService
    {
        // Azure AD B2C Coordinates
        public static string Tenant = "xamdevsummitb2c.onmicrosoft.com";
        public static string AzureADB2CHostname = "login.microsoftonline.com";
        public static string ClientID = "d6bdeb09-8039-4bed-8d3e-0d5999007bd6";
        public static string PolicySignUpSignIn = "B2C_1_signupin";       

        public static string[] Scopes = { "https://xamdevsummitb2c.onmicrosoft.com/partlynewsy/user_impersonation"};

        public static string AuthorityBase = $"https://{AzureADB2CHostname}/tfp/{Tenant}/";
        public static string Authority = $"{AuthorityBase}{PolicySignUpSignIn}";
        
        public static string IOSKeyChainGroup = "com.microsoft.PartlyNewsy";

        public IPublicClientApplication PCA = null;
        public object ParentActivityOrWindow { get; set; }

        public AuthenticationService()
        {
            PCA = PublicClientApplicationBuilder.Create(ClientID)
                .WithB2CAuthority(Authority)
                .WithIosKeychainSecurityGroup(IOSKeyChainGroup)
                .WithRedirectUri($"msal{ClientID}://auth")
                .Build();
        }

        async Task<bool> SignInAsync()
        {
            try
            {
                // acquire token silent
                await AcquireToken();
            }
            catch (MsalUiRequiredException)
            {
                // acquire token interactive
                await SignInInteractively();
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);

                return false;
            }

            return true;
        }

        async Task AcquireToken()
        {
            IEnumerable<IAccount> accounts = await PCA.GetAccountsAsync();
            
            AuthenticationResult ar = await PCA.AcquireTokenSilent(Scopes, GetAccountByPolicy(accounts, PolicySignUpSignIn))
                .WithB2CAuthority(Authority)
                .ExecuteAsync();

            await UpdateUserInfo(ar);
        }

        async Task SignInInteractively()
        {
            IEnumerable<IAccount> accounts = await PCA.GetAccountsAsync();

            AuthenticationResult ar = await PCA.AcquireTokenInteractive(Scopes)
                .WithAccount(GetAccountByPolicy(accounts, PolicySignUpSignIn))
                .WithParentActivityOrWindow(ParentActivityOrWindow)
                .ExecuteAsync();

            await UpdateUserInfo(ar);
        }

        IAccount GetAccountByPolicy(IEnumerable<IAccount> accounts, string policy)
        {
            foreach (var account in accounts)
            {
                string userIdentifier = account.HomeAccountId.ObjectId.Split('.')[0];

                if (userIdentifier.EndsWith(policy.ToLower(), StringComparison.OrdinalIgnoreCase))
                    return account;
            }

            return null;
        }

        async Task UpdateUserInfo(AuthenticationResult ar)
        {
            JObject user = ParseIdToken(ar.IdToken);

            var userId = user["oid"]?.ToString();
            Preferences.Set(Constants.PreferencesAccountKey, userId);
            await SecureStorage.SetAsync(Constants.PreferencesAccountAccessToken, ar.AccessToken);
            Preferences.Set(Constants.PreferencesAccountIdToken, ar.IdToken);
        }

        JObject ParseIdToken(string idToken)
        {
            // Get the piece with actual user info
            idToken = idToken.Split('.')[1];
            idToken = Base64UrlDecode(idToken);
            return JObject.Parse(idToken);
        }

        string Base64UrlDecode(string s)
        {
            s = s.Replace('-', '+').Replace('_', '/');
            s = s.PadRight(s.Length + (4 - s.Length % 4) % 4, '=');
            var byteArray = Convert.FromBase64String(s);
            var decoded = Encoding.UTF8.GetString(byteArray, 0, byteArray.Count());
            return decoded;
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
            return await SignInAsync();
        }

        public void SetParent(object parent)
        {
            ParentActivityOrWindow = parent;
        }
    }
}
