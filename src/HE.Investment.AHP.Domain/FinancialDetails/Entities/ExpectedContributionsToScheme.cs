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
        Tenure tenure,
        bool isUnregisteredBody)
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
        IsUnregisteredBody = isUnregisteredBody;
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

    public bool IsUnregisteredBody { get; }

    public bool IsAnswered()
    {
        var isAnswered = RentalIncome.IsProvided() && SalesOfHomesOnThisScheme.IsProvided() && SalesOfHomesOnOtherSchemes.IsProvided() &&
                         OwnResources.IsProvided() && RcgfContributions.IsProvided() && OtherCapitalSources.IsProvided();

        isAnswered = CheckUnregisteredBody(isAnswered, true);

        return isAnswered && IsAnsweredForSharedOwnershipHomes();
    }

    public bool AreAllNotAnswered()
    {
        var isNotAnswered = RentalIncome.IsNotProvided() && SalesOfHomesOnThisScheme.IsNotProvided() && SalesOfHomesOnOtherSchemes.IsNotProvided() &&
                         OwnResources.IsNotProvided() && RcgfContributions.IsNotProvided() && OtherCapitalSources.IsNotProvided();

        isNotAnswered = CheckUnregisteredBody(isNotAnswered, false);

        return isNotAnswered || !IsAnsweredForSharedOwnershipHomes();
    }

    public decimal? CalculateTotal()
    {
        if (AreAllNotAnswered())
        {
            return null;
        }

        var totalExpectedContributions = RentalIncome.GetValueOrZero() + SalesOfHomesOnThisScheme.GetValueOrZero() +
                                         SalesOfHomesOnOtherSchemes.GetValueOrZero() + OwnResources.GetValueOrZero() + RcgfContributions.GetValueOrZero() +
                                         OtherCapitalSources.GetValueOrZero();

        if (IsUnregisteredBody)
        {
            totalExpectedContributions += HomesTransferValue.GetValueOrZero();
        }

        if (SharedOwnershipHomes().Contains(ApplicationTenure))
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

    private bool IsAnsweredForSharedOwnershipHomes()
    {
        if (SharedOwnershipHomes().Contains(ApplicationTenure))
        {
            return SharedOwnershipSales.IsProvided();
        }

        return true;
    }

    private bool CheckUnregisteredBody(bool currentStatus, bool expectedStatus)
    {
        if (IsUnregisteredBody)
        {
            return currentStatus && HomesTransferValue.IsProvided() == expectedStatus;
        }

        return currentStatus;
    }

    private IEnumerable<Tenure> SharedOwnershipHomes()
    {
        yield return Tenure.SharedOwnership;
        yield return Tenure.HomeOwnershipLongTermDisabilities;
        yield return Tenure.OlderPersonsSharedOwnership;
    }
}
