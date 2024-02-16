using FluentAssertions;
using HE.Investment.AHP.Contract.Delivery.Enums;
using HE.Investment.AHP.Domain.Delivery.ValueObjects;
using HE.Investment.AHP.Domain.Tests.Delivery.Entities.TestDataBuilders;

namespace HE.Investment.AHP.Domain.Tests.Delivery.Entities.DeliveryPhaseTranchesTests;

public class CalculateTranchesTests
{
    [Theory]
    [InlineData(5_000_333, 3_000_199.8, 1_050_069, 750049, 1_200_081.8)]
    [InlineData(300_000, 180_000, 63_000, 45_000, 72_000)]
    [InlineData(1, 0.6, 0, 0, 0.6)]
    public void ShouldCalculateTranchesBaseOnMilestoneFramework(
        decimal requiredFunding,
        decimal expectedGrantApportioned,
        decimal expectedAcquisitionMilestone,
        decimal expectedStarOnSiteMilestone,
        decimal expectedCompletionMilestone)
    {
        // given
        var tranches = new DeliveryPhaseEntityBuilder()
            .WithHomesToBeDelivered(10)
            .WithHomesToBeDelivered(20)
            .WithSchemeFunding((int)requiredFunding, 50)
            .WithMilestoneFramework(new MilestoneFramework(0.35m, 0.25m, 0.4m))
            .Build()
            .Tranches;

        // when
        var milestonesTranches = tranches.CalculateTranches();

        // then
        tranches.GrantApportioned.Should().Be(expectedGrantApportioned);
        milestonesTranches.AcquisitionMilestone.Should().Be(expectedAcquisitionMilestone);
        milestonesTranches.StartOnSiteMilestone.Should().Be(expectedStarOnSiteMilestone);
        milestonesTranches.CompletionMilestone.Should().Be(expectedCompletionMilestone);
    }

    [Fact]
    public void ShouldReturnOnlyCompletionMilestone_WhenDeliveryPhaseIsForUnregisteredBody()
    {
        // given
        var requestedFunding = 1000;
        var tranches = new DeliveryPhaseEntityBuilder()
            .WithUnregisteredBody()
            .WithHomesToBeDelivered(10)
            .WithSchemeFunding(requestedFunding, 10)
            .WithMilestoneFramework(new MilestoneFramework(0.35m, 0.25m, 0.4m))
            .WithoutAcquisitionMilestone()
            .WithoutStartOnSiteMilestone()
            .Build()
            .Tranches;

        // when
        var milestonesTranches = tranches.CalculateTranches();

        // then
        tranches.GrantApportioned.Should().Be(requestedFunding);
        milestonesTranches.AcquisitionMilestone.Should().BeNull();
        milestonesTranches.StartOnSiteMilestone.Should().BeNull();
        milestonesTranches.CompletionMilestone.Should().Be(requestedFunding);
    }

    [Theory]
    [InlineData(BuildActivityType.OffTheShelf)]
    [InlineData(BuildActivityType.ExistingSatisfactory)]
    public void ShouldReturnOnlyCompletionMilestone_WhenBuildActivityIsOffTheShelfOrExistingSatisfactory(BuildActivityType buildActivityType)
    {
        // given
        var requestedFunding = 1000;
        var tranches = new DeliveryPhaseEntityBuilder()
            .WithRehabBuildActivity(buildActivityType)
            .WithHomesToBeDelivered(10)
            .WithSchemeFunding(requestedFunding, 10)
            .WithMilestoneFramework(new MilestoneFramework(0.35m, 0.25m, 0.4m))
            .WithoutAcquisitionMilestone()
            .WithoutStartOnSiteMilestone()
            .Build()
            .Tranches;

        // when
        var milestonesTranches = tranches.CalculateTranches();

        // then
        tranches.GrantApportioned.Should().Be(requestedFunding);
        milestonesTranches.AcquisitionMilestone.Should().BeNull();
        milestonesTranches.StartOnSiteMilestone.Should().BeNull();
        milestonesTranches.CompletionMilestone.Should().Be(requestedFunding);
    }

    [Fact]
    public void ShouldNotCalculateValue_WhenNoHomesToBeDelivered()
    {
        // given
        var tranches = new DeliveryPhaseEntityBuilder()
            .WithoutHomesToDeliver()
            .WithSchemeFunding(1000, 50)
            .WithMilestoneFramework(new MilestoneFramework(0.35m, 0.25m, 0.4m))
            .Build()
            .Tranches;

        // when
        var milestonesTranches = tranches.CalculateTranches();

        // then
        milestonesTranches.AcquisitionMilestone.Should().BeNull();
        milestonesTranches.StartOnSiteMilestone.Should().BeNull();
        milestonesTranches.CompletionMilestone.Should().BeNull();
    }

    [Fact]
    public void ShouldNotCalculateValue_WhenNoRequiredFunding()
    {
        // given
        var tranches = new DeliveryPhaseEntityBuilder()
            .WithSchemeFunding(null, 10)
            .WithMilestoneFramework(new MilestoneFramework(0.35m, 0.25m, 0.4m))
            .Build()
            .Tranches;

        // when
        var milestonesTranches = tranches.CalculateTranches();

        // then
        milestonesTranches.AcquisitionMilestone.Should().BeNull();
        milestonesTranches.StartOnSiteMilestone.Should().BeNull();
        milestonesTranches.CompletionMilestone.Should().BeNull();
    }

    [Fact]
    public void ShouldNotCalculateValue_WhenNoTotalHousesToDeliver()
    {
        // given
        var tranches = new DeliveryPhaseEntityBuilder()
            .WithSchemeFunding(1000, 0)
            .WithMilestoneFramework(new MilestoneFramework(0.35m, 0.25m, 0.4m))
            .Build()
            .Tranches;

        // when
        var milestonesTranches = tranches.CalculateTranches();

        // then
        milestonesTranches.AcquisitionMilestone.Should().BeNull();
        milestonesTranches.StartOnSiteMilestone.Should().BeNull();
        milestonesTranches.CompletionMilestone.Should().BeNull();
    }
}
