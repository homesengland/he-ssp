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
        public bool? localComissioningBodies { get; set; }
        public bool? shortStayAccommodation { get; set; }
        public int? revenueFunding { get; set; }
        public string moveOnArrangementsForNoRevenueFunding { get; set; }
        public List<int> fundingSources { get; set;}
        public string moveOnArrangementsForRevenueFunding { get; set; }
        public string exitPlan { get; set; }
        public string typologyLocationAndDesign { get; set; }
        public int? numberOfBedrooms { get; set; }
        public int? maxOccupancy { get; set; }
        public int? numberOfStoreys { get; set; }
        public System.DateTime? createdOn { get; set; }
    }
}
