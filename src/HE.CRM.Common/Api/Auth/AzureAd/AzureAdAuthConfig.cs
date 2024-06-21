namespace HE.CRM.Common.Api.Auth.AzureAd
{
    public class AzureAdAuthConfig
    {
        public string TenantId { get; set; }

        public string ClientId { get; set; }

        public string ClientSecret { get; set; }

        public string Scope { get; set; }
    }
}
