namespace HE.Investments.Common.Models.App;

public interface ICrmApiAuthConfig
{
    string? TenantId { get; }

    string? ClientId { get; }

    string? ClientSecret { get; }

    string? Scope { get; }
}
