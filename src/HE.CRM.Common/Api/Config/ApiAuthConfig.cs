namespace HE.CRM.Common.Api.Config
{
    public class ApiAuthConfig : IApiAuthConfig
    {
        public string TenantId { get; set; }

        public string ClientId { get; set; }

        public string ClientSecret { get; set; }

        public string Scope { get; set; }
    }
}
