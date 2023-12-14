using System.Globalization;
using FluentAssertions;
using HE.Investment.AHP.Domain.HomeTypes.Entities;

namespace HE.Investment.AHP.Domain.Tests.HomeTypes.EntitiesTests.TenureDetailsSegmentEntityTests;

public class CalculateProspectiveRentTests
{
    [Theory]
    [InlineData("10", "8", "80")]
    [InlineData("20", "5", "25")]
    [InlineData("5", "20", "400")]
    [InlineData("1450", "1200", "82.76")]
    [InlineData("980", "1250", "127.55")]
    public void ShouldReturnCalculatedResult_WhenMarketRentAndProspectiveRentAreNumbers(string marketRent, string prospectiveRent, string expectedResult)
    {
        // given && when
        decimal.TryParse(expectedResult, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out var parsedExpectedResult);
        var result = TenureDetailsSegmentEntity.CalculateProspectiveRent(marketRent, prospectiveRent);

        // then
        result.Should().Be(parsedExpectedResult);
    }

    [Theory]
    [InlineData("input", "8")]
    [InlineData("10", "input")]
    [InlineData("10", "")]
    [InlineData("", "10")]
    [InlineData("", "")]
    public void ShouldReturnZero_WhenMarketRentOrProspectiveRentIsNotANumber(string marketRent, string prospectiveRent)
    {
        // given && when
        var expectedResult = 0.00m;
        var result = TenureDetailsSegmentEntity.CalculateProspectiveRent(marketRent, prospectiveRent);

        // then
        result.Should().Be(expectedResult);
    }
}
