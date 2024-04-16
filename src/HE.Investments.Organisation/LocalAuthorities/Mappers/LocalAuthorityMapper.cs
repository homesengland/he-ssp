using HE.Investments.Common.Extensions;
using HE.Investments.Organisation.LocalAuthorities.ValueObjects;
using LocalAuthorityDto = HE.Common.IntegrationModel.PortalIntegrationModel.LocalAuthorityDto;

namespace HE.Investments.Organisation.LocalAuthorities.Mappers;

public static class LocalAuthorityMapper
{
    public static IList<LocalAuthority> MapToLocalAuthorityList(IList<LocalAuthorityDto> localAuthoritiesDto)
    {
        var result = new List<LocalAuthority>();

        foreach (var localAuthorityDto in localAuthoritiesDto)
        {
            result.Add(LocalAuthority.New(string.IsNullOrWhiteSpace(localAuthorityDto.code) ? localAuthorityDto.onsCode : localAuthorityDto.code, localAuthorityDto.name));
        }

        return result;
    }

    public static LocalAuthority? MapToLocalAuthority(string? localAuthorityCode, string? localAuthorityName)
    {
        if (localAuthorityCode.IsNotProvided() || localAuthorityName.IsNotProvided())
        {
            return null;
        }

        return new LocalAuthority(LocalAuthorityCode.From(localAuthorityCode!), localAuthorityName!);
    }
}
