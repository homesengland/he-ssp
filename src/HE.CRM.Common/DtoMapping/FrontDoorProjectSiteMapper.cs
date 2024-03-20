using System;
using System.Collections.Generic;
using System.Linq;
using DataverseModel;
using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.CRM.Common.Extensions.Entities;
using Microsoft.Xrm.Sdk;

namespace HE.CRM.Common.DtoMapping
{
    public class FrontDoorProjectSiteMapper
    {
        public static FrontDoorProjectSiteDto MapFrontDoorProjectSiteToDto(invln_FrontDoorProjectSitePOC frontDoorProjectSite, invln_localauthority localauthority = null)
        {
            var frontDoorProjectSiteToReturn = new FrontDoorProjectSiteDto()
            {
                SiteId = frontDoorProjectSite.invln_FrontDoorProjectSitePOCId?.ToString(),
                SiteName = frontDoorProjectSite.invln_Name,
                NumberofHomesEnabledBuilt = frontDoorProjectSite.invln_NumberofHomesEnabledBuilt,
                PlanningStatus = frontDoorProjectSite.invln_PlanningStatus?.Value,
                CreatedOn = frontDoorProjectSite.CreatedOn,
                LocalAuthority = frontDoorProjectSite.invln_LocalAuthorityId != null ? frontDoorProjectSite.invln_LocalAuthorityId.Id.ToString() : string.Empty,
                LocalAuthorityName = frontDoorProjectSite.invln_LocalAuthorityId != null ? frontDoorProjectSite.invln_LocalAuthorityId.Name : string.Empty,
                LocalAuthorityCode = localauthority.invln_onscode != null ? localauthority.invln_onscode : string.Empty,
            };

            return frontDoorProjectSiteToReturn;
        }

        public static invln_FrontDoorProjectSitePOC MapFrontDoorProjectSiteDtoToRegularEntity(FrontDoorProjectSiteDto frontDoorProjectSiteDto, string frontdoorprojectId)
        {
            var frontDoorProjectSiteDtoToReturn = new invln_FrontDoorProjectSitePOC()
            {
                invln_Name = frontDoorProjectSiteDto.SiteName,
                invln_NumberofHomesEnabledBuilt = frontDoorProjectSiteDto.NumberofHomesEnabledBuilt,
                invln_PlanningStatus = frontDoorProjectSiteDto.PlanningStatus.HasValue ? new OptionSetValue(frontDoorProjectSiteDto.PlanningStatus.Value) : null,
            };

            if (Guid.TryParse(frontdoorprojectId, out Guid projectId))
            {
                frontDoorProjectSiteDtoToReturn.invln_FrontDoorProjectId = new EntityReference(invln_FrontDoorProjectPOC.EntityLogicalName, projectId);
            }

            if (!string.IsNullOrEmpty(frontDoorProjectSiteDto.LocalAuthority) && Guid.TryParse(frontDoorProjectSiteDto.LocalAuthority, out Guid localAuthorityGUID) && localAuthorityGUID != Guid.Empty)
            {
                frontDoorProjectSiteDtoToReturn.invln_LocalAuthorityId = new EntityReference(invln_localauthority.EntityLogicalName, localAuthorityGUID);
            }

            return frontDoorProjectSiteDtoToReturn;
        }
    }
}






