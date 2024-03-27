using System.Globalization;
using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.Investments.Common.Contract;
using HE.Investments.Common.Contract.Exceptions;
using HE.Investments.Common.CRM.Extensions;
using HE.Investments.Common.CRM.Model;
using HE.Investments.Common.CRM.Serialization;
using HE.Investments.Common.Infrastructure.Cache.Interfaces;
using HE.Investments.Organisation.LocalAuthorities.Mappers;
using Microsoft.FeatureManagement;
using Microsoft.PowerPlatform.Dataverse.Client;
using LocalAuthority = HE.Investments.Organisation.LocalAuthorities.ValueObjects.LocalAuthority;

namespace HE.Investments.Organisation.LocalAuthorities.Repositories;

public class LocalAuthorityRepository : ILocalAuthorityRepository
{
    private readonly IOrganizationServiceAsync2 _serviceClient;

    private readonly ICacheService _cacheService;

    private readonly IFeatureManager _featureManager;

    public LocalAuthorityRepository(IOrganizationServiceAsync2 serviceClient, ICacheService cacheService, IFeatureManager featureManager)
    {
        _serviceClient = serviceClient;
        _cacheService = cacheService;
        _featureManager = featureManager;
    }

    public async Task<(IList<LocalAuthority> Items, int TotalItems)> Search(string phrase, int startPage, int pageSize, CancellationToken cancellationToken)
    {
        var localAuthorities = await GetLocalAuthorities(LocalAuthoritiesSource.Loans, cancellationToken);

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

    public async Task<LocalAuthority> GetById(StringIdValueObject localAuthorityId, CancellationToken cancellationToken)
    {
        var localAuthorities = await GetLocalAuthorities(LocalAuthoritiesSource.Loans, cancellationToken);

        var localAuthority = localAuthorities.FirstOrDefault(x => x.Id.ToString() == localAuthorityId.ToString()) ??
                             throw new NotFoundException($"Local authority with id {localAuthorityId} cannot be found");

        return localAuthority;
    }

    private async Task<IList<LocalAuthority>> GetLocalAuthorities(LocalAuthoritiesSource source, CancellationToken cancellationToken)
    {
        var useHeTables = await _featureManager.GetUseHeTablesParameter();
        return await _cacheService.GetValueAsync(
                   $"local-authorities-{source.ToString().ToLowerInvariant()}-{useHeTables}",
                   async () => await GetLocalAuthoritiesFromCrm(string.Empty, source, useHeTables, cancellationToken))
               ?? new List<LocalAuthority>();
    }

    private async Task<IList<LocalAuthority>?> GetLocalAuthoritiesFromCrm(string searchPage, LocalAuthoritiesSource source, string useHeTables, CancellationToken cancellationToken)
    {
        var req = new invln_getmultiplelocalauthoritiesformoduleRequest
        {
            invln_searchphrase = searchPage,
            invln_isahp = (source == LocalAuthoritiesSource.Ahp).ToString().ToLowerInvariant(),
            invln_isloanfd = (source == LocalAuthoritiesSource.FrontDoor).ToString().ToLowerInvariant(),
            invln_isloan = (source == LocalAuthoritiesSource.Loans).ToString().ToLowerInvariant(),
            invln_pagingrequest = CrmResponseSerializer.Serialize(new PagingRequestDto { pageNumber = 1, pageSize = 1000 }),
            invln_usehetables = useHeTables,
        };

        var response = await _serviceClient.ExecuteAsync(req, cancellationToken) as invln_getmultiplelocalauthoritiesformoduleResponse
                       ?? throw new NotFoundException(nameof(LocalAuthority));

        var localAuthoritiesDto = CrmResponseSerializer.Deserialize<PagedResponseDto<LocalAuthorityDto>>(response.invln_localauthorities)
                                  ?? throw new NotFoundException(nameof(LocalAuthority));

        var result = LocalAuthorityMapper.MapToLocalAuthorityList(localAuthoritiesDto.items);
        return result.Any() ? result : null;
    }
}
