using FluentAssertions;
using HE.Investments.AHP.Allocation.Contract.Claims.Enum;
using HE.Investments.AHP.Allocation.Domain.Claims.Crm;
using HE.Investments.AHP.Allocation.Domain.Claims.ValueObjects;
using HE.Investments.AHP.Allocation.Domain.Tests.TestObjectBuilders;
using HE.Investments.Common.Contract;
using HE.Investments.Common.CRM.Model;
using HE.Investments.TestsUtils.TestFramework;
using Xunit;

namespace HE.Investments.AHP.Allocation.Domain.Tests.Claims.Crm.PhaseCrmMapperTests;

public class MapToDtoTests : TestBase<PhaseCrmMapper>
{
    [Fact]
    public void ShouldMapPhase_WhenOnlyCompletionMilestoneIsProvided()
    {
        // given
        var entity = PhaseEntityTestBuilder.New()
            .WithId("hoDlTGbjXE2ZOfVuSF70Bw")
            .WithName("My first phase")
            .WithCompletionMilestone(MilestoneClaimTestBuilder.Draft().Build())
            .Build();

        // when
        var result = TestCandidate.MapToDto(entity);

        // then
        result.Id.Should().Be("4ce58086-e366-4d5c-9939-f56e485ef407");
        result.Name.Should().Be("My first phase");
        result.AcquisitionMilestone.Should().BeNull();
        result.StartOnSiteMilestone.Should().BeNull();
        result.CompletionMilestone.Should().NotBeNull();
    }

    [Fact]
    public void ShouldMapPhase_WhenAllMilestonesAreProvided()
    {
        // given
        var entity = PhaseEntityTestBuilder.New()
            .WithAcquisitionMilestone(MilestoneClaimTestBuilder.Draft().Submitted().Build())
            .WithStartOnSiteMilestone(MilestoneClaimTestBuilder.Draft().Submitted().Build())
            .WithCompletionMilestone(MilestoneClaimTestBuilder.Draft().Build())
            .Build();

        // when
        var result = TestCandidate.MapToDto(entity);

        // then
        result.AcquisitionMilestone.Should().NotBeNull();
        result.StartOnSiteMilestone.Should().NotBeNull();
        result.CompletionMilestone.Should().NotBeNull();
    }

    [Fact]
    public void ShouldMapMilestoneToNull_WhenMilestoneHasNoClaim()
    {
        // given
        var milestone = MilestoneClaimTestBuilder.Draft().WithoutClaim().Build();
        var entity = PhaseEntityTestBuilder.New().WithAcquisitionMilestone(milestone).Build();

        // when
        var result = TestCandidate.MapToDto(entity);

        // then
        result.AcquisitionMilestone.Should().BeNull();
    }

    [Fact]
    public void ShouldMapMilestone_WhenMilestoneIsDraft()
    {
        // given
        var milestone = MilestoneClaimTestBuilder.Draft()
            .WithType(MilestoneType.Acquisition)
            .WithGrantApportioned(100000, 0.1m)
            .WithCostsIncurred(true)
            .WithConfirmation(true)
            .WithMilestoneAchievedDate(AchievementDate.FromDateDetails(new DateDetails("01", "01", "2020")))
            .WithMilestoneSubmissionDate(new DateTime(2020, 2, 2, 0, 0, 0, DateTimeKind.Local))
            .Build();
        var entity = PhaseEntityTestBuilder.New().WithAcquisitionMilestone(milestone).Build();

        // when
        var result = TestCandidate.MapToDto(entity);

        // then
        result.AcquisitionMilestone.Should().NotBeNull();
        result.AcquisitionMilestone.Type.Should().Be((int)invln_Milestone.Acquisition);
        result.AcquisitionMilestone.Status.Should().Be((int)invln_ClaimExternalStatus.Draft);
        result.AcquisitionMilestone.AchievementDate.Should().Be(new DateTime(2020, 1, 1, 0, 0, 0, DateTimeKind.Local));
        result.AcquisitionMilestone.SubmissionDate.Should().Be(new DateTime(2020, 2, 2, 0, 0, 0, DateTimeKind.Local));
        result.AcquisitionMilestone.CostIncurred.Should().BeTrue();
        result.AcquisitionMilestone.IsConfirmed.Should().BeTrue();
    }
}
