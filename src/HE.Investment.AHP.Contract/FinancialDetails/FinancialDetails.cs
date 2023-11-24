namespace HE.Investment.AHP.Contract.FinancialDetails;
public class FinancialDetails
{
    public FinancialDetails()
    {
    }

    public Guid ApplicationId { get; set; }

    public string ApplicationName { get; set; }

    public decimal? ActualPurchasePrice { get; set; }

    public decimal? ExpectedPurchasePrice { get; set; }

    public bool? IsSchemaOnPublicLand { get; set; }

    public decimal? LandValue { get; set; }

    public decimal? ExpectedWorkCost { get; set; }

    public decimal? ExpectedOnCost { get; set; }

    public decimal? RentalIncomeContribution { get; set; }

    public decimal? SubsidyFromSaleOnThisScheme { get; set; }

    public decimal? SubsidyFromSaleOnOtherSchemes { get; set; }

    public decimal? OwnResourcesContribution { get; set; }

    public decimal? RecycledCapitalGarntFundContribution { get; set; }

    public decimal? OtherCapitalContributions { get; set; }

    public decimal? SharedOwnershipSalesContribution { get; set; }

    public decimal? TransferValueOfHomes { get; set; }

    public decimal? CountyCouncilGrants { get; set; }

    public decimal? DHSCExtraCareGrants { get; set; }

    public decimal? LocalAuthorityGrants { get; set; }

    public decimal? SocialServicesGrants { get; set; }

    public decimal? HealthRelatedGrants { get; set; }

    public decimal? LotteryFunding { get; set; }

    public decimal? OtherPublicGrants { get; set; }
}
