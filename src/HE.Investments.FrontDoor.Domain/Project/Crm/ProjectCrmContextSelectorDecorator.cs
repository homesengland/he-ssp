using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.Investments.Common;
using Microsoft.FeatureManagement;

namespace HE.Investments.FrontDoor.Domain.Project.Crm;

internal sealed class ProjectCrmContextSelectorDecorator : IProjectCrmContext
{
    private readonly IFeatureManager _featureManager;

    private readonly Func<ProjectCrmContext> _crmContextFactory;

    private readonly Func<ProjectCrmApiHttpClient> _httpClientFactory;

    public ProjectCrmContextSelectorDecorator(
        IFeatureManager featureManager,
        Func<ProjectCrmContext> crmContextFactory,
        Func<ProjectCrmApiHttpClient> httpClientFactory)
    {
        _featureManager = featureManager;
        _crmContextFactory = crmContextFactory;
        _httpClientFactory = httpClientFactory;
    }

    public async Task<IList<FrontDoorProjectDto>> GetOrganisationProjects(string userGlobalId, string organisationId, CancellationToken cancellationToken)
    {
        var context = await GetCrmContext();
        return await context.GetOrganisationProjects(userGlobalId, organisationId, cancellationToken);
    }

    public async Task<IList<FrontDoorProjectDto>> GetUserProjects(string userGlobalId, string organisationId, CancellationToken cancellationToken)
    {
        var context = await GetCrmContext();
        return await context.GetUserProjects(userGlobalId, organisationId, cancellationToken);
    }

    public async Task<FrontDoorProjectDto> GetOrganisationProjectById(string projectId, string userGlobalId, string organisationId, CancellationToken cancellationToken)
    {
        var context = await GetCrmContext();
        return await context.GetOrganisationProjectById(projectId, userGlobalId, organisationId, cancellationToken);
    }

    public async Task<FrontDoorProjectDto> GetUserProjectById(string projectId, string userGlobalId, string organisationId, CancellationToken cancellationToken)
    {
        var context = await GetCrmContext();
        return await context.GetUserProjectById(projectId, userGlobalId, organisationId, cancellationToken);
    }

    public async Task<bool> IsThereProjectWithName(string projectName, string organisationId, CancellationToken cancellationToken)
    {
        var context = await GetCrmContext();
        return await context.IsThereProjectWithName(projectName, organisationId, cancellationToken);
    }

    public async Task<string> Save(FrontDoorProjectDto dto, string userGlobalId, string organisationId, CancellationToken cancellationToken)
    {
        var context = await GetCrmContext();
        return await context.Save(dto, userGlobalId, organisationId, cancellationToken);
    }

    private async Task<IProjectCrmContext> GetCrmContext()
    {
        if (await _featureManager.IsEnabledAsync(FeatureFlags.UseFrontDoorApi))
        {
            return _httpClientFactory();
        }

        return _crmContextFactory();
    }
}
