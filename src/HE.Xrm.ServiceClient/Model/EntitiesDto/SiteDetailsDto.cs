using Microsoft.Xrm.Sdk;

namespace HE.Xrm.ServiceClientExample.Model.EntitiesDto
{
    public class SiteDetailsDto
    {

        public Guid id { get; set; }
        public Money currentValue { get; set; }
        public DateTime? dateOfPurchase { get; set; }
        public bool? existingLegalCharges { get; set; }
        public string existingLegalChargesInformation { get; set; }
        public bool? haveAPlanningReferenceNumber { get; set; }
        public Money howMuch { get; set; }
        public string landRegistryTitleNumber { get; set; }
        public EntityReference loanApplication { get; set; }
        public string Name { get; set; }
        public string nameOfGrantFund { get; set; }
        public int? numberOfAffordableHomes { get; set; }
        public int? numberOfHomes { get; set; }
        public string otherTypeOfHomes { get; set; }
        public string planningReferenceNumber { get; set; }
        public OptionSetValue publicSectorFunding { get; set; }
        public string reason { get; set; }
        public string siteCoordinates { get; set; }
        public Money siteCost { get; set; }
        public string siteName { get; set; }
        public bool? siteOwnership { get; set; }
        public OptionSetValueCollection typeOfHomes { get; set; }
        public OptionSetValue typeOfSite { get; set; }
        public OptionSetValue valuationSource { get; set; }
        public string whoProvided { get; set; }
    }
}
