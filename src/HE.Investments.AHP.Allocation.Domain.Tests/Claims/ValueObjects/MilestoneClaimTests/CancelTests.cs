using FluentAssertions;
using HE.Investments.AHP.Allocation.Contract.Claims.Enum;
using HE.Investments.AHP.Allocation.Domain.Claims.ValueObjects;
using HE.Investments.AHP.Allocation.Domain.Tests.TestObjectBuilders;
using HE.Investments.Common.Contract;
using HE.Investments.Common.Contract.Exceptions;
using Xunit;
using MilestoneStatus = HE.Investments.AHP.Allocation.Domain.Claims.Enums.MilestoneStatus;

namespace HE.Investments.AHP.Allocation.Domain.Tests.Claims.ValueObjects.MilestoneClaimTests;

public class CancelTests
{
    [Fact]
    public void ShouldThrowException_WhenClaimIsSubmitted()
    {
        // given
        var testCandidate = MilestoneClaimTestBuilder.Draft().Submitted().Build();

        // when
        var cancel = () => testCandidate.Cancel();

        // then
        cancel.Should().Throw<DomainValidationException>().WithMessage("Cancellation is not allowed for Submitted Claim");
    }

    [Fact]
    public void ShouldReturnCancelledClaim_WhenClaimIsInDraftStatus()
    {
        // given
        var testCandidate = MilestoneClaimTestBuilder.Draft()
            .WithType(MilestoneType.Acquisition)
            .WithMilestoneAchievedDate(DateDetails.FromDateTime(DateTime.Today)!)
            .WithCostsIncurred(true)
            .WithConfirmation(true)
            .Build();

        // when
        var result = testCandidate.Cancel();

        // then
        result.Should().BeOfType<MilestoneWithoutClaim>();
        result.Type.Should().Be(MilestoneType.Acquisition);
        result.Status.Should().Be(MilestoneStatus.Draft);
        result.ClaimDate.ForecastClaimDate.Should().Be(testCandidate.ClaimDate.ForecastClaimDate);
        result.ClaimDate.AchievementDate.Should().BeNull();
        result.CostsIncurred.Should().BeNull();
        result.IsConfirmed.Should().BeNull();
    }
}
