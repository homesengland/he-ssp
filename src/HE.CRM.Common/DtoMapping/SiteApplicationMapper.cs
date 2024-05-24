using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Services;
using DataverseModel;
using HE.Common.IntegrationModel.PortalIntegrationModel;
using Microsoft.Xrm.Sdk;

namespace HE.CRM.Common.DtoMapping
{
    public class SiteApplicationMapper
    {
        public static AhpSiteApplicationDto MapRegularEntityToDto(invln_Sites site, List<invln_scheme> applications)
        {
            var siteApp = new AhpSiteApplicationDto
            {
                SiteId = site.Id.ToString(),
                fdSiteId = site.invln_HeProjectLocalAuthorityId?.Id.ToString(),
                ahpProjectId = site.invln_AHPProjectId?.Id.ToString(),
                siteName = site.invln_HeProjectLocalAuthorityId?.Id.ToString(),
                siteStatus = site.StatusCode.ToString(),
                localAuthorityName = site.invln_LocalAuthority?.Name,
                AhpApplications = new List<AhpApplicationForSiteDto>(),
            };

            foreach (var app in applications)
            {
                var ahpApplicationDto = new AhpApplicationForSiteDto
                {
                    applicationId = app.Id.ToString(),
                    applicationName = app.invln_schemename,
                    applicationStatus = app.StatusCode?.Value,
                    requiredFunding = app.invln_fundingrequired?.Value.ToString(),
                    housesToDeliver = app.invln_noofhomes == null ? string.Empty : app.invln_noofhomes.Value.ToString(),
                    tenure = app?.invln_Tenure?.Value.ToString(),
                };
                siteApp.AhpApplications.Add(ahpApplicationDto);
            }
            return siteApp;
        }
    }
}
