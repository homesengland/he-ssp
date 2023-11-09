namespace HE.Investment.AHP.Contract.FinancialDetails.Models;
public class FinancialDetailsModel
{
    public FinancialDetailsModel()
    {
    }

    public Guid ApplicationId { get; set; }

    public string? ApplicationName { get; set; }

    public bool? IsPurchasePriceKnown { get; set; }

    public string? PurchasePrice { get; set; }

    public string? IsSchemaOnPublicLand { get; set; }

    public string? LandValue { get; set; }

    public string? ExpectedWorkCost { get; set; }

    public string? ExpectedOnCost { get; set; }

    public string? RentalIncomeContribution { get; set; }

    public string? SubsidyFromSaleOnThisScheme { get; set; }

    public string? SubsidyFromSaleOnOtherSchemes { get; set; }

    public string? OwnResourcesContribution { get; set; }

    public string? RecycledCapitalGarntFundContribution { get; set; }

    public string? OtherCapitalContributions { get; set; }

    public string? InitialSalesReceiptContribution { get; set; }

    public string? TransferValueOfHomes { get; set; }

    public string? CountyCouncilGrants { get; set; }

    public string? DHSCExtraCareGrants { get; set; }

    public string? LocalAuthorityGrants { get; set; }

    public string? SocialServicesGrants { get; set; }

    public string? HealthRelatedGarnts { get; set; }

    public string? LotteryFunding { get; set; }

    public string? OtherPublicGrants { get; set; }
}
