using HE.Investments.Common.Contract;

namespace HE.Investment.AHP.Domain.UserContext;

internal static class AhpCacheKeys
{
    public static string OrganisationConsortium(OrganisationId organisationId) => $"ahp-consortium-{organisationId}";
}
