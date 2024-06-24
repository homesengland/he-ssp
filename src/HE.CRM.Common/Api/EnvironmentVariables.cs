namespace HE.CRM.Common.Api
{
    internal static class EnvironmentVariables
    {
        public const string FrontDoorApiBaseUrl = "invln_frontdoorapibaseurl";

        public static class AzureAd
        {
            public const string TenantId = "invln_azureadclientid";

            public const string ClientId = "invln_azureadclientid";

            public const string ClientSecret = "invln_azureadclientsecret";

            public const string Scope = "invln_azureadscope";
        }
    }
}
