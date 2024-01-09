using FluentAssertions;
using HE.Investment.AHP.Contract.Delivery;
using HE.Investment.AHP.Contract.HomeTypes;
using HE.Investment.AHP.Domain.Delivery.Entities;
using HE.Investment.AHP.Domain.Delivery.ValueObjects;
using HE.Investment.AHP.Domain.HomeTypes.ValueObjects;
using HE.Investment.AHP.Domain.Tests.Delivery.Entities.TestDataBuilders;
using HE.Investments.Common.Contract.Exceptions;

namespace HE.Investment.AHP.Domain.Tests.Delivery.Entities.DeliveryPhasesEntityTests;

public class GetHomesToBeDeliveredInPhaseTests
{
    [Fact]
    public void ShouldThrowException_WhenDeliveryPhaseDoesNotExist()
    {
        // given
        var deliveryPhase = new DeliveryPhaseEntityBuilder().WithId("dp-1").Build();
        var testCandidate = new DeliveryPhasesEntityBuilder().WithDeliveryPhase(deliveryPhase).Build();

        // when
        var getHomes = () => testCandidate.GetHomesToDeliverInPhase(new DeliveryPhaseId("dp-2"));

        // then
        getHomes.Should().Throw<NotFoundException>().Which.EntityName.Should().Be(nameof(DeliveryPhaseEntity));
    }

    [Fact]
    public void ShouldReturnTotalHomesToBeDeliveredForEachHomeType()
    {
        // given
        var homeType1Id = new HomeTypeId("1 bed flat");
        var homeType2Id = new HomeTypeId("2 bed flat");
        var homeType3Id = new HomeTypeId("3 bed flat");

        var deliveryPhase1 = new DeliveryPhaseEntityBuilder()
            .WithId("dp-1")
            .WithHomesToBeDelivered(homeType1Id.Value, 6)
            .WithHomesToBeDelivered(homeType2Id.Value, 4)
            .Build();
        var deliveryPhase2 = new DeliveryPhaseEntityBuilder().WithId("dp-2").WithHomesToBeDelivered(homeType1Id.Value, 2).Build();
        var testCandidate = new DeliveryPhasesEntityBuilder()
            .WithDeliveryPhase(deliveryPhase1)
            .WithDeliveryPhase(deliveryPhase2)
            .WithHomesToDeliver(new HomesToDeliverBuilder().WithHomeTypeId(homeType1Id.Value).Build())
            .WithHomesToDeliver(new HomesToDeliverBuilder().WithHomeTypeId(homeType2Id.Value).Build())
            .WithHomesToDeliver(new HomesToDeliverBuilder().WithHomeTypeId(homeType3Id.Value).Build())
            .Build();

        // when
        var result = testCandidate.GetHomesToDeliverInPhase(deliveryPhase1.Id).ToList();

        // then
        result.Should().HaveCount(3);
        result.Single(x => x.HomesToDeliver.HomeTypeId == homeType1Id).ToDeliver.Should().Be(6);
        result.Single(x => x.HomesToDeliver.HomeTypeId == homeType2Id).ToDeliver.Should().Be(4);
        result.Single(x => x.HomesToDeliver.HomeTypeId == homeType3Id).ToDeliver.Should().Be(0);
    }
}
