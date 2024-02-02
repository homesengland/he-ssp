using FluentAssertions;
using HE.Investment.AHP.Contract.Application;
using HE.Investment.AHP.Domain.FinancialDetails.Entities;
using HE.Investment.AHP.Domain.FinancialDetails.ValueObjects;

namespace HE.Investment.AHP.Domain.Tests.FinancialDetails.Entities.ExpectedContributionsToSchemeTests;

public class CalculateTotalTests
{
    [Theory]
    [InlineData(Tenure.SharedOwnership)]
    [InlineData(Tenure.HomeOwnershipLongTermDisabilities)]
    [InlineData(Tenure.OlderPersonsSharedOwnership)]
    public void ShouldCalculateTotal_WhenAllValuesAreSetAndTenureIsOneOfTheSharedOwnership(Tenure tenure)
    {
        // given
        var expectedContributionsToScheme = new ExpectedContributionsToScheme(
            new ExpectedContributionValue(ExpectedContributionFields.RentalIncomeBorrowing, "1"),
            new ExpectedContributionValue(ExpectedContributionFields.SaleOfHomesOnThisScheme, "2"),
            new ExpectedContributionValue(ExpectedContributionFields.SaleOfHomesOnOtherSchemes, "3"),
            new ExpectedContributionValue(ExpectedContributionFields.OwnResources, "4"),
            new ExpectedContributionValue(ExpectedContributionFields.RcgfContribution, "5"),
            new ExpectedContributionValue(ExpectedContributionFields.OtherCapitalSources, "6"),
            new ExpectedContributionValue(ExpectedContributionFields.SharedOwnershipSales, "7"),
            new ExpectedContributionValue(ExpectedContributionFields.HomesTransferValue, "8"),
            tenure);

        // when
        var total = expectedContributionsToScheme.CalculateTotal();

        // then
        total.Should().Be(36);
    }

    [Theory]
    [InlineData(Tenure.SharedOwnership)]
    [InlineData(Tenure.HomeOwnershipLongTermDisabilities)]
    [InlineData(Tenure.OlderPersonsSharedOwnership)]
    public void ShouldCalculateTotal_WhenSomeValuesAreNotSetAndTenureIsOneOfTheSharedOwnership(Tenure tenure)
    {
        // given
        var expectedContributionsToScheme = new ExpectedContributionsToScheme(
            new ExpectedContributionValue(ExpectedContributionFields.RentalIncomeBorrowing, "1"),
            new ExpectedContributionValue(ExpectedContributionFields.SaleOfHomesOnThisScheme, "2"),
            new ExpectedContributionValue(ExpectedContributionFields.SaleOfHomesOnOtherSchemes, "3"),
            new ExpectedContributionValue(ExpectedContributionFields.OwnResources, "4"),
            new ExpectedContributionValue(ExpectedContributionFields.RcgfContribution, "5"),
            new ExpectedContributionValue(ExpectedContributionFields.OtherCapitalSources, "6"),
            new ExpectedContributionValue(ExpectedContributionFields.SharedOwnershipSales, "7"),
            null,
            tenure);

        // when
        var total = expectedContributionsToScheme.CalculateTotal();

        // then
        total.Should().Be(28);
    }

    [Theory]
    [InlineData(Tenure.AffordableRent)]
    [InlineData(Tenure.SocialRent)]
    [InlineData(Tenure.RentToBuy)]
    public void ShouldCalculateTotal_WhenAllValuesAreSetAndTenureIsNotOneOfTheSharedOwnership(Tenure tenure)
    {
        // given
        var expectedContributionsToScheme = new ExpectedContributionsToScheme(
            new ExpectedContributionValue(ExpectedContributionFields.RentalIncomeBorrowing, "1"),
            new ExpectedContributionValue(ExpectedContributionFields.SaleOfHomesOnThisScheme, "2"),
            new ExpectedContributionValue(ExpectedContributionFields.SaleOfHomesOnOtherSchemes, "3"),
            new ExpectedContributionValue(ExpectedContributionFields.OwnResources, "4"),
            new ExpectedContributionValue(ExpectedContributionFields.RcgfContribution, "5"),
            new ExpectedContributionValue(ExpectedContributionFields.OtherCapitalSources, "6"),
            new ExpectedContributionValue(ExpectedContributionFields.SharedOwnershipSales, "7"),
            new ExpectedContributionValue(ExpectedContributionFields.HomesTransferValue, "8"),
            tenure);

        // when
        var total = expectedContributionsToScheme.CalculateTotal();

        // then
        total.Should().Be(29);
    }

    [Theory]
    [InlineData(Tenure.AffordableRent)]
    [InlineData(Tenure.SocialRent)]
    [InlineData(Tenure.RentToBuy)]
    public void ShouldCalculateTotal_WhenSomeValuesAreNotSetAndTenureIsNotOneOfTheSharedOwnership(Tenure tenure)
    {
        // given
        var expectedContributionsToScheme = new ExpectedContributionsToScheme(
            new ExpectedContributionValue(ExpectedContributionFields.RentalIncomeBorrowing, "1"),
            new ExpectedContributionValue(ExpectedContributionFields.SaleOfHomesOnThisScheme, "2"),
            new ExpectedContributionValue(ExpectedContributionFields.SaleOfHomesOnOtherSchemes, "3"),
            new ExpectedContributionValue(ExpectedContributionFields.OwnResources, "4"),
            new ExpectedContributionValue(ExpectedContributionFields.RcgfContribution, "5"),
            new ExpectedContributionValue(ExpectedContributionFields.OtherCapitalSources, "6"),
            new ExpectedContributionValue(ExpectedContributionFields.SharedOwnershipSales, "7"),
            null,
            tenure);

        // when
        var total = expectedContributionsToScheme.CalculateTotal();

        // then
        total.Should().Be(21);
    }

    [Theory]
    [InlineData(Tenure.AffordableRent)]
    [InlineData(Tenure.SocialRent)]
    [InlineData(Tenure.SharedOwnership)]
    [InlineData(Tenure.RentToBuy)]
    [InlineData(Tenure.HomeOwnershipLongTermDisabilities)]
    [InlineData(Tenure.OlderPersonsSharedOwnership)]
    public void ShouldCalculateTotal_WhenAllValuesAreNotSet(Tenure tenure)
    {
        // given
        var expectedContributionsToScheme = new ExpectedContributionsToScheme(
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            tenure);

        // when
        var total = expectedContributionsToScheme.CalculateTotal();

        // then
        total.Should().Be(0);
    }
}
