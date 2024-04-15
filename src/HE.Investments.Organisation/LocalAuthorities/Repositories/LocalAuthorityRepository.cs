using System.Globalization;
using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.Investments.Common.Contract.Exceptions;
using HE.Investments.Common.CRM.Model;
using HE.Investments.Common.CRM.Serialization;
using HE.Investments.Common.Infrastructure.Cache.Interfaces;
using HE.Investments.Organisation.LocalAuthorities.Mappers;
using HE.Investments.Organisation.LocalAuthorities.ValueObjects;
using Microsoft.PowerPlatform.Dataverse.Client;
using LocalAuthority = HE.Investments.Organisation.LocalAuthorities.ValueObjects.LocalAuthority;

namespace HE.Investments.Organisation.LocalAuthorities.Repositories;

public class LocalAuthorityRepository : ILocalAuthorityRepository
{
    private readonly IOrganizationServiceAsync2 _serviceClient;

    private readonly ICacheService _cacheService;

    private readonly LocalAuthoritySource _source;

    public LocalAuthorityRepository(IOrganizationServiceAsync2 serviceClient, ICacheService cacheService, LocalAuthoritySource source)
    {
        _serviceClient = serviceClient;
        _cacheService = cacheService;
        _source = source;
    }

    public async Task<(IList<LocalAuthority> Items, int TotalItems)> Search(string phrase, int startPage, int pageSize, CancellationToken cancellationToken)
    {
        var localAuthorities = await GetLocalAuthorities(cancellationToken);

        if (!string.IsNullOrEmpty(phrase))
        {
            localAuthorities = localAuthorities
                .Where(c => c.Name.ToLower(CultureInfo.InvariantCulture).Contains(phrase.ToLower(CultureInfo.InvariantCulture)))
                .ToList();
        }

        var totalItems = localAuthorities.Count;

        var itemsAtPage = localAuthorities
            .OrderBy(c => c.Name)
            .Skip(startPage * pageSize)
            .Take(pageSize);

        return (itemsAtPage.ToList(), totalItems);
    }

    public async Task<LocalAuthority> GetByCode(LocalAuthorityCode localAuthorityCode, CancellationToken cancellationToken)
    {
        var localAuthorities = await GetLocalAuthorities(cancellationToken);

        var localAuthority = localAuthorities.FirstOrDefault(x => x.Code.ToString() == localAuthorityCode.ToString()) ??
                             throw new NotFoundException($"Local authority with code {localAuthorityCode} cannot be found");

        return localAuthority;
    }

    private async Task<IList<LocalAuthority>> GetLocalAuthorities(CancellationToken cancellationToken)
    {
        return await _cacheService.GetValueAsync(
                   $"local-authorities-{_source.ToString().ToLowerInvariant()}",
                   async () => await GetLocalAuthoritiesFromCrm(string.Empty, cancellationToken))
               ?? new List<LocalAuthority>();
    }

    private async Task<IList<LocalAuthority>?> GetLocalAuthoritiesFromCrm(string searchPage, CancellationToken cancellationToken)
    {
        var req = new invln_getmultiplelocalauthoritiesformoduleRequest
        {
            invln_searchphrase = searchPage,
            invln_isahp = (_source == LocalAuthoritySource.Ahp).ToString().ToLowerInvariant(),
            invln_isloanfd = (_source == LocalAuthoritySource.FrontDoor).ToString().ToLowerInvariant(),
            invln_isloan = (_source == LocalAuthoritySource.Loans).ToString().ToLowerInvariant(),
            invln_pagingrequest = CrmResponseSerializer.Serialize(new PagingRequestDto { pageNumber = 1, pageSize = 1000 }),
            invln_usehetables = "true",
        };

        var response = await _serviceClient.ExecuteAsync(req, cancellationToken) as invln_getmultiplelocalauthoritiesformoduleResponse
                       ?? throw new NotFoundException(nameof(LocalAuthority));

        var localAuthoritiesDto = CrmResponseSerializer.Deserialize<PagedResponseDto<LocalAuthorityDto>>(response.invln_localauthorities)
                                  ?? throw new NotFoundException(nameof(LocalAuthority));

        var result = LocalAuthorityMapper.MapToLocalAuthorityList(localAuthoritiesDto.items);
        return result.Any() ? result : null;
    }
}
