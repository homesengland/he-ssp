using System.Globalization;
using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.Investments.Common.Contract;
using HE.Investments.Common.Contract.Exceptions;
using HE.Investments.Common.CRM.Model;
using HE.Investments.Common.CRM.Serialization;
using HE.Investments.Common.Infrastructure.Cache.Interfaces;
using HE.Investments.Organisation.LocalAuthorities.Mappers;
using Microsoft.PowerPlatform.Dataverse.Client;
using Microsoft.Xrm.Sdk;
using LocalAuthority = HE.Investments.Organisation.LocalAuthorities.ValueObjects.LocalAuthority;

namespace HE.Investments.Organisation.LocalAuthorities.Repositories;

public class LocalAuthorityRepository : ILocalAuthorityRepository
{
    private readonly IOrganizationServiceAsync2 _serviceClient;

    private readonly ICacheService _cacheService;

    public LocalAuthorityRepository(IOrganizationServiceAsync2 serviceClient, ICacheService cacheService)
    {
        _serviceClient = serviceClient;
        _cacheService = cacheService;
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
        return await _cacheService.GetValueAsync(
                   $"local-authorities-{source}",
                   async () => await GetLocalAuthoritiesFromCrm(string.Empty, source, cancellationToken))
               ?? throw new NotFoundException(nameof(LocalAuthority));
    }

    private async Task<IList<LocalAuthority>> GetLocalAuthoritiesFromCrm(string searchPage, LocalAuthoritiesSource source, CancellationToken cancellationToken)
    {
        var req = new invln_getmultiplelocalauthoritiesformoduleRequest
        {
            invln_searchphrase = searchPage,
            invln_isahp = (source == LocalAuthoritiesSource.Ahp).ToString().ToLowerInvariant(),
            invln_isloanfd = (source == LocalAuthoritiesSource.FrontDoor).ToString().ToLowerInvariant(),
            invln_isloan = (source == LocalAuthoritiesSource.Loans).ToString().ToLowerInvariant(),
            invln_pagingrequest = CrmResponseSerializer.Serialize(new PagingRequestDto { pageNumber = 1, pageSize = 1000 }),
        };

        var response = await _serviceClient.ExecuteAsync(req, cancellationToken) as invln_getmultiplelocalauthoritiesformoduleResponse
                       ?? throw new NotFoundException(nameof(LocalAuthority));

        var localAuthoritiesDto = CrmResponseSerializer.Deserialize<PagedResponseDto<LocalAuthorityDto>>(response.invln_localauthorities)
                                  ?? throw new NotFoundException(nameof(LocalAuthority));

        return LocalAuthorityMapper.MapToLocalAuthorityList(localAuthoritiesDto.items);
    }
}
