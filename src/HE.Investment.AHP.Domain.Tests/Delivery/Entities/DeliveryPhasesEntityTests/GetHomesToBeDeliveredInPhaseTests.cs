using FluentAssertions;
using HE.Investment.AHP.Contract.Delivery;
using HE.Investment.AHP.Contract.HomeTypes;
using HE.Investment.AHP.Domain.Delivery.Entities;
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
        var getHomes = () => testCandidate.GetHomesToDeliverInPhase(new DeliveryPhaseId("dp-2")).ToList();

        // then
        getHomes.Should().Throw<NotFoundException>().Which.EntityName.Should().Be(nameof(DeliveryPhaseEntity));
    }

    [Fact]
    public void ShouldReturnNull_WhenHomeTypeIsNotDeliveredInThisPhase()
    {
        // given
        var homeTypeId = new HomeTypeId("1 bed flat");
        var deliveryPhase = new DeliveryPhaseEntityBuilder().WithId("dp-1").Build();
        var testCandidate = new DeliveryPhasesEntityBuilder()
            .WithDeliveryPhase(deliveryPhase)
            .WithHomesToDeliver(new HomesToDeliverBuilder().WithHomeTypeId(homeTypeId.Value).Build())
            .Build();

        // when
        var result = testCandidate.GetHomesToDeliverInPhase(deliveryPhase.Id).ToList();

        // then
        result.Should().HaveCount(1);
        result.Single(x => x.HomesToDeliver.HomeTypeId == homeTypeId).ToDeliver.Should().BeNull();
    }

    [Fact]
    public void ShouldNotReturnHomeType_WhenHomeTypeIsFullyDeliveredInOtherPhase()
    {
        // given
        var homeTypeId = new HomeTypeId("1 bed flat");
        var deliveryPhase = new DeliveryPhaseEntityBuilder().WithId("dp-1").Build();
        var otherPhase = new DeliveryPhaseEntityBuilder().WithId("dp-2").WithHomesToBeDelivered(10, homeTypeId.Value).Build();
        var testCandidate = new DeliveryPhasesEntityBuilder()
            .WithDeliveryPhase(deliveryPhase)
            .WithDeliveryPhase(otherPhase)
            .WithHomesToDeliver(new HomesToDeliverBuilder().WithHomeTypeId(homeTypeId.Value).WithTotalHomes(10).Build())
            .Build();

        // when
        var result = testCandidate.GetHomesToDeliverInPhase(deliveryPhase.Id).ToList();

        // then
        result.Should().BeEmpty();
    }

    [Theory]
    [InlineData(10)]
    [InlineData(5)]
    [InlineData(0)]
    public void ShouldReturnHomeType_WhenHomeTypeIsDeliveredInThisPhase(int toDeliver)
    {
        // given
        var homeTypeId = new HomeTypeId("1 bed flat");
        var deliveryPhase = new DeliveryPhaseEntityBuilder().WithId("dp-1").WithHomesToBeDelivered(toDeliver, homeTypeId.Value).Build();
        var otherPhase = new DeliveryPhaseEntityBuilder().WithId("dp-2").Build();
        var testCandidate = new DeliveryPhasesEntityBuilder()
            .WithDeliveryPhase(deliveryPhase)
            .WithDeliveryPhase(otherPhase)
            .WithHomesToDeliver(new HomesToDeliverBuilder().WithHomeTypeId(homeTypeId.Value).WithTotalHomes(10).Build())
            .Build();

        // when
        var result = testCandidate.GetHomesToDeliverInPhase(deliveryPhase.Id).ToList();

        // then
        result.Should().HaveCount(1);
        result.Single(x => x.HomesToDeliver.HomeTypeId == homeTypeId).ToDeliver.Should().Be(toDeliver);
    }
}
