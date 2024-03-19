using FluentAssertions;
using HE.Investments.Common.Contract;
using HE.Investments.Common.WWW.Helpers;

namespace HE.Investments.Common.WWW.Tests.Helpers.SummaryAnswerHelperTests;

public class ToDateTests
{
    [Fact]
    public void ShouldReturnNull_WhenDateIsNotProvided()
    {
        // given & when
        var result = SummaryAnswerHelper.ToDate(null);

        // then
        result.Should().BeNull();
    }

    [Fact]
    public void ShouldReturnDateInUkFormat_WhenDateIsProvided()
    {
        // given
        var date = new DateDetails("31", "12", "2022");

        // when
        var result = SummaryAnswerHelper.ToDate(date);

        // then
        result.Should().ContainSingle().Which.Should().Be("31/12/2022");
    }

    [Fact]
    public void ShouldReturnDateInUkFormat_WhenDateIsProvidedWithLeadingZeroes()
    {
        // given
        var date = new DateDetails("01", "01", "2022");

        // when
        var result = SummaryAnswerHelper.ToDate(date);

        // then
        result.Should().ContainSingle().Which.Should().Be("01/01/2022");
    }
}
