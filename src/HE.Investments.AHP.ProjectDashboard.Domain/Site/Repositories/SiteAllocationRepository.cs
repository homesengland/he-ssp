using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.Investment.AHP.Contract.Application;
using HE.Investment.AHP.Contract.Site;
using HE.Investments.AHP.Allocation.Contract;
using HE.Investments.AHP.ProjectDashboard.Domain.Project.Mappers;
using HE.Investments.AHP.ProjectDashboard.Domain.Site.Crm;
using HE.Investments.AHP.ProjectDashboard.Domain.Site.ValueObjects;
using HE.Investments.Common.CRM.Mappers;
using HE.Investments.Consortium.Shared.UserContext;

namespace HE.Investments.AHP.ProjectDashboard.Domain.Site.Repositories;

public sealed class SiteAllocationRepository : ISiteAllocationRepository
{
    private readonly ISiteAllocationCrmContext _crmContext;

    private readonly EnumMapper<Tenure> _tenureMapper = new TenureMapper();

    public SiteAllocationRepository(ISiteAllocationCrmContext crmContext)
    {
        _crmContext = crmContext;
    }

    public async Task<(IList<ApplicationBasicDetails> Applications, IList<AllocationSiteDetails> Allocations)> GetSiteApplicationsAndAllocations(
        SiteId siteId,
        ConsortiumUserAccount userAccount,
        CancellationToken cancellationToken)
    {
        var siteApplicationsAndAllocations = await _crmContext.GetSiteApplicationsAndAllocations(
            siteId.ToGuidAsString(),
            userAccount.SelectedOrganisationId().ToGuidAsString(),
            userAccount.UserGlobalId.ToString(),
            userAccount.Consortium.GetConsortiumIdAsString(),
            cancellationToken);

        return (siteApplicationsAndAllocations.AhpApplications.Select(CreateApplication).ToList(),
            siteApplicationsAndAllocations.AhpAllocations.Select(CreateAllocation).ToList());
    }

    private ApplicationBasicDetails CreateApplication(AhpApplicationForSiteDto application)
    {
        return new ApplicationBasicDetails(
            AhpApplicationId.From(application.applicationId),
            application.applicationName,
            AhpApplicationStatusMapper.MapToPortalStatus(application.applicationStatus),
            _tenureMapper.ToDomain(application.tenure)!.Value,
            null,
            application.housesToDeliver);
    }

    private AllocationSiteDetails CreateAllocation(AhpAllocationForSiteDto allocation)
    {
        return new AllocationSiteDetails(
            AllocationId.From(allocation.allocationId),
            allocation.allocationName,
            _tenureMapper.ToDomain(allocation.tenure)!.Value,
            allocation.housesToDeliver);
    }
}
