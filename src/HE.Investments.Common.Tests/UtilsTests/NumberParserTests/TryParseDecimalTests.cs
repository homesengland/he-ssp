using FluentAssertions;
using HE.Investments.Common.Utils;
using Xunit;

namespace HE.Investments.Common.Tests.UtilsTests.NumberParserTests;

public class TryParseDecimalTests
{
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("    ")]
    [InlineData("  \t\t \r\n ")]
    public void ShouldReturnValueMissingResult_WhenValueIsNotProvided(string? value)
    {
        // given & when
        var result = NumberParser.TryParseDecimal(value, int.MinValue, int.MaxValue, 2, out _);

        // then
        result.Should().Be(NumberParseResult.ValueMissing);
    }

    [Theory]
    [InlineData("+")]
    [InlineData("-")]
    [InlineData("a")]
    [InlineData(" --1 ")]
    [InlineData("++1")]
    [InlineData("1 0")]
    [InlineData("+1a")]
    [InlineData("12,2")]
    [InlineData("12. 12")]
    public void ShouldReturnValueNotANumberResult_WhenValueIsNotValidDecimal(string value)
    {
        // given & when
        var result = NumberParser.TryParseDecimal(value, int.MinValue, int.MaxValue, 2, out _);

        // then
        result.Should().Be(NumberParseResult.ValueNotANumber);
    }

    [Theory]
    [InlineData("12.12", 1)]
    [InlineData("0.0000001", 6)]
    public void ShouldReturnVValueInvalidPrecisionResult_WhenValueHasInvalidDecimalPrecision(string value, int precision)
    {
        // given & when
        var result = NumberParser.TryParseDecimal(value, int.MinValue, int.MaxValue, precision, out _);

        // then
        result.Should().Be(NumberParseResult.ValueInvalidPrecision);
    }

    [Theory]
    [InlineData("   \t -11 \t")]
    [InlineData("-11")]
    [InlineData("-10000000")]
    [InlineData("-100000000000000000000000000000000000000000000000000000000000000")]
    [InlineData("-10.0001")]
    public void ShouldReturnValueTooLowResult_WhenValueIsBelowAllowedMinimum(string value)
    {
        // given & when
        var result = NumberParser.TryParseDecimal(value, -10, 1000, 4, out _);

        // then
        result.Should().Be(NumberParseResult.ValueTooLow);
    }

    [Theory]
    [InlineData("   \t +11 \t")]
    [InlineData("+11")]
    [InlineData("11")]
    [InlineData("10000000")]
    [InlineData("100000000000000000000000000000000000000000000000000000000000000")]
    [InlineData("10.0001")]
    public void ShouldReturnValueTooHighResult_WhenValueIsBelowAllowedMinimum(string value)
    {
        // given & when
        var result = NumberParser.TryParseDecimal(value, 0, 10, 4, out _);

        // then
        result.Should().Be(NumberParseResult.ValueTooHigh);
    }

    [Theory]
    [InlineData("-10", 2, -10)]
    [InlineData("  \t -10.00\t", 2, -10)]
    [InlineData("+10.01", 2, 10.01)]
    [InlineData("  \t +10.1234 \t", 4, 10.1234)]
    [InlineData("00000000000000000000000000000000000010.9", 1, 10.9)]
    [InlineData(" 10 ", 0, 10)]
    public void ShouldReturnSuccessfullyParsedResult_WhenValueWithinGivenRange(string value, int precision, decimal expectedResult)
    {
        // given & when
        var result = NumberParser.TryParseDecimal(value, -11, 11, precision, out var parsedValue);

        // then
        result.Should().Be(NumberParseResult.SuccessfullyParsed);
        parsedValue.Should().Be(expectedResult);
    }
}
