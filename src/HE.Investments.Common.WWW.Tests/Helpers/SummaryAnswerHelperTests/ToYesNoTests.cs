using FluentAssertions;
using HE.Investments.Common.WWW.Helpers;

namespace HE.Investments.Common.WWW.Tests.Helpers.SummaryAnswerHelperTests;

public class ToYesNoTests
{
    [Fact]
    public void ShouldReturnNull_WhenAnswerIsNotProvided()
    {
        // given & when
        var result = SummaryAnswerHelper.ToYesNo(null);

        // then
        result.Should().BeNull();
    }

    [Fact]
    public void ShouldReturnYes_WhenAnswerIsTrue()
    {
        // given & when
        var result = SummaryAnswerHelper.ToYesNo(true);

        // then
        result.Should().ContainSingle().Which.Should().Be("Yes");
    }

    [Fact]
    public void ShouldReturnNo_WhenAnswerIsFalse()
    {
        // given & when
        var result = SummaryAnswerHelper.ToYesNo(false);

        // then
        result.Should().ContainSingle().Which.Should().Be("No");
    }

    [Fact]
    public void ShouldReturnYesWithAdditionalText_WhenAnswerIsTrueAndAdditionalTextIsProvided()
    {
        // given & when
        var result = SummaryAnswerHelper.ToYesNo(true, "Additional text");

        // then
        result.Should().ContainSingle().Which.Should().Be("Yes, Additional text");
    }

    [Fact]
    public void ShouldReturnNoWithAdditionalText_WhenAnswerIsFalseAndAdditionalTextIsProvided()
    {
        // given & when
        var result = SummaryAnswerHelper.ToYesNo(false, "Additional text");

        // then
        result.Should().ContainSingle().Which.Should().Be("No, Additional text");
    }
}
