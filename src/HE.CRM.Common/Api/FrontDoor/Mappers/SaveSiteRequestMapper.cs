using System;
using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.CRM.Common.Api.FrontDoor.Contract.Requests;

namespace HE.CRM.Common.Api.FrontDoor.Mappers
{
    public static class SaveSiteRequestMapper
    {
        public static SaveSiteRequest Map(FrontDoorProjectSiteDto site, Guid projectId)
        {
            return new SaveSiteRequest
            {
                ProjectRecordId = projectId,
                SiteId = string.IsNullOrEmpty(site.SiteId) ? (Guid?)null : Guid.Parse(site.SiteId),
                SiteName = site.SiteName,
                PlanningStatus = site.PlanningStatus,
                LocalAuthorityCode = site.LocalAuthorityCode,
                NumberOfHomesEnabledBuilt = site.NumberofHomesEnabledBuilt,
            };
        }
    }
}
