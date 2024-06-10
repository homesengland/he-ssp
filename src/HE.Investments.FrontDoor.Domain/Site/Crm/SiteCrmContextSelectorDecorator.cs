using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.Investments.Account.Shared.User;
using HE.Investments.Common;
using Microsoft.FeatureManagement;

namespace HE.Investments.FrontDoor.Domain.Site.Crm;

internal sealed class SiteCrmContextSelectorDecorator : ISiteCrmContext
{
    private readonly IFeatureManager _featureManager;

    private readonly Func<SiteCrmContext> _crmContextFactory;

    private readonly Func<SiteCrmApiHttpClient> _httpClientFactory;

    public SiteCrmContextSelectorDecorator(
        IFeatureManager featureManager,
        Func<SiteCrmContext> crmContextFactory,
        Func<SiteCrmApiHttpClient> httpClientFactory)
    {
        _featureManager = featureManager;
        _httpClientFactory = httpClientFactory;
        _crmContextFactory = crmContextFactory;
    }

    public async Task<PagedResponseDto<FrontDoorProjectSiteDto>> GetSites(
        string projectId,
        UserAccount userAccount,
        PagingRequestDto pagination,
        CancellationToken cancellationToken)
    {
        var context = await GetCrmContext();
        return await context.GetSites(projectId, userAccount, pagination, cancellationToken);
    }

    public async Task<FrontDoorProjectSiteDto> GetSite(string projectId, string siteId, UserAccount userAccount, CancellationToken cancellationToken)
    {
        var context = await GetCrmContext();
        return await context.GetSite(projectId, siteId, userAccount, cancellationToken);
    }

    public async Task<string> Save(string projectId, FrontDoorProjectSiteDto dto, string userGlobalId, string organisationId, CancellationToken cancellationToken)
    {
        var context = await GetCrmContext();
        return await context.Save(projectId, dto, userGlobalId, organisationId, cancellationToken);
    }

    public async Task<string> Remove(string siteId, UserAccount userAccount, CancellationToken cancellationToken)
    {
        var context = await GetCrmContext();
        return await context.Remove(siteId, userAccount, cancellationToken);
    }

    private async Task<ISiteCrmContext> GetCrmContext()
    {
        if (await _featureManager.IsEnabledAsync(FeatureFlags.UseFrontDoorApi))
        {
            return _httpClientFactory();
        }

        return _crmContextFactory();
    }
}
