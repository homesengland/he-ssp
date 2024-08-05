using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.Investments.Common;
using HE.Investments.Common.CRM.Model;
using HE.Investments.Common.CRM.Services;
using HE.Investments.Common.Extensions;
using Microsoft.FeatureManagement;

namespace HE.Investments.AHP.Allocation.Domain.Site.Crm;

internal sealed class SiteAllocationCrmContext : ISiteAllocationCrmContext
{
    private readonly ICrmService _service;

    private readonly IFeatureManager _featureManager;

    public SiteAllocationCrmContext(ICrmService service, IFeatureManager featureManager)
    {
        _service = service;
        _featureManager = featureManager;
    }

    public async Task<AhpSiteApplicationAndAllocationDto> GetSiteApplicationsAndAllocations(
        string siteId,
        string organisationId,
        string userId,
        string? consortiumId,
        CancellationToken cancellationToken)
    {
        var request = new invln_getsiteapplicationsRequest
        {
            invln_userid = userId,
            invln_organizationid = organisationId,
            invln_consortiumid = consortiumId?.TryToGuidAsString() ?? string.Empty,
            invln_siteid = siteId,
        };

        var result = await _service.ExecuteAsync<invln_getsiteapplicationsRequest, invln_getsiteapplicationsResponse, AhpSiteApplicationDto>(
            request,
            r => r.invln_siteapplication,
            cancellationToken);

        return new AhpSiteApplicationAndAllocationDto
        {
            SiteId = result.SiteId,
            fdSiteId = result.fdSiteId,
            siteName = result.siteName,
            siteStatus = result.siteStatus,
            ahpProjectId = result.ahpProjectId,
            localAuthorityName = result.localAuthorityName,
            AhpApplications = result.AhpApplications,
            AhpAllocations = await GetMockedAllocations(),
        };
    }

    private async Task<List<AhpAllocationForSiteDto>> GetMockedAllocations()
    {
        if (await _featureManager.IsEnabledAsync(FeatureFlags.TurnOffAhpAllocation))
        {
            return [];
        }

        return
        [
            new AhpAllocationForSiteDto
            {
                allocationId = Guid.NewGuid().ToString(),
                allocationName = "My first allocation",
                housesToDeliver = 12,
                tenure = (int)invln_Tenure.Affordablerent,
            },
            new AhpAllocationForSiteDto
            {
                allocationId = Guid.NewGuid().ToString(),
                allocationName = "My second allocation",
                housesToDeliver = 24,
                tenure = (int)invln_Tenure.HOLD,
            },
        ];
    }
}
