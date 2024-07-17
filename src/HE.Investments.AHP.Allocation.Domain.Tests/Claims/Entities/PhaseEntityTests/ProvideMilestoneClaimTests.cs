using FluentAssertions;
using HE.Investments.AHP.Allocation.Contract.Claims.Enum;
using HE.Investments.AHP.Allocation.Domain.Tests.TestObjectBuilders;
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
            .WithAcquisitionMilestone(MilestoneClaimTestBuilder.New().WithType(MilestoneType.Acquisition).WithForecastClaimDate(Today).Build())
            .Build();
        var newMilestone = MilestoneClaimTestBuilder.New().WithType(MilestoneType.Acquisition).WithForecastClaimDate(Today.AddDays(10)).Build();

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
            .WithAcquisitionMilestone(MilestoneClaimTestBuilder.New().WithType(MilestoneType.Acquisition).Build())
            .Build();
        var milestone = MilestoneClaimTestBuilder.New().WithType(MilestoneType.Acquisition).Build();

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
            .WithStartOnSiteMilestone(MilestoneClaimTestBuilder.New().WithType(MilestoneType.StartOnSite).WithForecastClaimDate(Today).Build())
            .Build();
        var newMilestone = MilestoneClaimTestBuilder.New().WithType(MilestoneType.StartOnSite).WithForecastClaimDate(Today.AddDays(10)).Build();

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
            .WithStartOnSiteMilestone(MilestoneClaimTestBuilder.New().WithType(MilestoneType.StartOnSite).Build())
            .Build();
        var milestone = MilestoneClaimTestBuilder.New().WithType(MilestoneType.StartOnSite).Build();

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
            .WithStartOnSiteMilestone(MilestoneClaimTestBuilder.New().WithType(MilestoneType.Completion).WithForecastClaimDate(Today).Build())
            .Build();
        var newMilestone = MilestoneClaimTestBuilder.New().WithType(MilestoneType.Completion).WithForecastClaimDate(Today.AddDays(10)).Build();

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
            .WithStartOnSiteMilestone(MilestoneClaimTestBuilder.New().WithType(MilestoneType.Completion).Build())
            .Build();
        var milestone = MilestoneClaimTestBuilder.New().WithType(MilestoneType.Completion).Build();

        // when
        testCandidate.ProvideMilestoneClaim(milestone);

        // then
        testCandidate.CompletionMilestone.Should().Be(milestone);
        testCandidate.IsModified.Should().BeFalse();
    }
}
