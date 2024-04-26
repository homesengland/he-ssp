namespace HE.Investment.AHP.WWW.Models.FinancialDetails;
public class FinancialDetailsContributionsModel : FinancialDetailsBaseModel
{
    public FinancialDetailsContributionsModel()
    {
    }

    public FinancialDetailsContributionsModel(
        Guid applicationId,
        string applicationName,
        string? rentalIncomeBorrowing,
        string? saleOfHomesOnThisScheme,
        string? saleOfHomesOnOtherSchemes,
        string? ownResources,
        string? rcgfContribution,
        string? otherCapitalSources,
        string? sharedOwnershipSales,
        string? homesTransferValue,
        bool isSharedOwnership,
        bool isUnregisteredBodyAccount,
        string? totalExpectedContributions)
        : base(applicationId, applicationName)
    {
        RentalIncomeBorrowing = rentalIncomeBorrowing;
        SaleOfHomesOnThisScheme = saleOfHomesOnThisScheme;
        SaleOfHomesOnOtherSchemes = saleOfHomesOnOtherSchemes;
        OwnResources = ownResources;
        RcgfContribution = rcgfContribution;
        OtherCapitalSources = otherCapitalSources;
        SharedOwnershipSales = sharedOwnershipSales;
        HomesTransferValue = homesTransferValue;
        IsSharedOwnership = isSharedOwnership;
        IsUnregisteredBodyAccount = isUnregisteredBodyAccount;
        TotalExpectedContributions = totalExpectedContributions;
    }

    public string? RentalIncomeBorrowing { get; set; }

    public string? SaleOfHomesOnThisScheme { get; set; }

    public string? SaleOfHomesOnOtherSchemes { get; set; }

    public string? OwnResources { get; set; }

    public string? RcgfContribution { get; set; }

    public string? OtherCapitalSources { get; set; }

    public string? SharedOwnershipSales { get; set; }

    public string? HomesTransferValue { get; set; }

    public bool IsSharedOwnership { get; set; }

    public bool IsUnregisteredBodyAccount { get; set; }

    public string? TotalExpectedContributions { get; set; }
}
