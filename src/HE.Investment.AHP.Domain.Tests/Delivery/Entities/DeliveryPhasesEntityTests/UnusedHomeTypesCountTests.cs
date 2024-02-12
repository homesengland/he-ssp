using FluentAssertions;
using HE.Investment.AHP.Domain.Tests.Delivery.Entities.TestDataBuilders;

namespace HE.Investment.AHP.Domain.Tests.Delivery.Entities.DeliveryPhasesEntityTests;

public class UnusedHomeTypesCountTests
{
    [Fact]
    public void ShouldReturnZero_WhenThereAreNoHomeTypesSpecified()
    {
        // given
        var testCandidate = new DeliveryPhasesEntityBuilder().Build();

        // when
        var result = testCandidate.UnusedHomeTypesCount;

        // then
        result.Should().Be(0);
    }

    [Fact]
    public void ShouldReturnNegativeNumber_WhenTooManyHomeTypesAreProvided()
    {
        // given
        var deliveryPhase = new DeliveryPhaseEntityBuilder()
            .WithHomesToBeDelivered(2, "ht-1")
            .WithHomesToBeDelivered(2, "ht-2")
            .Build();
        var testCandidate = new DeliveryPhasesEntityBuilder()
            .WithHomesToDeliver(new HomesToDeliverBuilder().WithHomeTypeId("ht-1").WithTotalHomes(1).Build())
            .WithHomesToDeliver(new HomesToDeliverBuilder().WithHomeTypeId("ht-2").WithTotalHomes(1).Build())
            .WithDeliveryPhase(deliveryPhase)
            .Build();

        // when
        var result = testCandidate.UnusedHomeTypesCount;

        // then
        result.Should().Be(-2);
    }

    [Fact]
    public void ShouldReturnTotalHomesToDeliver_WhenThereAreNoDeliveryPhases()
    {
        // given
        var testCandidate = new DeliveryPhasesEntityBuilder()
            .WithHomesToDeliver(new HomesToDeliverBuilder().WithTotalHomes(2).Build())
            .WithHomesToDeliver(new HomesToDeliverBuilder().WithTotalHomes(3).Build())
            .WithHomesToDeliver(new HomesToDeliverBuilder().WithTotalHomes(4).Build())
            .Build();

        // when
        var result = testCandidate.UnusedHomeTypesCount;

        // then
        result.Should().Be(9);
    }

    [Fact]
    public void ShouldReturnTotalHomesToDeliverReducedBySpecifiedInDeliveryPhases_WhenThereAreSomeDeliveryPhases()
    {
        // given
        var deliveryPhase = new DeliveryPhaseEntityBuilder()
            .WithHomesToBeDelivered(1, "ht-1")
            .WithHomesToBeDelivered(3, "ht-2")
            .Build();
        var testCandidate = new DeliveryPhasesEntityBuilder()
            .WithHomesToDeliver(new HomesToDeliverBuilder().WithHomeTypeId("ht-1").WithTotalHomes(2).Build())
            .WithHomesToDeliver(new HomesToDeliverBuilder().WithHomeTypeId("ht-2").WithTotalHomes(3).Build())
            .WithHomesToDeliver(new HomesToDeliverBuilder().WithHomeTypeId("ht-3").WithTotalHomes(1).Build())
            .WithDeliveryPhase(deliveryPhase)
            .Build();

        // when
        var result = testCandidate.UnusedHomeTypesCount;

        // then
        result.Should().Be(2);
    }
}
