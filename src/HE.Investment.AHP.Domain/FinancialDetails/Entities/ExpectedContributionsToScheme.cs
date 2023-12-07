using HE.Investment.AHP.Domain.FinancialDetails.ValueObjects;
using HE.Investments.Common.Extensions;

namespace HE.Investment.AHP.Domain.FinancialDetails.Entities;

public class ExpectedContributionsToScheme
{
    public ExpectedContributionsToScheme(
        ExpectedContribution? rentalIncome,
        ExpectedContribution? salesOfHomesOnThisScheme,
        ExpectedContribution? salesOfHomesOnOtherSchemes,
        ExpectedContribution? ownResources,
        ExpectedContribution? rcgfContributions,
        ExpectedContribution? otherCapitalSources,
        ExpectedContribution? sharedOwnershipSales,
        ExpectedContribution? homesTransferValue)
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

    public ExpectedContribution? RentalIncome { get; private set; }

    public ExpectedContribution? SalesOfHomesOnThisScheme { get; private set; }

    public ExpectedContribution? SalesOfHomesOnOtherSchemes { get; private set; }

    public ExpectedContribution? OwnResources { get; private set; }

    public ExpectedContribution? RcgfContributions { get; private set; }

    public ExpectedContribution? OtherCapitalSources { get; private set; }

    public ExpectedContribution? SharedOwnershipSales { get; private set; }

    public ExpectedContribution? HomesTransferValue { get; private set; }

    public decimal CalculateTotal()
    {
        var totalExpectedContributions = RentalIncome.GetValueOrZero() + SalesOfHomesOnThisScheme.GetValueOrZero() +
                                         SalesOfHomesOnOtherSchemes.GetValueOrZero() + OwnResources.GetValueOrZero() + RcgfContributions.GetValueOrZero() +
                                         OtherCapitalSources.GetValueOrZero() + SharedOwnershipSales.GetValueOrZero() + HomesTransferValue.GetValueOrZero();

        return totalExpectedContributions;
    }
}
