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

    public static LocalAuthority? MapToLocalAuthority(LocalAuthorityDto? localAuthorityDto)
    {
        if (localAuthorityDto is null)
        {
            return null;
        }

        return new LocalAuthority(LocalAuthorityId.From(localAuthorityDto.onsCode), localAuthorityDto.name);
    }

    public static LocalAuthorityDto? MapToLocalAuthorityDto(LocalAuthority? localAuthority)
    {
        if (localAuthority?.Id is null || localAuthority?.Name is null)
        {
            return null;
        }

        return new LocalAuthorityDto { onsCode = localAuthority.Id.ToString(), name = localAuthority.Name };
    }
}
