using System.Globalization;
using FluentAssertions;
using HE.Investment.AHP.Domain.HomeTypes.Entities;

namespace HE.Investment.AHP.Domain.Tests.HomeTypes.EntitiesTests.TenureDetailsSegmentEntityTests;

public class CalculateProspectiveRentAsPercentageOfTheUnsoldShareTests
{
    [Theory]
    [InlineData("1000", "800", "10", "46.22")]
    [InlineData("2000", "20", "35", "0.8")]
    [InlineData("511", "90", "12", "10.41")]
    [InlineData("14500", "945", "60", "8.47")]
    [InlineData("9800", "5000", "75", "106.12")]
    public void ShouldReturnCalculatedResult_WhenProvidedInputsAreNumber(string? marketValue, string? prospectiveRent, string? initialSale, string expectedResult)
    {
        // given && when
        decimal.TryParse(expectedResult, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out var parsedExpectedResult);
        var result = TenureDetailsSegmentEntity.CalculateProspectiveRentAsPercentageOfTheUnsoldShare(marketValue, prospectiveRent, initialSale);

        // then
        result.Should().Be(parsedExpectedResult);
    }

    [Theory]
    [InlineData("input", "8", "10")]
    [InlineData("10", "input", "15")]
    [InlineData("", "10", "")]
    [InlineData("", "", "")]
    public void ShouldReturnZero_WhenProvidedInputIsNotANumber(string? marketValue, string? prospectiveRent, string? initialSale)
    {
        // given && when
        var expectedResult = 0.00m;
        var result = TenureDetailsSegmentEntity.CalculateProspectiveRentAsPercentageOfTheUnsoldShare(marketValue, prospectiveRent, initialSale);

        // then
        result.Should().Be(expectedResult);
    }
}
