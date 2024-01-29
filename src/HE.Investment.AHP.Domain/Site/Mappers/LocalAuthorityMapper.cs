extern alias Org;

using ContractLocalAuthority = HE.Investment.AHP.Contract.Site.LocalAuthority;
using OrganisationLocalAuthority = Org::HE.Investments.Organisation.LocalAuthorities.ValueObjects.LocalAuthority;

namespace HE.Investment.AHP.Domain.Site.Mappers;

public static class LocalAuthorityMapper
{
    public static ContractLocalAuthority? Map(OrganisationLocalAuthority? organisationLocalAuthority)
    {
        return organisationLocalAuthority != null ? new ContractLocalAuthority
        {
            Name = organisationLocalAuthority.Name,
            Id = organisationLocalAuthority.Id.ToString(),
        }
        : null;
    }
}
