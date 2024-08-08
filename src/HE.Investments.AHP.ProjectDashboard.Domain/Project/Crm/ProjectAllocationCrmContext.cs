using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.Investments.Common.CRM.Model;
using HE.Investments.Common.CRM.Services;
using HE.Investments.Common.Extensions;

namespace HE.Investments.AHP.ProjectDashboard.Domain.Project.Crm;

public class ProjectAllocationCrmContext : IProjectAllocationCrmContext
{
    private readonly ICrmService _service;

    public ProjectAllocationCrmContext(ICrmService service)
    {
        _service = service;
    }

    public async Task<ProjectWithAllocationListDto> GetProjectAllocations(string projectId, string userId, string organisationId, string? consortiumId, CancellationToken cancellationToken)
    {
        // return await _service.ExecuteAsync<invln_getprojectallocationsRequest, invln_getprojectallocationsResponse, ProjectWithAllocationListDto>(
        //     new invln_getahpprojectRequest
        ////     {
        //         invln_userid = userId,
        //         invln_accountid = organisationId.TryToGuidAsString(),
        //         invln_heprojectid = projectId.TryToGuidAsString(),
        //     },
        //     r => r.invln_ahpProjectAllocations,
        ////     cancellationToken); todo crm wire up AB#104052 AB#104051

        var project = await _service.ExecuteAsync<invln_getahpproject_v2Request, invln_getahpproject_v2Response, AhpProjectDto>(
        new invln_getahpproject_v2Request
        {
            invln_userid = userId,
            invln_accountid = organisationId.TryToGuidAsString(),
            invln_consortiumid = consortiumId?.ToGuidAsString()!,
            invln_heprojectid = projectId.TryToGuidAsString(),
        },
        r => r.invln_ahpProjectApplications,
        cancellationToken);

        return new ProjectWithAllocationListDto
        {
            ProjectId = new Guid(projectId),
            ProjectName = project.AhpProjectName,
            ListOfAllocations = MapAllocations(project),
        };
    }

    private List<AllocationDto>? MapAllocations(AhpProjectDto project)
    {
        var random = new Random();
        return project.ListOfAhpAllocations?
            .Select(x => new AllocationDto
            {
                Id = new Guid(x.Id),
                Name = x.Name,
                Homes = x.Homes,
                Tenure = x.Tenure,
                LocalAuthority = x.LocalAuthority,
                HasMilestoneInDueState = random.Next(2) == 0,
            })
            .ToList();
    }
}
