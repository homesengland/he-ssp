namespace HE.CRM.Common.Api.Config
{
    public interface IApiAuthConfig
    {
        string TenantId { get; }

        string ClientId { get; }

        string ClientSecret { get; }

        string Scope { get; }
    }
}
