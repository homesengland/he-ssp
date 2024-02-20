using FluentAssertions;
using HE.Investment.AHP.Domain.HomeTypes.ValueObjects;
using HE.Investment.AHP.Domain.Tests.HomeTypes.TestDataBuilders;

namespace HE.Investment.AHP.Domain.Tests.HomeTypes.EntitiesTests.TenureDetailsSegmentEntityTests;

public class CalculateExpectedFirstTrancheTests
{
    [Theory]
    [InlineData(0, 0.1, 0)]
    [InlineData(1, 0.1, 0.1)]
    [InlineData(99_999_999, 0.75, 74_999_999.25)]
    public void ShouldReturnCalculatedResult_WhenInputsAreProvided(int marketValue, decimal initialSale, decimal expectedResult)
    {
        // given
        var marketValueVO = new MarketValue(marketValue);
        var initialSaleVO = new InitialSale(initialSale);

        var tenureDetails = new TenureDetailsTestDataBuilder()
            .WithMarketValue(marketValueVO)
            .WithInitialSale(initialSaleVO)
            .Build();

        // when
        tenureDetails.ChangeExpectedFirstTranche();

        // then
        tenureDetails.ExpectedFirstTranche.Should().NotBeNull();
        tenureDetails.ExpectedFirstTranche!.Value.Should().Be(expectedResult);
    }

    [Theory]
    [InlineData(null, "10")]
    [InlineData("10", null)]
    public void ShouldReturnNull_WhenAnyValueIsNotProvided(string? marketValue, string? initialSale)
    {
        // given
        var tenureDetails = new TenureDetailsTestDataBuilder().Build();

        // when
        var result = tenureDetails.CalculateExpectedFirstTranche(
            marketValue == null ? null : new MarketValue(marketValue, true),
            initialSale == null ? null : new InitialSale(initialSale, true));

        // then
        result.Should().BeNull();
    }
}
