using System.Globalization;
using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.Investments.Common.Contract.Exceptions;
using HE.Investments.Common.CRM.Model;
using HE.Investments.Common.CRM.Serialization;
using HE.Investments.Common.Infrastructure.Cache.Interfaces;
using HE.Investments.Organisation.LocalAuthorities.Mappers;
using HE.Investments.Organisation.LocalAuthorities.ValueObjects;
using Microsoft.PowerPlatform.Dataverse.Client;

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
        var localAuthorities = await _cacheService.GetValueAsync(
                                   "local-authorities",
                                   async () => await GetLocalAuthorities(cancellationToken))
                               ?? throw new NotFoundException(nameof(LocalAuthority));

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

    public async Task<LocalAuthority> GetById(string localAuthorityId, CancellationToken cancellationToken)
    {
        var localAuthorities = await _cacheService.GetValueAsync(
                                   "local-authorities",
                                   async () => await GetLocalAuthorities(cancellationToken))
                               ?? throw new NotFoundException(nameof(LocalAuthority));

        var localAuthority = localAuthorities.FirstOrDefault(x => x.Id.ToString() == localAuthorityId) ??
                             throw new NotFoundException($"Local authority with id {localAuthorityId} cannot be found");

        return localAuthority;
    }

    private async Task<IList<LocalAuthority>> GetLocalAuthorities(CancellationToken cancellationToken)
    {
        var req = new invln_searchlocalauthorityRequest();
        var response = await _serviceClient.ExecuteAsync(req, cancellationToken) as invln_searchlocalauthorityResponse
                       ?? throw new NotFoundException(nameof(LocalAuthority));

        var localAuthoritiesDto = CrmResponseSerializer.Deserialize<IList<LocalAuthorityDto>>(response.invln_localauthorities)
                                  ?? throw new NotFoundException(nameof(LocalAuthority));

        return LocalAuthorityMapper.MapToLocalAuthorityList(localAuthoritiesDto);
    }
}
