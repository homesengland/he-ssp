using HE.Investment.AHP.Contract.Application;
using HE.Investment.AHP.Domain.FinancialDetails.ValueObjects;
using HE.Investments.Common.Domain;
using HE.Investments.Common.Extensions;

namespace HE.Investment.AHP.Domain.FinancialDetails.Entities;

public class ExpectedContributionsToScheme : ValueObject, IQuestion
{
    public ExpectedContributionsToScheme(
        ExpectedContributionValue? rentalIncome,
        ExpectedContributionValue? salesOfHomesOnThisScheme,
        ExpectedContributionValue? salesOfHomesOnOtherSchemes,
        ExpectedContributionValue? ownResources,
        ExpectedContributionValue? rcgfContributions,
        ExpectedContributionValue? otherCapitalSources,
        ExpectedContributionValue? sharedOwnershipSales,
        ExpectedContributionValue? homesTransferValue,
        Tenure tenure)
    {
        RentalIncome = rentalIncome;
        SalesOfHomesOnThisScheme = salesOfHomesOnThisScheme;
        SalesOfHomesOnOtherSchemes = salesOfHomesOnOtherSchemes;
        OwnResources = ownResources;
        RcgfContributions = rcgfContributions;
        OtherCapitalSources = otherCapitalSources;
        SharedOwnershipSales = sharedOwnershipSales;
        HomesTransferValue = homesTransferValue;
        ApplicationTenure = tenure;
    }

    public Tenure ApplicationTenure { get; }

    public ExpectedContributionValue? RentalIncome { get; private set; }

    public ExpectedContributionValue? SalesOfHomesOnThisScheme { get; private set; }

    public ExpectedContributionValue? SalesOfHomesOnOtherSchemes { get; private set; }

    public ExpectedContributionValue? OwnResources { get; private set; }

    public ExpectedContributionValue? RcgfContributions { get; private set; }

    public ExpectedContributionValue? OtherCapitalSources { get; private set; }

    public ExpectedContributionValue? SharedOwnershipSales { get; private set; }

    public ExpectedContributionValue? HomesTransferValue { get; private set; }

    public bool IsAnswered()
    {
        var isAnswered = RentalIncome.IsProvided() && SalesOfHomesOnThisScheme.IsProvided() && SalesOfHomesOnOtherSchemes.IsProvided() &&
                         OwnResources.IsProvided() && RcgfContributions.IsProvided() && OtherCapitalSources.IsProvided() && HomesTransferValue.IsProvided();

        return isAnswered && IsAnsweredForTenureSharedOwnership();
    }

    public decimal CalculateTotal()
    {
        var totalExpectedContributions = RentalIncome.GetValueOrZero() + SalesOfHomesOnThisScheme.GetValueOrZero() +
                                         SalesOfHomesOnOtherSchemes.GetValueOrZero() + OwnResources.GetValueOrZero() + RcgfContributions.GetValueOrZero() +
                                         OtherCapitalSources.GetValueOrZero() + SharedOwnershipSales.GetValueOrZero() + HomesTransferValue.GetValueOrZero();

        return totalExpectedContributions;
    }

    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return RentalIncome;
        yield return SalesOfHomesOnThisScheme;
        yield return SalesOfHomesOnOtherSchemes;
        yield return OwnResources;
        yield return RcgfContributions;
        yield return OtherCapitalSources;
        yield return SharedOwnershipSales;
        yield return HomesTransferValue;
    }

    private bool IsAnsweredForTenureSharedOwnership()
    {
        if (ApplicationTenure != Tenure.SharedOwnership)
        {
            return true;
        }

        return SharedOwnershipSales.IsProvided();
    }
}
