using System;

namespace HE.Common.IntegrationModel.PortalIntegrationModel
{
    public class SiteDetailsDto
    {
        public string id { get; set; }
        public string currentValue { get; set; }
        public DateTime? dateOfPurchase { get; set; }
        public string existingLegalCharges { get; set; }
        public string existingLegalChargesInformation { get; set; }
        public string haveAPlanningReferenceNumber { get; set; }
        public string howMuch { get; set; }
        public string landRegistryTitleNumber { get; set; }
        public string Name { get; set; }
        public string nameOfGrantFund { get; set; }
        public string numberOfAffordableHomes { get; set; }
        public string numberOfHomes { get; set; }
        public string otherTypeOfHomes { get; set; }
        public string planningReferenceNumber { get; set; }
        public string publicSectorFunding { get; set; }
        public string reason { get; set; }
        public string siteCoordinates { get; set; }
        public string siteCost { get; set; }
        public string siteName { get; set; }
        public string siteOwnership { get; set; }
        public string[] typeOfHomes { get; set; }
        public string typeOfSite { get; set; }
        public string valuationSource { get; set; }
        public string whoProvided { get; set; }
    }
}