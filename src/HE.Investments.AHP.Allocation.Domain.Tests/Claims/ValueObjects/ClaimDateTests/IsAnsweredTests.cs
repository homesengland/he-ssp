using FluentAssertions;
using HE.Investments.AHP.Allocation.Domain.Claims.ValueObjects;
using Xunit;

namespace HE.Investments.AHP.Allocation.Domain.Tests.Claims.ValueObjects.ClaimDateTests;

public class IsAnsweredTests
{
    [Fact]
    public void ShouldReturnTrue_WhenForecastClaimDateAndAchievementDateAreProvided()
    {
        // given
        var testCandidate = new ClaimDate(new DateTime(2023, 1, 1, 0, 0, 0, DateTimeKind.Local), new AchievementDate(new DateTime(2024, 12, 12, 0, 0, 0, DateTimeKind.Local)));

        // when
        var result = testCandidate.IsAnswered();

        // then
        result.Should().BeTrue();
    }

    [Fact]
    public void ShouldReturnFalse_WhenAchievementDateIsNotProvided()
    {
        // given
        var testCandidate = new ClaimDate(new DateTime(2023, 1, 1, 0, 0, 0, DateTimeKind.Local));

        // when
        var result = testCandidate.IsAnswered();

        // then
        result.Should().BeFalse();
    }
}
