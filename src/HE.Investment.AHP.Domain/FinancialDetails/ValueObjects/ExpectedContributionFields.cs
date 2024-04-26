using System.ComponentModel;

namespace HE.Investment.AHP.Domain.FinancialDetails.ValueObjects;

public enum ExpectedContributionFields
{
    Undefined,

    [Description("borrowing against the rental income for this scheme contribution value")]
    RentalIncomeBorrowing,

    [Description("cross subsidy from the sale of open market homes on this scheme contribution value")]
    SaleOfHomesOnThisScheme,

    [Description("cross subsidy from the sale of open market homes on other schemes contribution value")]
    SaleOfHomesOnOtherSchemes,

    [Description("own resources contribution value")]
    OwnResources,

    [Description("Recycled Capital Grant Fund contribution value")]
    RcgfContribution,

    [Description("other capital sources contribution value")]
    OtherCapitalSources,

    [Description("initial sales receipts from Shared Ownership homes contribution value")]
    SharedOwnershipSales,

    [Description("transfer value of the homes on this scheme contribution value")]
    HomesTransferValue,
}
