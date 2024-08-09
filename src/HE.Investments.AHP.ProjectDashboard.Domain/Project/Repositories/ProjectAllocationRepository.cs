using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.Investment.AHP.Contract.Application;
using HE.Investments.AHP.ProjectDashboard.Domain.Project.Crm;
using HE.Investments.AHP.ProjectDashboard.Domain.Project.Mappers;
using HE.Investments.AHP.ProjectDashboard.Domain.Project.ValueObjects;
using HE.Investments.Common.CRM.Mappers;
using HE.Investments.Consortium.Shared.UserContext;
using HE.Investments.FrontDoor.Shared.Project;

namespace HE.Investments.AHP.ProjectDashboard.Domain.Project.Repositories;

public class ProjectAllocationRepository : IProjectAllocationRepository
{
    private readonly IProjectAllocationCrmContext _crmContext;

    private readonly EnumMapper<Tenure> _tenureMapper = new TenureMapper();

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
            new AhpProjectName(project.ProjectName),
            allocations: MapAhpAllocations(project));
    }

    private List<AhpProjectAllocation>? MapAhpAllocations(ProjectWithAllocationListDto project)
    {
        return project.ListOfAllocations?
            .Select(x => new AhpProjectAllocation(
                x.Id.ToString(),
                x.Name,
                x.Homes,
                _tenureMapper.ToDomain(x.Tenure)!.Value,
                x.LocalAuthority.name,
                null,
                x.HasMilestoneInDueState))
            .ToList();
    }
}
