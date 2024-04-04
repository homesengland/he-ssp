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
            else
            {
                frontDoorProjectSiteDtoToReturn.invln_LocalAuthorityId = null;
            }

            return frontDoorProjectSiteDtoToReturn;
        }




        public static FrontDoorProjectSiteDto MapHeFrontDoorProjectSiteToDto(he_ProjectLocalAuthority frontDoorProjectSite, he_LocalAuthority localauthority = null)
        {
            var frontDoorProjectSiteToReturn = new FrontDoorProjectSiteDto()
            {
                SiteId = frontDoorProjectSite.he_ProjectLocalAuthorityId?.ToString(),
                SiteName = frontDoorProjectSite.he_Name,
                NumberofHomesEnabledBuilt = frontDoorProjectSite.he_Homes,
                PlanningStatus = frontDoorProjectSite.he_planningstatusofthesite?.Value,
                CreatedOn = frontDoorProjectSite.CreatedOn,
                LocalAuthority = frontDoorProjectSite.he_LocalAuthority != null ? frontDoorProjectSite.he_LocalAuthority.Id.ToString() : string.Empty,
                LocalAuthorityName = frontDoorProjectSite.he_LocalAuthority != null ? frontDoorProjectSite.he_LocalAuthority.Name : string.Empty,
                LocalAuthorityCode = localauthority.he_GSSCode != null ? localauthority.he_GSSCode : string.Empty,
            };

            return frontDoorProjectSiteToReturn;
        }


        public static he_ProjectLocalAuthority MapHeFrontDoorProjectSiteDtoToRegularEntity(FrontDoorProjectSiteDto frontDoorProjectSiteDto, string frontdoorprojectId)
        {
            var projectLocalAuthorityToReturn = new he_ProjectLocalAuthority()
            {
                he_Name = frontDoorProjectSiteDto.SiteName,
                he_Homes = frontDoorProjectSiteDto.NumberofHomesEnabledBuilt,
                he_planningstatusofthesite = frontDoorProjectSiteDto.PlanningStatus.HasValue ? new OptionSetValue(frontDoorProjectSiteDto.PlanningStatus.Value) : null,
            };

            if (Guid.TryParse(frontdoorprojectId, out Guid projectId))
            {
                projectLocalAuthorityToReturn.he_Project = new EntityReference(he_Pipeline.EntityLogicalName, projectId);
            }

            if (!string.IsNullOrEmpty(frontDoorProjectSiteDto.LocalAuthority) && Guid.TryParse(frontDoorProjectSiteDto.LocalAuthority, out Guid localAuthorityGUID) && localAuthorityGUID != Guid.Empty)
            {
                projectLocalAuthorityToReturn.he_LocalAuthority = new EntityReference(he_LocalAuthority.EntityLogicalName, localAuthorityGUID);
            }
            else
            {
                projectLocalAuthorityToReturn.he_LocalAuthority = null;
            }

            return projectLocalAuthorityToReturn;
        }

    }
}






