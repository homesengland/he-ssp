using FluentAssertions;
using HE.Investment.AHP.Domain.Tests.FinancialDetails.TestObjectBuilders;

namespace HE.Investment.AHP.Domain.Tests.FinancialDetails.Entities.FinancialDetailsEntityTests;

public class ExpectedTotalCostsTests
{
    [Fact]
    public void ShouldReturnNull_WhenAllCostsAreNotProvided()
    {
        // given
        var financialDetailsEntity = FinancialDetailsEntityBuilder
            .New()
            .Build();

        // when
        var expectedTotalCosts = financialDetailsEntity.ExpectedTotalCosts();

        // then
        expectedTotalCosts.Should().BeNull();
    }

    [Theory]
    [InlineData(null, "100", 100)]
    [InlineData("5", null, 5)]
    [InlineData("1", "100", 101)]
    public void ShouldReturnExpectedTotalCosts(string expectedWorksCosts, string expectedOnCosts, int expectedTotalCost)
    {
        // given
        var financialDetailsEntity = FinancialDetailsEntityBuilder
            .New()
            .WithOtherApplicationCosts(expectedWorksCosts, expectedOnCosts)
            .Build();

        // when
        var expectedTotalCosts = financialDetailsEntity.ExpectedTotalCosts();

        // then
        expectedTotalCosts.Should().Be(expectedTotalCost);
    }

    [Fact]
    public void ShouldReturnExpectedTotalCosts_WhenOtherApplicationCostsAreProvidedAndOtherCostsAreCurrentLandValue()
    {
        // given
        var financialDetailsEntity = FinancialDetailsEntityBuilder
            .New()
            .WithOtherApplicationCosts("123", null)
            .WithLandValue("2")
            .Build();

        // when
        var expectedTotalCosts = financialDetailsEntity.ExpectedTotalCosts();

        // then
        expectedTotalCosts.Should().Be(125);
    }
}
