using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.Investments.Account.Domain.UserOrganisation.Storage.Api;
using HE.Investments.Account.Domain.UserOrganisation.Storage.Crm;
using HE.Investments.Common;
using Microsoft.FeatureManagement;

namespace HE.Investments.Account.Domain.UserOrganisation.Storage;

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

    public async Task<IList<FrontDoorProjectDto>> GetOrganisationProjects(string organisationId, CancellationToken cancellationToken)
    {
        var context = await GetProjectContext();
        return await context.GetOrganisationProjects(organisationId, cancellationToken);
    }

    public async Task<IList<FrontDoorProjectDto>> GetUserProjects(string userGlobalId, string organisationId, CancellationToken cancellationToken)
    {
        var context = await GetProjectContext();
        return await context.GetUserProjects(userGlobalId, organisationId, cancellationToken);
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
