using FluentAssertions;
using HE.Investments.AHP.Allocation.Domain.Tests.TestObjectBuilders;
using Xunit;

namespace HE.Investments.AHP.Allocation.Domain.Tests.Claims.ValueObjects.MilestoneClaimTests;

public class IsEditableTests
{
    [Fact]
    public void ShouldReturnFalse_WhenMilestoneClaimIsSubmitted()
    {
        // given
        var testCandidate = MilestoneClaimTestBuilder.Draft().Submitted().Build();

        // when
        var result = testCandidate.IsEditable;

        // then
        result.Should().BeFalse();
    }

    [Fact]
    public void ShouldReturnTrue_WhenMilestoneClaimIsDraft()
    {
        // given
        var testCandidate = MilestoneClaimTestBuilder.Draft().Build();

        // when
        var result = testCandidate.IsEditable;

        // then
        result.Should().BeTrue();
    }
}
