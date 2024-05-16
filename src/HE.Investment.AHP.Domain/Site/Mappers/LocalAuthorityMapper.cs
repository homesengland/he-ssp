extern alias Org;

using HE.Common.IntegrationModel.PortalIntegrationModel;
using Org::HE.Investments.Organisation.LocalAuthorities.ValueObjects;
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

    public static LocalAuthority? FromDto(SiteLocalAuthority? localAuthority)
    {
        return localAuthority is null || string.IsNullOrWhiteSpace(localAuthority.id) || string.IsNullOrWhiteSpace(localAuthority.name)
            ? null
            : new LocalAuthority(new LocalAuthorityCode(localAuthority.id), localAuthority.name);
    }
}
