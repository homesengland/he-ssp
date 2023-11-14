extern alias Org;

using HE.InvestmentLoans.Contract.Projects.ValueObjects;
using LocalAuthorityDto = HE.Common.IntegrationModel.PortalIntegrationModel.LocalAuthorityDto;

namespace HE.InvestmentLoans.BusinessLogic.Projects.Repositories.Mappers;

public static class LocalAuthorityMapper
{
    public static IList<LocalAuthority> MapToLocalAuthorityList(IList<LocalAuthorityDto> localAuthoritiesDto)
    {
        var result = new List<LocalAuthority>();

        foreach (var localAuthorityDto in localAuthoritiesDto)
        {
            result.Add(LocalAuthority.New(localAuthorityDto.onsCode, localAuthorityDto.name));
        }

        return result;
    }
}
