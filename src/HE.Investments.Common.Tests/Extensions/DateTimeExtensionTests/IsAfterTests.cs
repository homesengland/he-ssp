using FluentAssertions;
using HE.Investments.Common.Extensions;
using Xunit;

namespace HE.Investments.Common.Tests.Extensions.DateTimeExtensionTests;

public class IsAfterTests
{
    [Fact]
    public void ReturnFalseWhenProvidedDateIsBeforeOtherDate()
    {
        // given
        var date = new DateTime(2023, 7, 5, 0, 0, 0, DateTimeKind.Unspecified);
        var otherDate = date.AddSeconds(1);

        // given & then
        date.IsAfter(otherDate).Should().BeFalse();
    }

    [Fact]
    public void ReturnFalseWhenProvidedDateIsEqualToOtherDate()
    {
        // given
        var date = new DateTime(2023, 7, 5, 0, 0, 0, DateTimeKind.Unspecified);
        var otherDate = new DateTime(2023, 7, 5, 0, 0, 0, DateTimeKind.Unspecified);

        // given & then
        date.IsAfter(otherDate).Should().BeFalse();
    }

    [Fact]
    public void ReturnTrueWhenProvidedDateIsAfterOtherDate()
    {
        // given
        var date = new DateTime(2023, 7, 5, 0, 0, 0, DateTimeKind.Unspecified);
        var otherDate = date.AddSeconds(-1);

        // given & then
        date.IsAfter(otherDate).Should().BeTrue();
    }
}
