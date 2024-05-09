using FluentAssertions;
using HE.Investments.Common.Extensions;
using Xunit;

namespace HE.Investments.Common.Tests.Extensions.CollectionExtensionsTests;

public class PopItemTests
{
    [Fact]
    public void ShouldRemoveAndReturnFirstItem_WhenThereAreSomeItems()
    {
        // given
        var testCandidate = new List<string> { "a", "b", "c" };

        // when
        var result = testCandidate.PopItem();

        // then
        result.Should().Be("a");
        testCandidate.Should().BeEquivalentTo("b", "c");
    }

    [Fact]
    public void ShouldReturnNull_WhenThereAreNoItems()
    {
        // given
        var testCandidate = new List<string>();

        // when
        var result = testCandidate.PopItem();

        // then
        result.Should().BeNull();
        testCandidate.Should().BeEmpty();
    }
}
