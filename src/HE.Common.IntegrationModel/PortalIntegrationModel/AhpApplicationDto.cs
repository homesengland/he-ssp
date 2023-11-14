namespace HE.Common.IntegrationModel.PortalIntegrationModel
{
    public class AhpApplicationDto
    {
        public string id { get; set; }
        public string name { get; set; }
        public int? tenure { get; set; }
        public int? schemeInformationSectionCompletionStatus { get; set; }
        public int? homeTypesSectionCompletionStatus { get; set; }
        public int? financialDetailsSectionCompletionStatus { get; set; }
        public int? deliveryPhasesSectionCompletionStatus { get; set; }
        public decimal? borrowingAgainstRentalIncomeFromThisScheme { get; set; }
        public decimal? fundingFromOpenMarketHomesOnThisScheme { get; set; }
        public decimal? fundingFromOpenMarketHomesNotOnThisScheme { get; set; }
        public decimal? fundingGeneratedFromOtherSources { get; set; }
        public decimal? recycledCapitalGrantFund { get; set; }
        public decimal? transferValue { get; set; }
        public decimal? totalInitialSalesIncome { get; set; }
        public decimal? otherCapitalSources { get; set; }
        public string contactId { get; set; }
        public string organisationId { get; set; }
    }
}
