using FluentAssertions;
using HE.Investments.AHP.Allocation.Domain.Claims.Enums;
using HE.Investments.AHP.Allocation.Domain.Tests.TestObjectBuilders;
using Xunit;

namespace HE.Investments.AHP.Allocation.Domain.Tests.Claims.ValueObjects.MilestoneClaimTests;

public class IsSubmittedTests
{
    [Fact]
    public void ShouldReturnFalse_WhenMilestoneClaimIsDraft()
    {
        // given
        var testCandidate = MilestoneClaimTestBuilder.Draft().Build();

        // when
        var result = testCandidate.IsSubmitted;

        // then
        result.Should().BeFalse();
    }

    [Theory]
    [InlineData(MilestoneStatus.Submitted)]
    [InlineData(MilestoneStatus.UnderReview)]
    [InlineData(MilestoneStatus.Approved)]
    [InlineData(MilestoneStatus.Rejected)]
    [InlineData(MilestoneStatus.Paid)]
    public void ShouldReturnTrue_WhenStatusIs(MilestoneStatus status)
    {
        // given
        var testCandidate = MilestoneClaimTestBuilder.Draft().Submitted(status).Build();

        // when
        var result = testCandidate.IsSubmitted;

        // then
        result.Should().BeTrue();
    }
}
