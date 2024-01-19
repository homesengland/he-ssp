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

    public ExpectedContributionsToScheme(Tenure tenure)
    {
        ApplicationTenure = tenure;
    }

    public Tenure ApplicationTenure { get; }

    public ExpectedContributionValue? RentalIncome { get; }

    public ExpectedContributionValue? SalesOfHomesOnThisScheme { get; }

    public ExpectedContributionValue? SalesOfHomesOnOtherSchemes { get; }

    public ExpectedContributionValue? OwnResources { get; }

    public ExpectedContributionValue? RcgfContributions { get; }

    public ExpectedContributionValue? OtherCapitalSources { get; }

    public ExpectedContributionValue? SharedOwnershipSales { get; }

    public ExpectedContributionValue? HomesTransferValue { get; }

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
                                         OtherCapitalSources.GetValueOrZero() + HomesTransferValue.GetValueOrZero();

        if (ApplicationTenure == Tenure.SharedOwnership)
        {
            return totalExpectedContributions + SharedOwnershipSales.GetValueOrZero();
        }

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
