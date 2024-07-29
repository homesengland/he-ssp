using FluentAssertions;
using HE.Investments.AHP.Allocation.Contract.Claims.Enum;
using HE.Investments.AHP.Allocation.Domain.Tests.TestObjectBuilders;
using HE.Investments.Common.Contract.Exceptions;
using HE.Investments.Common.Utils;
using Moq;
using Xunit;
using MilestoneStatus = HE.Investments.AHP.Allocation.Domain.Claims.Enums.MilestoneStatus;

namespace HE.Investments.AHP.Allocation.Domain.Tests.Claims.Entities.PhaseEntityTests;

public class SubmitMilestoneClaimTests
{
    private readonly IDateTimeProvider _dateTimeProvider = Mock.Of<IDateTimeProvider>();

    [Fact]
    public void ShouldChangeMilestoneAndMarkAsModified_WhenMilestoneIsSuccessfullySubmitted()
    {
        // given
        GetDateTimeProviderMock();
        var testCandidate = PhaseEntityTestBuilder.New()
            .WithAcquisitionMilestone(
                MilestoneClaimTestBuilder
                    .Draft()
                    .Submitted(MilestoneStatus.Submitted)
                    .Build())
            .WithStartOnSiteMilestone(
                MilestoneClaimTestBuilder
                    .Draft()
                    .Submitted(MilestoneStatus.Submitted)
                    .Build())
            .WithCompletionMilestone(
                MilestoneClaimTestBuilder
                    .Draft()
                    .WithConfirmation(true)
                    .Build())
            .Build();

        // when
        testCandidate.SubmitMilestoneClaim(MilestoneType.Completion, _dateTimeProvider.Now);

        // then
        testCandidate.CompletionMilestone.ClaimDate.SubmissionDate.Should().Be(_dateTimeProvider.Now);
        testCandidate.CompletionMilestone.Status.Should().Be(MilestoneStatus.Submitted);
        testCandidate.IsModified.Should().BeTrue();
    }

    [Fact]
    public void ShouldThrowException_WhenMilestoneWasAlreadySubmitted()
    {
        // given
        var testCandidate = PhaseEntityTestBuilder.New()
            .WithCompletionMilestone(
                MilestoneClaimTestBuilder
                    .Draft()
                    .Submitted(MilestoneStatus.Submitted)
                    .Build())
            .Build();

        // when
        var result = () => testCandidate.SubmitMilestoneClaim(MilestoneType.Completion, _dateTimeProvider.Now);

        // then
        result.Should().Throw<DomainValidationException>().WithMessage("Submission is not allowed for Submitted Claim");
    }

    private void GetDateTimeProviderMock()
    {
        Mock.Get(_dateTimeProvider)
            .Setup(x => x.Now)
            .Returns(new DateTime(2024, 07, 07, 12, 25, 0, DateTimeKind.Unspecified));
    }
}
