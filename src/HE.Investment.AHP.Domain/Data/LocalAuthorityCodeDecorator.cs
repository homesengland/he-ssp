using HE.Common.IntegrationModel.PortalIntegrationModel;

namespace HE.Investment.AHP.Domain.Data;

// TODO: remove this class when CRM will use HE local authority codes for AHP sites
public class LocalAuthorityCodeDecorator : ISiteCrmContext
{
    private const string AhpLocalAuthorityPrefix = "E0";

    private readonly ISiteCrmContext _decorated;

    public LocalAuthorityCodeDecorator(ISiteCrmContext decorated)
    {
        _decorated = decorated;
    }

    public async Task<PagedResponseDto<SiteDto>> GetOrganisationSites(Guid organisationId, PagingRequestDto pagination, CancellationToken cancellationToken)
    {
        return UnwrapCode(await _decorated.GetOrganisationSites(organisationId, pagination, cancellationToken));
    }

    public async Task<PagedResponseDto<SiteDto>> GetUserSites(string userGlobalId, PagingRequestDto pagination, CancellationToken cancellationToken)
    {
        return UnwrapCode(await _decorated.GetUserSites(userGlobalId, pagination, cancellationToken));
    }

    public async Task<SiteDto?> GetById(string siteId, CancellationToken cancellationToken)
    {
        return UnwrapCode(await _decorated.GetById(siteId, cancellationToken));
    }

    public async Task<bool> Exist(string name, CancellationToken cancellationToken)
    {
        return await _decorated.Exist(name, cancellationToken);
    }

    public async Task<string> Save(Guid organisationId, string userGlobalId, SiteDto dto, CancellationToken cancellationToken)
    {
        if (dto.localAuthority != null && string.IsNullOrWhiteSpace(dto.localAuthority.id))
        {
            dto.localAuthority.id = $"E0{dto.localAuthority.id}";
        }

        return await _decorated.Save(organisationId, userGlobalId, WrapCode(dto), cancellationToken);
    }

    private static PagedResponseDto<SiteDto> UnwrapCode(PagedResponseDto<SiteDto> dto)
    {
        foreach (var item in dto.items)
        {
            UnwrapCode(item);
        }

        return dto;
    }

    private static SiteDto? UnwrapCode(SiteDto? dto)
    {
        if (!string.IsNullOrWhiteSpace(dto?.localAuthority?.id))
        {
            dto.localAuthority.id = dto.localAuthority.id.Replace(AhpLocalAuthorityPrefix, string.Empty);
        }

        return dto;
    }

    private static SiteDto WrapCode(SiteDto dto)
    {
        if (!string.IsNullOrWhiteSpace(dto.localAuthority?.id))
        {
            dto.localAuthority.id = $"{AhpLocalAuthorityPrefix}{dto.localAuthority.id}";
        }

        return dto;
    }
}
