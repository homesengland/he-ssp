using FluentAssertions;
using HE.Investment.AHP.Domain.Tests.FinancialDetails.TestObjectBuilders;

namespace HE.Investment.AHP.Domain.Tests.FinancialDetails.Entities.FinancialDetailsEntityTests;

public class ExpectedTotalCostsTests
{
    [Fact]
    public void ShouldReturnZero_WhenAllCostsAreNotProvided()
    {
        // given
        var financialDetailsEntity = FinancialDetailsEntityBuilder
            .New()
            .Build();

        // when
        var expectedTotalCosts = financialDetailsEntity.ExpectedTotalCosts();

        // then
        expectedTotalCosts.Should().Be(0);
    }

    [Theory]
    [InlineData(null, 100.0, 100.0)]
    [InlineData(5.0, null, 5.0)]
    [InlineData(1.0, 100.0, 101.0)]
    public void ShouldReturnExpectedTotalCosts(double? expectedWorksCosts, double? expectedOnCosts, double expectedTotalCost)
    {
        // given
        var financialDetailsEntity = FinancialDetailsEntityBuilder
            .New()
            .WithOtherApplicationCosts((decimal?)expectedWorksCosts, (decimal?)expectedOnCosts)
            .Build();

        // when
        var expectedTotalCosts = financialDetailsEntity.ExpectedTotalCosts();

        // then
        expectedTotalCosts.Should().Be((decimal)expectedTotalCost);
    }

    [Fact]
    public void ShouldReturnExpectedTotalCosts_WhenOtherApplicationCostsAreProvidedAndOtherCostsAreCurrentLandValue()
    {
        // given
        var financialDetailsEntity = FinancialDetailsEntityBuilder
            .New()
            .WithOtherApplicationCosts(123, null)
            .WithLandValue(2)
            .Build();

        // when
        var expectedTotalCosts = financialDetailsEntity.ExpectedTotalCosts();

        // then
        expectedTotalCosts.Should().Be(125);
    }
}
