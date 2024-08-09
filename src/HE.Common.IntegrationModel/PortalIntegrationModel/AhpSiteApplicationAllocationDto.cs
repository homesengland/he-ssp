using System.Collections.Generic;

namespace HE.Common.IntegrationModel.PortalIntegrationModel
{
    public class AhpSiteApplicationAllocationDto
    {
        public string SiteId { get; set; }
        public string fdSiteId { get; set; }
        public string siteName { get; set; }
        public string siteStatus { get; set; }
        public string ahpProjectId { get; set; }
        public string localAuthorityName { get; set; }
        public List<AhpApplicationForSiteDto> AhpApplications { get; set; }
        public List<AhpAllocationForSiteDto> AhpAllocations { get; set; }
    }
}
