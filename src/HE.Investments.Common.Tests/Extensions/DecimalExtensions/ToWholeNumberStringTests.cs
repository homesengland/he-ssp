using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using HE.Investments.Common.Extensions;
using Xunit;

namespace HE.Investments.Common.Tests.Extensions.DecimalExtensions;

[SuppressMessage("Performance", "CA1825:Avoid zero-length array allocations", Justification = "TestCases")]
public class ToWholeNumberStringTests
{
    public static readonly TheoryData<decimal?, string?> TestCases =
        new()
        {
            { null, null },
            { 0, "0" },
            { 0.45m, "0" },
            { 0.55m, "0" },
            { 1m, "1" },
            { 1.5667m, "1" },
            { -1.5667m, "-1" },
        };

    [Theory]
    [MemberData(nameof(TestCases))]
    public void ReturnWholeNumberString(decimal? input, string? expected)
    {
        var actual = input.ToWholeNumberString();

        actual.Should().Be(expected);
    }
}
