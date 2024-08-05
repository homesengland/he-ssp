using HE.Common.IntegrationModel.PortalIntegrationModel;

namespace HE.Investments.AHP.Allocation.Domain.Site.Crm;

public interface ISiteAllocationCrmContext
{
    Task<AhpSiteApplicationAndAllocationDto> GetSiteApplicationsAndAllocations(
        string siteId,
        string organisationId,
        string userId,
        string? consortiumId,
        CancellationToken cancellationToken);
}
