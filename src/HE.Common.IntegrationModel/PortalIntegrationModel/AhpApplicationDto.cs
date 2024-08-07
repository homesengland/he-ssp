using System;
using System.Web;

namespace HE.Common.IntegrationModel.PortalIntegrationModel
{
    public class AhpApplicationDto
    {
        public string id { get; set; }
        public string name { get; set; }
        public int? tenure { get; set; }
        public string referenceNumber { get; set; }
        public int? applicationStatus { get; set; }
        public string fdProjectId { get; set; }
        public string siteId { get; set; }
        public string programmeId { get; set; }
        public string applicationName { get; set; }

        //sections statuses
        public int? schemeInformationSectionCompletionStatus { get; set; }

        public int? homeTypesSectionCompletionStatus { get; set; }
        public int? financialDetailsSectionCompletionStatus { get; set; }
        public int? deliveryPhasesSectionCompletionStatus { get; set; }
        public DateTime? dateSubmitted { get; set; }
        public int? previousExternalStatus { get; set; }

        //financial
        public decimal? borrowingAgainstRentalIncomeFromThisScheme { get; set; }

        public decimal? fundingFromOpenMarketHomesOnThisScheme { get; set; }
        public decimal? fundingFromOpenMarketHomesNotOnThisScheme { get; set; }
        public decimal? fundingGeneratedFromOtherSources { get; set; }
        public decimal? recycledCapitalGrantFund { get; set; }
        public decimal? transferValue { get; set; }
        public decimal? totalInitialSalesIncome { get; set; }
        public decimal? otherCapitalSources { get; set; }
        public bool? isPublicLand { get; set; }
        public decimal? currentLandValue { get; set; }
        public decimal? expectedOnCosts { get; set; }
        public decimal? expectedOnWorks { get; set; }
        public decimal? actualAcquisitionCost { get; set; }
        public decimal? expectedAcquisitionCost { get; set; }
        public decimal? ownResources { get; set; }
        public decimal? howMuchReceivedFromCountyCouncil { get; set; }
        public decimal? howMuchReceivedFromDhscExtraCareFunding { get; set; }
        public decimal? howMuchReceivedFromLocalAuthority1 { get; set; }
        public decimal? howMuchReceivedFromLocalAuthority2 { get; set; }
        public decimal? howMuchReceivedFromSocialServices { get; set; }
        public decimal? howMuchReceivedFromDepartmentOfHealth { get; set; }
        public decimal? howMuchReceivedFromLotteryFunding { get; set; }
        public decimal? howMuchReceivedFromOtherPublicBodies { get; set; }

        //schema
        public decimal? fundingRequested { get; set; }

        public int? noOfHomes { get; set; }
        public string affordabilityEvidence { get; set; }
        public string discussionsWithLocalStakeholders { get; set; }
        public string meetingLocalHousingNeed { get; set; }
        public string meetingLocalProrities { get; set; }
        public string reducingEnvironmentalImpact { get; set; }
        public string sharedOwnershipSalesRisk { get; set; }

        //other
        public string contactId { get; set; }

        public string contactExternalId { get; set; }
        public string organisationId { get; set; }
        public DateTime? lastExternalModificationOn { get; set; }
        public ContactDto lastExternalModificationBy { get; set; }
        public ContactDto lastExternalSubmittedBy { get; set; }
        public bool? representationsandwarranties { get; set; }

        public string developingPartnerId { get; set; }
        public string developingPartnerName { get; set; }
        public string ownerOfTheLandDuringDevelopmentId { get; set; }
        public string ownerOfTheLandDuringDevelopmentName { get; set; }
        public string ownerOfTheHomesAfterCompletionId { get; set; }
        public string ownerOfTheHomesAfterCompletionName { get; set; }
        public bool? applicationPartnerConfirmation { get; set; }

        public bool? isAllocation { get; set; }
    }
}
