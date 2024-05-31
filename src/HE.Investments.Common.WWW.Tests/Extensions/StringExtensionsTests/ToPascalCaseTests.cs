using FluentAssertions;
using HE.Investments.Common.WWW.Extensions;

namespace HE.Investments.Common.WWW.Tests.Extensions.StringExtensionsTests;

public class ToPascalCaseTests
{
    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void ShouldReturnEmpty_WhenInputIsEmpty(string input)
    {
        // given & When
        var result = input.ToPascalCase();

        // then
        result.Should().BeEmpty();
    }

    [Fact]
    public void ShouldReturnSingleWordCapitalized_WhenInputIsSingleWord()
    {
        // given
        var input = "word";

        // when
        var result = input.ToPascalCase();

        // then
        result.Should().Be("Word");
    }

    [Fact]
    public void ShouldReturnWordsCapitalizedWithoutSpaces_WhenInputIsMultipleWords()
    {
        // given
        var input = "multiple words here";

        // when
        var result = input.ToPascalCase();

        // then
        result.Should().Be("MultipleWordsHere");
    }

    [Fact]
    public void ShouldIgnoreExtraSpaces_WhenInputHasExtraSpaces()
    {
        // given
        var input = "multiple   words   here";

        // when
        var result = input.ToPascalCase();

        // then
        result.Should().Be("MultipleWordsHere");
    }
}
