using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.Investment.AHP.Domain.Application.Repositories;
using HE.Investment.AHP.Domain.Project.ValueObjects;
using HE.Investments.AHP.Allocation.Domain.Project.Crm;
using HE.Investments.Consortium.Shared.UserContext;
using HE.Investments.FrontDoor.Shared.Project;

namespace HE.Investments.AHP.Allocation.Domain.Project.Repositories;

public class ProjectAllocationRepository : IProjectAllocationRepository
{
    private readonly IProjectAllocationCrmContext _crmContext;

    public ProjectAllocationRepository(IProjectAllocationCrmContext crmContext)
    {
        _crmContext = crmContext;
    }

    public async Task<AhpProjectOverview> GetProjectAllocations(FrontDoorProjectId projectId, ConsortiumUserAccount userAccount, CancellationToken cancellationToken)
    {
        var project = await _crmContext.GetProjectAllocations(
            projectId.ToGuidAsString(),
            userAccount.UserGlobalId.ToString(),
            userAccount.SelectedOrganisationId().ToString(),
            userAccount.Consortium.GetConsortiumIdAsString(), // todo consortium id to be removed after adding new endpoint AB#104052 AB#104051
            cancellationToken);

        return new AhpProjectOverview(
            projectId,
            new AhpProjectName(project.AhpProjectName),
            allocations: MapAhpAllocation(project));
    }

    private List<AhpProjectAllocation>? MapAhpAllocation(AhpProjectDto project)
    {
        return project.ListOfAhpAllocations?
            .Select(x => new AhpProjectAllocation(
                x.Id,
                x.Name,
                x.Homes,
                ApplicationTenureMapper.ToDomain(x.Tenure)!.Value,
                x.LocalAuthority.name,
                null))
            .ToList();
    }
}
