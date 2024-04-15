extern alias Org;

using HE.Investments.Common.CRM.Model;
using HE.Investments.Common.CRM.Serialization;
using HE.Investments.Common.CRM.Services;
using HE.Investments.FrontDoor.Shared.Project;
using HE.Investments.FrontDoor.Shared.Project.Contract;
using HE.Investments.FrontDoor.Shared.Project.Crm;
using HE.Investments.IntegrationTestsFramework.Auth;
using HE.Investments.Loans.IntegrationTests.Config;
using Org::HE.Common.IntegrationModel.PortalIntegrationModel;

namespace HE.Investments.Loans.IntegrationTests.Crm;

public class FrontDoorProjectCrmRepository
{
    private readonly ICrmService _crmService;

    public FrontDoorProjectCrmRepository(ICrmService crmService)
    {
        _crmService = crmService;
    }

    public async Task CreateProject(FrontDoorProjectData projectData, ILoginData loginData)
    {
        var dto = new FrontDoorProjectDto
        {
            ProjectName = projectData.Name,
            ProjectSupportsHousingDeliveryinEngland = projectData.IsEnglandHousingDelivery,
            OrganisationId = Guid.Parse(loginData.OrganisationId),
            externalId = loginData.UserGlobalId,
            ActivitiesinThisProject = Map(projectData.SupportActivities),
        };

        var request = new invln_setfrontdoorprojectRequest
        {
            invln_userid = loginData.UserGlobalId,
            invln_organisationid = loginData.OrganisationId,
            invln_entityfieldsparameters = CrmResponseSerializer.Serialize(dto),
            invln_frontdoorprojectid = dto.ProjectId,
            invln_usehetables = "true",
        };

        var projectId = await _crmService.ExecuteAsync<invln_setfrontdoorprojectRequest, invln_setfrontdoorprojectResponse>(
            request,
            r => r.invln_frontdoorprojectid,
            CancellationToken.None);

        projectData.SetProjectId(new FrontDoorProjectId(projectId));
    }

    private List<int> Map(IEnumerable<SupportActivityType> activities)
    {
        return activities.Select(x => FrontDoorProjectEnumMapping.ActivityType[x]).Where(x => x != null).Select(x => x!.Value).ToList();
    }
}
