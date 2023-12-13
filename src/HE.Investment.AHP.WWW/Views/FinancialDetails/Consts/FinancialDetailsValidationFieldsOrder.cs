using HE.Investment.AHP.Domain.FinancialDetails.Constants;

namespace HE.Investment.AHP.WWW.Views.FinancialDetails.Consts;
public static class FinancialDetailsValidationFieldsOrder
{
    public static List<string> LandStatus => new() { FinancialDetailsValidationFieldNames.PurchasePrice };

    public static List<string> LandValue => new() { FinancialDetailsValidationFieldNames.LandValue };

    public static List<string> OtherSchemeCosts => new()
    {
        FinancialDetailsValidationFieldNames.ExpectedWorksCosts,
        FinancialDetailsValidationFieldNames.ExpectedOnCosts,
    };

    public static List<string> ExpectedContributions => new()
    {
        FinancialDetailsValidationFieldNames.RentalIncomeBorrowing,
        FinancialDetailsValidationFieldNames.SaleOfHomesOnThisScheme,
        FinancialDetailsValidationFieldNames.SaleOfHomesOnOtherSchemes,
        FinancialDetailsValidationFieldNames.OwnResources,
        FinancialDetailsValidationFieldNames.RcgfContribution,
        FinancialDetailsValidationFieldNames.OtherCapitalSources,
        FinancialDetailsValidationFieldNames.SharedOwnershipSales,
        FinancialDetailsValidationFieldNames.HomesTransferValue,
    };

    public static List<string> Grants => new()
    {
        FinancialDetailsValidationFieldNames.CountyCouncilGrants,
        FinancialDetailsValidationFieldNames.DHSCExtraCareGrants,
        FinancialDetailsValidationFieldNames.LocalAuthorityGrants,
        FinancialDetailsValidationFieldNames.SocialServicesGrants,
        FinancialDetailsValidationFieldNames.HeatlthRelatedGrants,
        FinancialDetailsValidationFieldNames.LotteryGrants,
        FinancialDetailsValidationFieldNames.OtherPublicBodiesGrants,
    };
}
