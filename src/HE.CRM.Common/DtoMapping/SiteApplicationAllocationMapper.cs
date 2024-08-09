using System.Collections.Generic;
using DataverseModel;
using HE.Common.IntegrationModel.PortalIntegrationModel;

namespace HE.CRM.Common.DtoMapping
{
    public class SiteApplicationAllocationMapper
    {
        public static AhpSiteApplicationAllocationDto MapRegularEntityToDto(invln_Sites site, List<invln_scheme> applications, List<invln_scheme> allocations)
        {
            var siteApp = new AhpSiteApplicationAllocationDto
            {
                SiteId = site.Id.ToString(),
                fdSiteId = site.invln_HeProjectLocalAuthorityId?.Id.ToString(),
                ahpProjectId = site.invln_AHPProjectId?.Id.ToString(),
                siteName = site.invln_sitename.ToString(),
                siteStatus = site.StatusCode.ToString(),
                localAuthorityName = site.invln_LocalAuthority?.Name,
                AhpApplications = new List<AhpApplicationForSiteDto>(),
                AhpAllocations = new List<AhpAllocationForSiteDto>(),
            };

            foreach (var app in applications)
            {
                var ahpApplicationDto = new AhpApplicationForSiteDto
                {
                    applicationId = app.Id.ToString(),
                    applicationName = app.invln_schemename,
                    applicationStatus = app.invln_ExternalStatus?.Value,
                    requiredFunding = app.invln_fundingrequired?.Value.ToString(),
                    housesToDeliver = app?.invln_noofhomes,
                    tenure = app?.invln_Tenure?.Value,
                };
                siteApp.AhpApplications.Add(ahpApplicationDto);
            }

            foreach (var allo in allocations)
            {
                var ahpAllocationsDto = new AhpAllocationForSiteDto
                {
                    allocationId = allo.Id.ToString(),
                    allocationName = allo.invln_schemename,
                    housesToDeliver = allo?.invln_noofhomes,
                    tenure = allo?.invln_Tenure?.Value,
                };
                siteApp.AhpAllocations.Add(ahpAllocationsDto);
            }

            return siteApp;
        }
    }
}




