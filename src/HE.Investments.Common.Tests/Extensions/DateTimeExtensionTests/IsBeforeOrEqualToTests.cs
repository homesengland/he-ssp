using FluentAssertions;
using HE.Investments.Common.Extensions;
using Xunit;

namespace HE.Investments.Common.Tests.Extensions.DateTimeExtensionTests;

public class IsBeforeOrEqualToTests
{
    [Fact]
    public void ReturnTrueWhenProvidedDateIsBeforeOtherDate()
    {
        var date = new DateTime(2023, 7, 5);
        var otherDate = date.AddSeconds(1);

        date.IsBeforeOrEqualTo(otherDate).Should().BeTrue();
    }

    [Fact]
    public void ReturnTrueWhenProvidedDateIsEqualToOtherDate()
    {
        var date = new DateTime(2023, 7, 5);
        var otherDate = new DateTime(2023, 7, 5);

        date.IsBeforeOrEqualTo(otherDate).Should().BeTrue();
    }

    [Fact]
    public void ReturnFalseWhenProvidedDateIsAfterOtherDate()
    {
        var date = new DateTime(2023, 7, 5);
        var otherDate = date.AddSeconds(-1);

        date.IsBeforeOrEqualTo(otherDate).Should().BeFalse();
    }
}
