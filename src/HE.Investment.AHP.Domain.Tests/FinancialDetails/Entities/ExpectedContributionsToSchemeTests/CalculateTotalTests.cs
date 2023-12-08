using FluentAssertions;
using HE.Investment.AHP.Domain.FinancialDetails.Entities;
using HE.Investment.AHP.Domain.FinancialDetails.ValueObjects;

namespace HE.Investment.AHP.Domain.Tests.FinancialDetails.Entities.ExpectedContributionsToSchemeTests;

public class CalculateTotalTests
{
    [Fact]
    public void ShouldCalculateTotal_WhenAllValuesAreSet()
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
            new ExpectedContributionValue(ExpectedContributionFields.HomesTransferValue, "8"));

        // when
        var total = expectedContributionsToScheme.CalculateTotal();

        // then
        total.Should().Be(36);
    }

    [Fact]
    public void ShouldCalculateTotal_WhenSomeValuesAreNotSet()
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
            null);

        // when
        var total = expectedContributionsToScheme.CalculateTotal();

        // then
        total.Should().Be(28);
    }

    [Fact]
    public void ShouldCalculateTotal_WhenAllValuesAreNotSet()
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
            null);

        // when
        var total = expectedContributionsToScheme.CalculateTotal();

        // then
        total.Should().Be(0);
    }
}
