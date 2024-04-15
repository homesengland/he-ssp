extern alias Org;

using ContractLocalAuthority = HE.Investments.Common.Contract.LocalAuthority;
using OrganisationLocalAuthority = Org::HE.Investments.Organisation.LocalAuthorities.ValueObjects.LocalAuthority;

namespace HE.Investment.AHP.Domain.Site.Mappers;

public static class LocalAuthorityMapper
{
    public static ContractLocalAuthority? Map(OrganisationLocalAuthority? organisationLocalAuthority)
    {
        return organisationLocalAuthority != null
            ? new ContractLocalAuthority(organisationLocalAuthority.Code.Value, organisationLocalAuthority.Name)
            : null;
    }
}
