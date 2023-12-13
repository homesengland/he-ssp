using System.Globalization;
using FluentAssertions;
using HE.Investment.AHP.Domain.HomeTypes.Entities;

namespace HE.Investment.AHP.Domain.Tests.HomeTypes.EntitiesTests.TenureDetailsSegmentEntityTests;

public class CalculateExpectedFirstTrancheTests
{
    [Theory]
    [InlineData("1000", "10", "100")]
    [InlineData("200", "23", "46")]
    [InlineData("511", "46", "235.06")]
    [InlineData("14500", "60", "8700")]
    [InlineData("9800", "75", "7350")]
    public void ShouldReturnCalculatedResult_WhenMarketValueAndInitialSaleAreNumbers(string marketValue, string initialSale, string expectedResult)
    {
        // given && when
        decimal.TryParse(expectedResult, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out var parsedExpectedResult);
        var result = TenureDetailsSegmentEntity.CalculateExpectedFirstTranche(marketValue, initialSale);

        // then
        result.Should().Be(parsedExpectedResult);
    }

    [Theory]
    [InlineData("input", "8")]
    [InlineData("10", "input")]
    [InlineData("10", "")]
    [InlineData("", "10")]
    [InlineData("", "")]
    public void ShouldReturnZero_WhenMarketValueOrInitialSaleIsNotANumber(string marketValue, string initialSale)
    {
        // given && when
        var expectedResult = 0.00m;
        var result = TenureDetailsSegmentEntity.CalculateExpectedFirstTranche(marketValue, initialSale);

        // then
        result.Should().Be(expectedResult);
    }
}
