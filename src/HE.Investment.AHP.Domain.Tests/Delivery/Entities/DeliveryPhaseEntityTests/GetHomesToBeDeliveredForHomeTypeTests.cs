using FluentAssertions;
using HE.Investment.AHP.Domain.HomeTypes.ValueObjects;
using HE.Investment.AHP.Domain.Tests.Delivery.Entities.TestDataBuilders;

namespace HE.Investment.AHP.Domain.Tests.Delivery.Entities.DeliveryPhaseEntityTests;

public class GetHomesToBeDeliveredForHomeTypeTests
{
    [Fact]
    public void ShouldReturnZero_WhenHomeTypeIsNotUsedInDeliveryPhase()
    {
        // given
        var testCandidate = new DeliveryPhaseEntityBuilder()
            .WithHomesToBeDelivered("ht-1", 10)
            .WithHomesToBeDelivered("ht-2", 5)
            .Build();

        // when
        var result = testCandidate.GetHomesToBeDeliveredForHomeType(new HomeTypeId("ht-3"));

        // then
        result.Should().Be(0);
    }

    [Fact]
    public void ShouldReturnValue_WhenHomeTypeIsUsedInDeliveryPhase()
    {
        // given
        var testCandidate = new DeliveryPhaseEntityBuilder()
            .WithHomesToBeDelivered("ht-1", 10)
            .WithHomesToBeDelivered("ht-2", 5)
            .Build();

        // when
        var result = testCandidate.GetHomesToBeDeliveredForHomeType(new HomeTypeId("ht-1"));

        // then
        result.Should().Be(10);
    }
}
