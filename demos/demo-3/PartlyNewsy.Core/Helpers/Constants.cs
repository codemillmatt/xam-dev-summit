using System;

namespace PartlyNewsy.Core
{
    public static class Constants
    {
        public static readonly string PreferencesAccountKey = "account-id";
        public static readonly string PreferencesAccountAccessToken = "account-access-token";
        public static readonly string PreferencesAccountIdToken = "account-id-token";

        public static readonly string CosmosEndpoint = "";
        //public static readonly string PermissionsUrlBase = "https://xam-dev-summit-permission-scus.azurewebsites.net";
        //public static readonly string PermissionsFunctionUrl = "https://xam-dev-summit-permission-scus.azurewebsites.net/api/permissions/UserInfoPermission";

        public static readonly string PermissionsUrlBase = "https://xam-dev-summit.azurefd.net";
        public static readonly string PermissionsFunctionUrl = "https://xam-dev-summit.azurefd.net/api/permissions/UserInfoPermission";


        //public static readonly string NewsFunctionsUrl = "https://xam-dev-summit-function-scus.azurewebsites.net/api/news";
        public static readonly string NewsFunctionsUrl = "https://xam-dev-summit.azurefd.net/api/news";
    }
}
