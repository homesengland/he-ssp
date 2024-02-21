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
        public string moveOnArrangements { get; set; }
        public List<int> fundingSources { get; set; }
        public string exitPlan { get; set; }
        public string typologyLocationAndDesign { get; set; }
        public int? numberOfBedrooms { get; set; }
        public int? maxOccupancy { get; set; }
        public int? numberOfStoreys { get; set; }
        public System.DateTime? createdOn { get; set; }
        public int? buildingType { get; set; }
        public int? sharedFacilities { get; set; }
        public bool? isMoveOnAccommodation { get; set; }
        public bool? needsOfParticularGroup { get; set; }
        public int? homesDesignedForUseOfParticularGroup { get; set; }
        public bool? areHomesCustomBuild { get; set; }
        public int? accessibilityCategory { get; set; }
        public decimal? marketValue { get; set; }
        public decimal? marketRent { get; set; }
        public decimal? prospectiveRent { get; set; }
        public bool? isWheelchairStandardMet { get; set; }
        public string designPlansMoreInformation { get; set; }
        public bool? RtSOExemption { get; set; }
        public decimal? initialSalePercent { get; set; }
        public decimal? prospectiveRentAsPercentOfMarketRent { get; set; }
        public bool? isCompleted { get; set; }
        public string exemptionJustification { get; set; }
        public decimal? floorArea { get; set; }
        public bool? doAllHomesMeetNDSS { get; set; }
        public List<int> whichNDSSStandardsHaveBeenMet { get; set; }
        public bool? targetRentOver80PercentOfMarketRent { get; set; }
        public decimal? expectedFirstTrancheSaleReceipt { get; set; }
        public decimal? proposedRentAsPercentOfUnsoldShare { get; set; }
    }
}
