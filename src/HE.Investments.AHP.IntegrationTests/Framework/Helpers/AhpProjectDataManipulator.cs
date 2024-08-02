using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.Investment.AHP.Domain.Project.Crm;
using HE.Investments.IntegrationTestsFramework.Auth;

namespace HE.Investments.AHP.IntegrationTests.Framework.Helpers;

public class AhpProjectDataManipulator
{
    private readonly ProjectCrmContext _projectCrmContext;

    public AhpProjectDataManipulator(ProjectCrmContext projectCrmContext)
    {
        _projectCrmContext = projectCrmContext;
    }

    public async Task CreateAhpProject(ILoginData loginData, string frontDoorProjectId, string projectName, string frontDoorSiteId, string siteName, string? consortiumId = null)
    {
        await _projectCrmContext.CreateProject(
            loginData.UserGlobalId,
            loginData.OrganisationId,
            consortiumId,
            frontDoorProjectId,
            projectName,
            [new SiteDto { fdSiteid = frontDoorSiteId, name = siteName }],
            CancellationToken.None);
    }

    public async Task<AhpProjectDto> GetAhpProject(ILoginData loginData, string projectId)
    {
        return await _projectCrmContext.GetProject(
            projectId,
            loginData.UserGlobalId,
            loginData.OrganisationId,
            null,
            CancellationToken.None);
    }
}
