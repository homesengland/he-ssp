using FluentAssertions;
using HE.Investments.Common.Extensions;
using Xunit;

namespace HE.Investments.Common.Tests.Extensions.DecimalExtensions;

public class ToPoundsStringTests
{
    [Fact]
    public void ShouldReturnNull_WhenDecimalIsNull()
    {
        // given
        decimal? val = null;

        // when
        var result = val.ToPoundsString();

        // then
        result.Should().BeNull();
    }

    [Theory]
    [InlineData(123.45, "123.45")]
    [InlineData(280.2, "280.20")]
    public void ShouldReturnExactString_WhenDecimalIsNotNull(decimal input, string expected)
    {
        // given & when
        var result = input.ToPoundsString();

        // then
        result.Should().Be(expected);
    }

    [Fact]
    public void ShouldReturnExactString_WhenDecimalHasNoPlacesAfterDot()
    {
        // given
        decimal? val = 123;

        // when
        var result = val.ToPoundsString();

        // then
        result.Should().Be("123");
    }
}
