using System.ComponentModel;

namespace HE.Investment.AHP.Domain.FinancialDetails.ValueObjects;

public enum ExpectedContributionFields
{
    Undefined,

    [Description("amount you will contribute from borrowing against the rental income for this scheme")]
    RentalIncomeBorrowing,

    [Description("amount you will contribute from the cross subsidy from the sale of open market homes on this scheme")]
    SaleOfHomesOnThisScheme,

    [Description("amount you will contribute from cross subsidy from the sale of open market homes on other schemes")]
    SaleOfHomesOnOtherSchemes,

    [Description("amount you intend to contribute from your own resources")]
    OwnResources,

    [Description("amount you intend to contribute from your Recycled Capital Grant Fund")]
    RcgfContribution,

    [Description("amount you will contribute from other capital sources")]
    OtherCapitalSources,

    [Description("amount you will contribute from initial sales receipts from Shared Ownership homes")]
    SharedOwnershipSales,

    [Description("transfer value of the homes on this scheme")]
    HomesTransferValue,
}
