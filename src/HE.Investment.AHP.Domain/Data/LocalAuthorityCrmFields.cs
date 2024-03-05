using HE.Investments.Common.CRM.Model;

namespace HE.Investment.AHP.Domain.Data;

public static class LocalAuthorityCrmFields
{
    public static readonly IReadOnlyList<string> Fields = new List<string>
    {
        nameof(invln_AHGLocalAuthorities.invln_LocalAuthorityName),
        nameof(invln_AHGLocalAuthorities.invln_GSSCode),
    };
}
