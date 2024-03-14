using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.Investments.Account.Contract.UserOrganisation;
using HE.Investments.Account.Shared.User;
using HE.Investments.Common.Contract;
using HE.Investments.Common.CRM.Model;
using HE.Investments.Common.CRM.Services;
using HE.Investments.Common.Extensions;

namespace HE.Investments.Account.Domain.UserOrganisation.Repositories;

public class ProjectRepository : IProjectRepository
{
    private readonly ICrmService _crmService;

    public ProjectRepository(ICrmService crmService)
    {
        _crmService = crmService;
    }

    public async Task<IList<UserProject>> GetUserProjects(UserAccount userAccount, CancellationToken cancellationToken)
    {
        var request = new invln_getmultiplefrontdoorprojectsRequest
        {
            invln_organisationid = userAccount.SelectedOrganisationId().ToString(),
            inlvn_userid = userAccount.CanViewAllApplications() ? string.Empty : userAccount.UserGlobalId.ToString(),
            invln_fieldstoretrieve = nameof(invln_FrontDoorProjectPOC.invln_Name).ToLowerInvariant(),
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
