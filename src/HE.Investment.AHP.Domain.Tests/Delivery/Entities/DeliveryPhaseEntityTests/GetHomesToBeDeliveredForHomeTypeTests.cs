using FluentAssertions;
using HE.Investment.AHP.Contract.HomeTypes;
using HE.Investment.AHP.Domain.Tests.Delivery.Entities.TestDataBuilders;

namespace HE.Investment.AHP.Domain.Tests.Delivery.Entities.DeliveryPhaseEntityTests;

public class GetHomesToBeDeliveredForHomeTypeTests
{
    [Fact]
    public void ShouldReturnNull_WhenHomeTypeIsNotUsedInDeliveryPhase()
    {
        // given
        var testCandidate = new DeliveryPhaseEntityBuilder()
            .WithHomesToBeDelivered("ht-1", 10)
            .WithHomesToBeDelivered("ht-2", 5)
            .Build();

        // when
        var result = testCandidate.GetHomesToBeDeliveredForHomeType(new HomeTypeId("ht-3"));

        // then
        result.Should().BeNull();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(10)]
    public void ShouldReturnValue_WhenHomeTypeIsUsedInDeliveryPhase(int toDeliver)
    {
        // given
        var testCandidate = new DeliveryPhaseEntityBuilder()
            .WithHomesToBeDelivered("ht-1", toDeliver)
            .WithHomesToBeDelivered("ht-2", 5)
            .Build();

        // when
        var result = testCandidate.GetHomesToBeDeliveredForHomeType(new HomeTypeId("ht-1"));

        // then
        result.Should().Be(toDeliver);
    }
}
