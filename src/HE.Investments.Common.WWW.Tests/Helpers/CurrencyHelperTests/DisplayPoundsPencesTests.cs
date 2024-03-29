using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using HE.Investments.Common.WWW.Helpers;

namespace HE.Investments.Common.WWW.Tests.Helpers.CurrencyHelperTests;

public class DisplayPoundsPencesTests
{
    public static IEnumerable<object[]> MoneyData()
    {
        yield return new object[] { 0m, "£0" };
        yield return new object[] { 0.1m, "£0.10" };
        yield return new object[] { 100m, "£100" };
        yield return new object[] { 100.991m, "£100.99" };
        yield return new object[] { 123456789.00m, "£123,456,789" };
        yield return new object[] { 123456789.123m, "£123,456,789.12" };
    }

    [Fact]
    public void ShouldReturnNull_WhenValueIsNull()
    {
        // given & when
        var result = ((decimal?)null).DisplayPoundsPences();

        // then
        result.Should().BeNull();
    }

    [Theory]
    [SuppressMessage("Performance", "CA1825:Avoid zero-length array allocations", Justification = "False positive returned by analyzer")]
    [MemberData(nameof(MoneyData))]
    public void ShouldReturnValueWithPoundsSymbolThousandsSeparatorAndPences(decimal value, string expectedValue)
    {
        // given & when
        var result = CurrencyHelper.DisplayPoundsPences(value);

        // then
        result.Should().Be(expectedValue);
    }
}
