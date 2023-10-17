#pragma warning disable IDE0005 // Using directive is unnecessary.
using System;
using System.Text.Json.Serialization;
using Microsoft.Xrm.Sdk;
#pragma warning restore IDE0005 // Using directive is unnecessary.

namespace HE.Common.IntegrationModel.PortalIntegrationModel
{
    public class SiteDetailsDto
    {
        public string siteDetailsId { get; set; }

        public string currentValue { get; set; }

        public DateTime? dateOfPurchase { get; set; }

        public bool? existingLegalCharges { get; set; }

        public string existingLegalChargesInformation { get; set; }

        public bool? haveAPlanningReferenceNumber { get; set; }

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

        public bool? siteOwnership { get; set; }

        public string[] typeOfHomes { get; set; }

        public string typeOfSite { get; set; }

        public string valuationSource { get; set; }

        public string whoProvided { get; set; }

        public int? planningPermissionStatus { get; set; }

        public DateTime? startDate { get; set; }

        public bool? affordableHousing { get; set; }

        public int? completionStatus { get; set; }

        public bool? projectHasStartDate { get; set; }
    }
}
