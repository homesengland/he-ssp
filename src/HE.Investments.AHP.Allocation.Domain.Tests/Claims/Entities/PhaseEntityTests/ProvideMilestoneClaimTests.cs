using FluentAssertions;
using HE.Investments.AHP.Allocation.Contract.Claims.Enum;
using HE.Investments.AHP.Allocation.Domain.Tests.TestObjectBuilders;
using HE.Investments.Common.Contract.Exceptions;
using Xunit;

namespace HE.Investments.AHP.Allocation.Domain.Tests.Claims.Entities.PhaseEntityTests;

public class ProvideMilestoneClaimTests
{
    private static readonly DateTime Today = new(2024, 02, 12, 0, 0, 0, DateTimeKind.Local);

    [Fact]
    public void ShouldChangeMilestoneAndMarkAsModified_WhenDifferentAcquisitionMilestoneWasProvided()
    {
        // given
        var testCandidate = PhaseEntityTestBuilder.New()
            .WithAcquisitionMilestone(MilestoneClaimTestBuilder.Draft().WithType(MilestoneType.Acquisition).WithForecastClaimDate(Today).Build())
            .Build();
        var newMilestone = MilestoneClaimTestBuilder.Draft().WithType(MilestoneType.Acquisition).WithForecastClaimDate(Today.AddDays(10)).Build();

        // when
        testCandidate.ProvideMilestoneClaim(newMilestone);

        // then
        testCandidate.AcquisitionMilestone.Should().Be(newMilestone);
        testCandidate.IsModified.Should().BeTrue();
    }

    [Fact]
    public void ShouldDoNothing_WhenTheSameAcquisitionMilestoneWasProvided()
    {
        // given
        var testCandidate = PhaseEntityTestBuilder.New()
            .WithAcquisitionMilestone(MilestoneClaimTestBuilder.Draft().WithType(MilestoneType.Acquisition).Build())
            .Build();
        var milestone = MilestoneClaimTestBuilder.Draft().WithType(MilestoneType.Acquisition).Build();

        // when
        testCandidate.ProvideMilestoneClaim(milestone);

        // then
        testCandidate.AcquisitionMilestone.Should().Be(milestone);
        testCandidate.IsModified.Should().BeFalse();
    }

    [Fact]
    public void ShouldChangeMilestoneAndMarkAsModified_WhenDifferentStartOnSiteMilestoneWasProvided()
    {
        // given
        var testCandidate = PhaseEntityTestBuilder.New()
            .WithAcquisitionMilestone(MilestoneClaimTestBuilder.Draft().Submitted().Build())
            .WithStartOnSiteMilestone(MilestoneClaimTestBuilder.Draft().WithType(MilestoneType.StartOnSite).WithForecastClaimDate(Today).Build())
            .Build();
        var newMilestone = MilestoneClaimTestBuilder.Draft().WithType(MilestoneType.StartOnSite).WithForecastClaimDate(Today.AddDays(10)).Build();

        // when
        testCandidate.ProvideMilestoneClaim(newMilestone);

        // then
        testCandidate.StartOnSiteMilestone.Should().Be(newMilestone);
        testCandidate.IsModified.Should().BeTrue();
    }

    [Fact]
    public void ShouldDoNothing_WhenTheSameStartOnSiteMilestoneWasProvided()
    {
        // given
        var testCandidate = PhaseEntityTestBuilder.New()
            .WithAcquisitionMilestone(MilestoneClaimTestBuilder.Draft().Submitted().Build())
            .WithStartOnSiteMilestone(MilestoneClaimTestBuilder.Draft().WithType(MilestoneType.StartOnSite).Build())
            .Build();
        var milestone = MilestoneClaimTestBuilder.Draft().WithType(MilestoneType.StartOnSite).Build();

        // when
        testCandidate.ProvideMilestoneClaim(milestone);

        // then
        testCandidate.StartOnSiteMilestone.Should().Be(milestone);
        testCandidate.IsModified.Should().BeFalse();
    }

    [Fact]
    public void ShouldChangeMilestoneAndMarkAsModified_WhenDifferentCompletionMilestoneWasProvided()
    {
        // given
        var testCandidate = PhaseEntityTestBuilder.New()
            .WithAcquisitionMilestone(MilestoneClaimTestBuilder.Draft().Submitted().Build())
            .WithStartOnSiteMilestone(MilestoneClaimTestBuilder.Draft().Submitted().Build())
            .WithCompletionMilestone(MilestoneClaimTestBuilder.Draft().WithType(MilestoneType.Completion).WithForecastClaimDate(Today).Build())
            .Build();
        var newMilestone = MilestoneClaimTestBuilder.Draft().WithType(MilestoneType.Completion).WithForecastClaimDate(Today.AddDays(10)).Build();

        // when
        testCandidate.ProvideMilestoneClaim(newMilestone);

        // then
        testCandidate.CompletionMilestone.Should().Be(newMilestone);
        testCandidate.IsModified.Should().BeTrue();
    }

    [Fact]
    public void ShouldDoNothing_WhenTheSameCompletionMilestoneWasProvided()
    {
        // given
        var testCandidate = PhaseEntityTestBuilder.New()
            .WithAcquisitionMilestone(MilestoneClaimTestBuilder.Draft().Submitted().Build())
            .WithStartOnSiteMilestone(MilestoneClaimTestBuilder.Draft().Submitted().Build())
            .WithCompletionMilestone(MilestoneClaimTestBuilder.Draft().WithType(MilestoneType.Completion).Build())
            .Build();
        var milestone = MilestoneClaimTestBuilder.Draft().WithType(MilestoneType.Completion).Build();

        // when
        testCandidate.ProvideMilestoneClaim(milestone);

        // then
        testCandidate.CompletionMilestone.Should().Be(milestone);
        testCandidate.IsModified.Should().BeFalse();
    }

    [Fact]
    public void ShouldThrowException_WhenMilestoneCannotBeClaimed()
    {
        // given
        var testCandidate = PhaseEntityTestBuilder.New()
            .WithAcquisitionMilestone(MilestoneClaimTestBuilder.Draft().Build())
            .WithStartOnSiteMilestone(MilestoneClaimTestBuilder.Draft().Build())
            .Build();
        var milestone = MilestoneClaimTestBuilder.Draft().WithType(MilestoneType.StartOnSite).WithConfirmation(true).Build();

        // when
        var provide = () => testCandidate.ProvideMilestoneClaim(milestone);

        // then
        provide.Should().Throw<DomainValidationException>().WithMessage("Start on site milestone cannot be claimed right now.");
    }
}
