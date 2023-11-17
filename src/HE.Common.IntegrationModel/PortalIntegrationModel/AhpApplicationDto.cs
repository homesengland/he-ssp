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
    }
}
