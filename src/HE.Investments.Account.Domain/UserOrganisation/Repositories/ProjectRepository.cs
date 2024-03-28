using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.Investments.Account.Contract.UserOrganisation;
using HE.Investments.Account.Shared.User;
using HE.Investments.Common.Contract;
using HE.Investments.Common.CRM.Extensions;
using HE.Investments.Common.CRM.Model;
using HE.Investments.Common.CRM.Services;
using HE.Investments.Common.Extensions;
using Microsoft.FeatureManagement;

namespace HE.Investments.Account.Domain.UserOrganisation.Repositories;

public class ProjectRepository : IProjectRepository
{
    private readonly ICrmService _crmService;

    private readonly IFeatureManager _featureManager;

    public ProjectRepository(ICrmService crmService, IFeatureManager featureManager)
    {
        _crmService = crmService;
        _featureManager = featureManager;
    }

    public async Task<IList<UserProject>> GetUserProjects(UserAccount userAccount, CancellationToken cancellationToken)
    {
        var request = new invln_getmultiplefrontdoorprojectsRequest
        {
            invln_organisationid = userAccount.SelectedOrganisationId().ToString(),
            inlvn_userid = userAccount.CanViewAllApplications() ? string.Empty : userAccount.UserGlobalId.ToString(),
            invln_fieldstoretrieve = nameof(invln_FrontDoorProjectPOC.invln_Name).ToLowerInvariant(),
            invln_usehetables = await _featureManager.GetUseHeTablesParameter(),
        };

        var projects = await _crmService.ExecuteAsync<invln_getmultiplefrontdoorprojectsRequest, invln_getmultiplefrontdoorprojectsResponse, IList<FrontDoorProjectDto>>(
            request,
            r => string.IsNullOrEmpty(r.invln_frontdoorprojects) ? "[]" : r.invln_frontdoorprojects,
            cancellationToken);

        return projects
            .OrderByDescending(x => x.CreatedOn ?? DateTime.MinValue)
            .Select(x => new UserProject(
                HeProjectId.From(x.ProjectId),
                x.ProjectName,
                ApplicationStatus.Draft.GetDescription()))
            .ToList();
    }
}
