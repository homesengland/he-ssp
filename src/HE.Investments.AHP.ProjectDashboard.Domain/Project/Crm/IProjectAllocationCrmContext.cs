using HE.Common.IntegrationModel.PortalIntegrationModel;

namespace HE.Investments.AHP.ProjectDashboard.Domain.Project.Crm;

public interface IProjectAllocationCrmContext
{
    Task<ProjectWithAllocationListDto> GetProjectAllocations(
        string projectId,
        string userId,
        string organisationId,
        string? consortiumId,
        CancellationToken cancellationToken);
}
