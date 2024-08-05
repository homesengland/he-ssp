using System.Collections.Generic;

namespace HE.Common.IntegrationModel.PortalIntegrationModel
{
    public class AhpSiteApplicationAndAllocationDto : AhpSiteApplicationDto
    {
        public List<AhpAllocationForSiteDto> AhpAllocations { get; set; }
    }
}
