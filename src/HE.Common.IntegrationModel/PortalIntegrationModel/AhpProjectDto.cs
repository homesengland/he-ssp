using System;
using System.Collections.Generic;

namespace HE.Common.IntegrationModel.PortalIntegrationModel
{
    public class AhpProjectDto
    {
        public string AhpProjectId { get; set; }
        public string AhpProjectName { get; set; }
        public string FrontDoorProjectId { get; set; }
        public string FrontDoorProjectName { get; set; }        
        public List<SiteDto> ListOfSites { get; set; }
        public List<AhpApplicationDto> ListOfApplications { get; set; }
    }
}
