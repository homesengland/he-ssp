using HE.Investment.AHP.Domain.FinancialDetails.ValueObjects;
using HE.Investments.Common.Extensions;

namespace HE.Investment.AHP.Domain.FinancialDetails.Entities;

public class ExpectedContributionsToScheme
{
    public ExpectedContributionsToScheme(
        ExpectedContributionValue? rentalIncome,
        ExpectedContributionValue? salesOfHomesOnThisScheme,
        ExpectedContributionValue? salesOfHomesOnOtherSchemes,
        ExpectedContributionValue? ownResources,
        ExpectedContributionValue? rcgfContributions,
        ExpectedContributionValue? otherCapitalSources,
        ExpectedContributionValue? sharedOwnershipSales,
        ExpectedContributionValue? homesTransferValue)
    {
        RentalIncome = rentalIncome;
        SalesOfHomesOnThisScheme = salesOfHomesOnThisScheme;
        SalesOfHomesOnOtherSchemes = salesOfHomesOnOtherSchemes;
        OwnResources = ownResources;
        RcgfContributions = rcgfContributions;
        OtherCapitalSources = otherCapitalSources;
        SharedOwnershipSales = sharedOwnershipSales;
        HomesTransferValue = homesTransferValue;
    }

    public ExpectedContributionValue? RentalIncome { get; private set; }

    public ExpectedContributionValue? SalesOfHomesOnThisScheme { get; private set; }

    public ExpectedContributionValue? SalesOfHomesOnOtherSchemes { get; private set; }

    public ExpectedContributionValue? OwnResources { get; private set; }

    public ExpectedContributionValue? RcgfContributions { get; private set; }

    public ExpectedContributionValue? OtherCapitalSources { get; private set; }

    public ExpectedContributionValue? SharedOwnershipSales { get; private set; }

    public ExpectedContributionValue? HomesTransferValue { get; private set; }

    public decimal CalculateTotal()
    {
        var totalExpectedContributions = RentalIncome.GetValueOrZero() + SalesOfHomesOnThisScheme.GetValueOrZero() +
                                         SalesOfHomesOnOtherSchemes.GetValueOrZero() + OwnResources.GetValueOrZero() + RcgfContributions.GetValueOrZero() +
                                         OtherCapitalSources.GetValueOrZero() + SharedOwnershipSales.GetValueOrZero() + HomesTransferValue.GetValueOrZero();

        return totalExpectedContributions;
    }
}
