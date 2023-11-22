using System;

namespace HE.Common.IntegrationModel.PortalIntegrationModel
{
    public class AhpApplicationDto
    {
        public string id { get; set; }
        public string name { get; set; }
        public int? tenure { get; set; }

        //sections statuses
        public int? schemeInformationSectionCompletionStatus { get; set; }
        public int? homeTypesSectionCompletionStatus { get; set; }
        public int? financialDetailsSectionCompletionStatus { get; set; }
        public int? deliveryPhasesSectionCompletionStatus { get; set; }

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
        public string organisationId { get; set; }
        public DateTime? lastExternalModificationOn { get; set; }
        public ContactDto lastExternalModificationBy { get; set; }
    }
}
