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
        public DateTime? CreatedOn { get; set; }
        public string LocalAuthority { get; set; }
        public string LocalAuthorityName { get; set; }
        public string LocalAuthorityCode { get; set; }
    }
}
