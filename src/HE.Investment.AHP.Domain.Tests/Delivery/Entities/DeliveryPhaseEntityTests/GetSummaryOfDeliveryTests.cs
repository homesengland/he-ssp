using FluentAssertions;
using HE.Investment.AHP.Contract.Delivery.Enums;
using HE.Investment.AHP.Domain.Delivery.ValueObjects;
using HE.Investment.AHP.Domain.Tests.Delivery.Entities.TestDataBuilders;

namespace HE.Investment.AHP.Domain.Tests.Delivery.Entities.DeliveryPhaseEntityTests;

public class GetSummaryOfDeliveryTests
{
    [Theory]
    [InlineData(5_000_333, 3_000_199.8, 1_050_069, 750049, 1_200_081.8)]
    [InlineData(300_000, 180_000, 63_000, 45_000, 72_000)]
    [InlineData(1, 0.6, 0, 0, 0.6)]
    public void ShouldCalculateSummaryBaseOnMilestoneFramework(
        decimal requiredFunding,
        decimal expectedGrantApportioned,
        decimal expectedAcquisitionMilestone,
        decimal expectedStarOnSiteMilestone,
        decimal expectedCompletionMilestone)
    {
        // given
        var deliveryPhase = new DeliveryPhaseEntityBuilder()
            .WithHomesToBeDelivered(10)
            .WithHomesToBeDelivered(20)
            .WithSchemeFunding((int)requiredFunding, 50)
            .Build();

        // when
        var summary = deliveryPhase.GetSummaryOfDelivery(new MilestoneFramework(0.35m, 0.25m, 0.4m));

        // then
        summary.GrantApportioned.Should().Be(expectedGrantApportioned);
        summary.AcquisitionMilestone.Should().Be(expectedAcquisitionMilestone);
        summary.StartOnSiteMilestone.Should().Be(expectedStarOnSiteMilestone);
        summary.CompletionMilestone.Should().Be(expectedCompletionMilestone);
    }

    [Fact]
    public void ShouldReturnOnlyCompletionMilestone_WhenDeliveryPhaseIsForUnregisteredBody()
    {
        // given
        var requestedFunding = 1000;
        var deliveryPhase = new DeliveryPhaseEntityBuilder()
            .WithUnregisteredBody()
            .WithHomesToBeDelivered(10)
            .WithSchemeFunding(requestedFunding, 10)
            .Build();

        // when
        var summary = deliveryPhase.GetSummaryOfDelivery(new MilestoneFramework(0.35m, 0.25m, 0.4m));

        // then
        summary.GrantApportioned.Should().Be(requestedFunding);
        summary.AcquisitionMilestone.Should().BeNull();
        summary.StartOnSiteMilestone.Should().BeNull();
        summary.CompletionMilestone.Should().Be(requestedFunding);
    }

    [Theory]
    [InlineData(BuildActivityType.OffTheShelf)]
    [InlineData(BuildActivityType.ExistingSatisfactory)]
    public void ShouldReturnOnlyCompletionMilestone_WhenBuildActivityIsOffTheShelfOrExistingSatisfactory(BuildActivityType buildActivityType)
    {
        // given
        var requestedFunding = 1000;
        var deliveryPhase = new DeliveryPhaseEntityBuilder()
            .WithRehabBuildActivity(buildActivityType)
            .WithHomesToBeDelivered(10)
            .WithSchemeFunding(requestedFunding, 10)
            .Build();

        // when
        var summary = deliveryPhase.GetSummaryOfDelivery(new MilestoneFramework(0.35m, 0.25m, 0.4m));

        // then
        summary.GrantApportioned.Should().Be(requestedFunding);
        summary.AcquisitionMilestone.Should().BeNull();
        summary.StartOnSiteMilestone.Should().BeNull();
        summary.CompletionMilestone.Should().Be(requestedFunding);
    }

    [Fact]
    public void ShouldNotCalculateValue_WhenNoHomesToBeDelivered()
    {
        // given
        var deliveryPhase = new DeliveryPhaseEntityBuilder().WithoutHomesToDeliver().WithSchemeFunding(1000, 50).Build();

        // when
        var summary = deliveryPhase.GetSummaryOfDelivery(new MilestoneFramework(0.35m, 0.25m, 0.4m));

        // then
        summary.GrantApportioned.Should().BeNull();
        summary.AcquisitionMilestone.Should().BeNull();
        summary.StartOnSiteMilestone.Should().BeNull();
        summary.CompletionMilestone.Should().BeNull();
    }

    [Fact]
    public void ShouldNotCalculateValue_WhenNoRequiredFunding()
    {
        // given
        var deliveryPhase = new DeliveryPhaseEntityBuilder().WithSchemeFunding(null, 10).Build();

        // when
        var summary = deliveryPhase.GetSummaryOfDelivery(new MilestoneFramework(0.35m, 0.25m, 0.4m));

        // then
        summary.GrantApportioned.Should().BeNull();
        summary.AcquisitionMilestone.Should().BeNull();
        summary.StartOnSiteMilestone.Should().BeNull();
        summary.CompletionMilestone.Should().BeNull();
    }

    [Fact]
    public void ShouldNotCalculateValue_WhenNoTotalHousesToDeliver()
    {
        // given
        var deliveryPhase = new DeliveryPhaseEntityBuilder().WithSchemeFunding(1000, 0).Build();

        // when
        var summary = deliveryPhase.GetSummaryOfDelivery(new MilestoneFramework(0.35m, 0.25m, 0.4m));

        // then
        summary.GrantApportioned.Should().BeNull();
        summary.AcquisitionMilestone.Should().BeNull();
        summary.StartOnSiteMilestone.Should().BeNull();
        summary.CompletionMilestone.Should().BeNull();
    }
}
