using FluentAssertions;
using HE.Investments.Common.Extensions;
using Xunit;

namespace HE.Investments.Common.Tests.Extensions.DecimalExtensions;

public class ToWholePercentageTests
{
    [Theory]
    [InlineData(0.5, "50%")]
    [InlineData(0.51, "51%")]
    [InlineData(0.51888, "52%")]
    [InlineData(0, "0%")]
    [InlineData(1.337, "134%")]
    public void ReturnTrue_WhenProvidedDateIsBeforeOtherDate(decimal value, string expectedString)
    {
        // given & when
        var stringValue = value.ToWholePercentage();

        // then
        stringValue.Should().Be(expectedString);
    }

    [Fact]
    public void ReturnNull_WhenDecimalIsNull()
    {
        // given & then
        var stringValue = ((decimal?)null).ToWholePercentage();

        // then exception should not be thrown
        stringValue.Should().BeNull();
    }
}
