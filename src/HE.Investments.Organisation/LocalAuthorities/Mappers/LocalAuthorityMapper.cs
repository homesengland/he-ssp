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
            result.Add(LocalAuthority.New(localAuthorityDto.code, localAuthorityDto.name));
        }

        return result;
    }

    public static LocalAuthority? MapToLocalAuthority(string? localAuthorityId, string? localAuthorityName)
    {
        if (localAuthorityId.IsNotProvided() || localAuthorityName.IsNotProvided())
        {
            return null;
        }

        return new LocalAuthority(LocalAuthorityId.From(localAuthorityId!), localAuthorityName!);
    }
}
