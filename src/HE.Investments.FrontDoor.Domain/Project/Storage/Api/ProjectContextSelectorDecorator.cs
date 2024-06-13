using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.Investments.Common;
using HE.Investments.FrontDoor.Domain.Project.Storage.Crm;
using Microsoft.FeatureManagement;

namespace HE.Investments.FrontDoor.Domain.Project.Storage.Api;

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

    public async Task<IList<FrontDoorProjectDto>> GetOrganisationProjects(string userGlobalId, string organisationId, CancellationToken cancellationToken)
    {
        var context = await GetProjectContext();
        return await context.GetOrganisationProjects(userGlobalId, organisationId, cancellationToken);
    }

    public async Task<IList<FrontDoorProjectDto>> GetUserProjects(string userGlobalId, string organisationId, CancellationToken cancellationToken)
    {
        var context = await GetProjectContext();
        return await context.GetUserProjects(userGlobalId, organisationId, cancellationToken);
    }

    public async Task<FrontDoorProjectDto> GetOrganisationProjectById(string projectId, string userGlobalId, string organisationId, CancellationToken cancellationToken)
    {
        var context = await GetProjectContext();
        return await context.GetOrganisationProjectById(projectId, userGlobalId, organisationId, cancellationToken);
    }

    public async Task<FrontDoorProjectDto> GetUserProjectById(string projectId, string userGlobalId, string organisationId, CancellationToken cancellationToken)
    {
        var context = await GetProjectContext();
        return await context.GetUserProjectById(projectId, userGlobalId, organisationId, cancellationToken);
    }

    public async Task<bool> IsThereProjectWithName(string projectName, string organisationId, CancellationToken cancellationToken)
    {
        var context = await GetProjectContext();
        return await context.IsThereProjectWithName(projectName, organisationId, cancellationToken);
    }

    public async Task<string> Save(FrontDoorProjectDto dto, string userGlobalId, string organisationId, CancellationToken cancellationToken)
    {
        var context = await GetProjectContext();
        return await context.Save(dto, userGlobalId, organisationId, cancellationToken);
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
