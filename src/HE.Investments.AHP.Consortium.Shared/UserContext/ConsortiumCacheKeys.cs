using HE.Investments.Common.Contract;

namespace HE.Investments.AHP.Consortium.Shared.UserContext;

public static class ConsortiumCacheKeys
{
    public static string OrganisationConsortium(OrganisationId organisationId) => $"ahp-consortium-{organisationId}";
}
