using FluentAssertions;
using HE.Investments.AHP.Allocation.Domain.Claims.Enums;
using HE.Investments.AHP.Allocation.Domain.Tests.TestObjectBuilders;
using Xunit;

namespace HE.Investments.AHP.Allocation.Domain.Tests.Claims.ValueObjects.MilestoneClaimTests;

public class IsClaimedTests
{
    [Theory]
    [InlineData(MilestoneStatus.Undefined)]
    [InlineData(MilestoneStatus.Draft)]
    public void ShouldReturnFalse_WhenStatusIs(MilestoneStatus status)
    {
        // given
        var testCandidate = MilestoneClaimTestBuilder.New().WithStatus(status).Build();

        // when
        var result = testCandidate.IsClaimed;

        // then
        result.Should().BeFalse();
    }

    [Theory]
    [InlineData(MilestoneStatus.Submitted)]
    [InlineData(MilestoneStatus.UnderReview)]
    [InlineData(MilestoneStatus.Approved)]
    [InlineData(MilestoneStatus.Rejected)]
    [InlineData(MilestoneStatus.Reclaimed)]
    public void ShouldReturnTrue_WhenStatusIs(MilestoneStatus status)
    {
        // given
        var testCandidate = MilestoneClaimTestBuilder.New().WithStatus(status).Build();

        // when
        var result = testCandidate.IsClaimed;

        // then
        result.Should().BeTrue();
    }
}
