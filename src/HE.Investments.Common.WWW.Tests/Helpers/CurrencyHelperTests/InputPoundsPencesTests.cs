using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using HE.Investments.Common.WWW.Helpers;

namespace HE.Investments.Common.WWW.Tests.Helpers.CurrencyHelperTests;

public class InputPoundsPencesTests
{
    public static IEnumerable<object[]> MoneyData()
    {
        yield return new object[] { 0m, "0" };
        yield return new object[] { 100.00m, "100" };
        yield return new object[] { 100.1m, "100.10" };
        yield return new object[] { 123456789.1234m, "123456789.12" };
    }

    [Fact]
    public void ShouldReturnNull_WhenValueIsNull()
    {
        // given & when
        var result = CurrencyHelper.InputPoundsPences(null);

        // then
        result.Should().BeNull();
    }

    [Theory]
    [SuppressMessage("Performance", "CA1825:Avoid zero-length array allocations", Justification = "False positive returned by analyzer")]
    [MemberData(nameof(MoneyData))]
    public void ShouldReturnValueWithoutPoundsSymbolAndThousandsSeparator(decimal value, string expectedValue)
    {
        // given & when
        var result = CurrencyHelper.InputPoundsPences(value);

        // then
        result.Should().Be(expectedValue);
    }
}
