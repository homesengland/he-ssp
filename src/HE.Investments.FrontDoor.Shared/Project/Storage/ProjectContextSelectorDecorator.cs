using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.Investments.Common;
using HE.Investments.FrontDoor.Shared.Project.Storage.Api;
using HE.Investments.FrontDoor.Shared.Project.Storage.Crm;
using Microsoft.FeatureManagement;

namespace HE.Investments.FrontDoor.Shared.Project.Storage;

internal sealed class ProjectContextSelectorDecorator : IProjectContext
{
    private readonly IFeatureManager _featureManager;

    private readonly Func<ProjectCrmContext> _crmContextFactory;

    private readonly Func<ProjectApiContext> _apiContextFactory;

    public ProjectContextSelectorDecorator(
        IFeatureManager featureManager,
        Func<ProjectCrmContext> crmContextFactory,
        Func<ProjectApiContext> apiContextFactory)
    {
        _featureManager = featureManager;
        _crmContextFactory = crmContextFactory;
        _apiContextFactory = apiContextFactory;
    }

    public async Task<FrontDoorProjectDto> GetOrganisationProjectById(string projectId, string organisationId, CancellationToken cancellationToken)
    {
        var context = await GetProjectContext();
        return await context.GetOrganisationProjectById(projectId, organisationId, cancellationToken);
    }

    public async Task<FrontDoorProjectDto> GetUserProjectById(string projectId, string userGlobalId, string organisationId, CancellationToken cancellationToken)
    {
        var context = await GetProjectContext();
        return await context.GetUserProjectById(projectId, userGlobalId, organisationId, cancellationToken);
    }

    public async Task<FrontDoorProjectSiteDto> GetProjectSite(string projectId, string siteId, CancellationToken cancellationToken)
    {
        var context = await GetProjectContext();
        return await context.GetProjectSite(projectId, siteId, cancellationToken);
    }

    public async Task<PagedResponseDto<FrontDoorProjectSiteDto>> GetProjectSites(string projectId, CancellationToken cancellationToken)
    {
        var context = await GetProjectContext();
        return await context.GetProjectSites(projectId, cancellationToken);
    }

    public async Task DeactivateProject(string projectId, CancellationToken cancellationToken)
    {
        var context = await GetProjectContext();
        await context.DeactivateProject(projectId, cancellationToken);
    }

    private async Task<IProjectContext> GetProjectContext()
    {
        if (await _featureManager.IsEnabledAsync(FeatureFlags.UseFrontDoorApi))
        {
            return _apiContextFactory();
        }

        return _crmContextFactory();
    }
}
