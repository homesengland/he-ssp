using FluentAssertions;
using HE.Investments.Common.Contract.Enum;
using HE.Investments.Common.WWW.Helpers;

namespace HE.Investments.Common.WWW.Tests.Helpers.SummaryAnswerHelperTests;

public class ToEnumTests
{
    [Fact]
    public void ShouldReturnNull_WhenEnumValueIsNotProvided()
    {
        // given & when
        var result = SummaryAnswerHelper.ToEnum((YesNoType?)null);

        // then
        result.Should().BeNull();
    }

    [Fact]
    public void ShouldReturnEnumDescription_WhenEnumValueIsProvided()
    {
        // given & when
        var result = SummaryAnswerHelper.ToEnum(YesNoType.Yes);

        // then
        result.Should().ContainSingle().Which.Should().Be("Yes");
    }
}
