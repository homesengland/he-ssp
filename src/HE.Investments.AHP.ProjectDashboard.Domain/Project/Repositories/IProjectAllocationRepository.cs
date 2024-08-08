using HE.Investments.AHP.ProjectDashboard.Domain.Project.ValueObjects;
using HE.Investments.Consortium.Shared.UserContext;
using HE.Investments.FrontDoor.Shared.Project;

namespace HE.Investments.AHP.ProjectDashboard.Domain.Project.Repositories;

public interface IProjectAllocationRepository
{
    Task<AhpProjectOverview> GetProjectAllocations(FrontDoorProjectId projectId, ConsortiumUserAccount userAccount, CancellationToken cancellationToken);
}
