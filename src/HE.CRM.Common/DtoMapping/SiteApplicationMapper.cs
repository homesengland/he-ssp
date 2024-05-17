using System;
using System.Collections.Generic;
using System.Linq;
using DataverseModel;
using HE.Common.IntegrationModel.PortalIntegrationModel;
using Microsoft.Xrm.Sdk;

namespace HE.CRM.Common.DtoMapping
{
    public class SiteApplicationMapper
    {
        public static AHPSiteApplicationDto MapRegularEntityToDto(invln_Sites site, List<invln_scheme> applications)
        {
            var siteApp = new AHPSiteApplicationDto();
            siteApp.SiteId = site.Id.ToString();
            siteApp.fdSiteId = "";
            siteApp.ahpProjectId = site.invln_AHPProjectId.ToString();
            siteApp.siteName = site.invln_sitename;
            siteApp.siteStatus = site.StatusCode.ToString();
            List<invln_scheme> apps = new List<invln_scheme>();

            foreach (var app in applications)
            {
                FrontDoorHPApplicationDto ahpApplicationDto = new FrontDoorHPApplicationDto();
                ahpApplicationDto.applicationId = app.Id.ToString();
                ahpApplicationDto.applicationName = app.invln_schemename;
                ahpApplicationDto.applicationStatus = app.StatusCode.Value;
                ahpApplicationDto.requiredFunding = app.invln_fundingrequired.Value.ToString();
                ahpApplicationDto.housesToDeliver = app.invln_noofhomes.Value.ToString();
                ahpApplicationDto.tenure = app.invln_Tenure.ToString();
            }
            return siteApp;
        }
    }
}
