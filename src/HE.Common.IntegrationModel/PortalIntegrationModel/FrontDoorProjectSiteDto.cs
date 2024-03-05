using System;
using System.Collections.Generic;
using System.Text;

namespace HE.Common.IntegrationModel.PortalIntegrationModel
{
    public class FrontDoorProjectSiteDto
    {
        public string SiteId { get; set; }
        public string SiteName { get; set; }
        public int? NumberofHomesEnabledBuilt { get; set; }        
        public int? PlanningStatus { get; set; }
    }
}
