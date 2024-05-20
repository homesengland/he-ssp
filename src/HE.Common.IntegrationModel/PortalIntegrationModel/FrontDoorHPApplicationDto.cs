using System;
using System.Collections.Generic;
using System.Security.Policy;

namespace HE.Common.IntegrationModel.PortalIntegrationModel
{
    public class FrontDoorHPApplicationDto
    {
        public string applicationId { get; set; }
        public string applicationName { get; set; }
        public int? applicationStatus { get; set; }
        public string requiredFunding { get; set; }
        public string housesToDeliver { get; set; }
        public string tenure { get; set; }
    }
}
