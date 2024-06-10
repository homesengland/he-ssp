namespace HE.Investments.Common.Models.App;

public class CrmApiAuthConfig : ICrmApiAuthConfig
{
    public string? TenantId { get; set; }

    public string? ClientId { get; set; }

    public string? ClientSecret { get; set; }

    public string? Scope { get; set; }
}
