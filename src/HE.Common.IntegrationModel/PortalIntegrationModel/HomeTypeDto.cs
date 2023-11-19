
using System.Collections.Generic;

namespace HE.Common.IntegrationModel.PortalIntegrationModel
{
    public class HomeTypeDto
    {
        public string id { get; set; }
        public string homeTypeName { get; set; }
        public int? numberOfHomes { get; set; }
        public int? housingType { get; set; }
        public int? housingTypeForOlderPeople { get; set; }
        public int? housingTypeForVulnerable { get; set; }
        public int? clientGroup { get; set; }
        public List<int> designPrinciples { get; set; }
        public string applicationId { get; set; }
    }
}
