using System.Collections.Generic;
using System.Security.Policy;

namespace HE.Common.IntegrationModel.PortalIntegrationModel
{

    public class AHPSiteApplicationDto
    {
        public string SiteId { get; set; }
        public string fdSiteId { get; set; }
        public string siteName { get; set; }
        public string siteStatus { get; set; }
        public string ahpProjectId { get; set; }
        public List<FrontDoorHPApplicationDto> AhpApplications { get; set; }
    }
}
