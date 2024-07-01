namespace HE.CRM.Common.Api
{
    internal static class EnvironmentVariables
    {
        public const string FrontDoorApiBaseUrl = "AzureAd.FrontDoorApiBaseUrl";

        public static class AzureAd
        {
            public const string TenantId = "AzureAd.TenantId";

            public const string ClientId = "AzureAd.ClientId";

            public const string ClientSecret = "AzureAd.ClientSecret";

            public const string Scope = "AzureAd.Scope";
        }
    }
}
