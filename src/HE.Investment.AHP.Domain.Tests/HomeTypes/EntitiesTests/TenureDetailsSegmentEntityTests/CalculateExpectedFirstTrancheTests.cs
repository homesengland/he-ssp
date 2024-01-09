using FluentAssertions;
using HE.Investment.AHP.Domain.HomeTypes.Entities;
using HE.Investment.AHP.Domain.HomeTypes.ValueObjects;
using HE.Investment.AHP.Domain.Tests.HomeTypes.TestDataBuilders;
using HE.Investments.Common.Contract.Exceptions;

namespace HE.Investment.AHP.Domain.Tests.HomeTypes.EntitiesTests.TenureDetailsSegmentEntityTests;

public class CalculateExpectedFirstTrancheTests
{
    [Theory]
    [InlineData(1000, 10, 100)]
    [InlineData(200, 23, 46)]
    [InlineData(511, 46, 235.06)]
    [InlineData(14500, 60, 8700)]
    [InlineData(9800, 75, 7350)]
    public void ShouldReturnCalculatedResult_WhenMarketValueAndInitialSaleAreNumbers(int marketValue, int initialSale, decimal expectedResult)
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
    [InlineData("input", "8")]
    [InlineData("10", "input")]
    [InlineData("10", "")]
    [InlineData("", "10")]
    [InlineData("", "")]
    public void ShouldReturnZero_WhenMarketValueOrInitialSaleIsNotANumber(string marketValue, string initialSale)
    {
        // given
        var tenureDetails = new TenureDetailsTestDataBuilder().Build();

        // when
        Action action = () => _ = tenureDetails.CalculateExpectedFirstTranche(
            new MarketValue(marketValue, true),
            new InitialSale(initialSale, true));

        // then
        action.Should().Throw<DomainValidationException>();
    }
}
