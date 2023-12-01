namespace HE.Investment.AHP.Contract.FinancialDetails;
public class FinancialDetails
{
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

    public decimal TotalExpectedContributions
    {
        get
        {
            decimal result = 0;
            result += RentalIncomeContribution ?? 0
                + SubsidyFromSaleOnThisScheme ?? 0
                + SubsidyFromSaleOnOtherSchemes ?? 0
                + OwnResourcesContribution ?? 0
                + RecycledCapitalGarntFundContribution ?? 0
                + OtherCapitalContributions ?? 0
                + SharedOwnershipSalesContribution ?? 0
                + TransferValueOfHomes ?? 0;

            return result;
        }
    }

    public decimal TotalRecievedGrands
    {
        get
        {
            decimal result = 0;
            result += CountyCouncilGrants ?? 0
                   + DHSCExtraCareGrants ?? 0
                   + LocalAuthorityGrants ?? 0
                   + SocialServicesGrants ?? 0
                   + HealthRelatedGrants ?? 0
                   + LotteryFunding ?? 0
                   + OtherPublicGrants ?? 0;

            return result;
        }
    }
}
