using FluentAssertions;
using HE.Investment.AHP.Contract.Delivery;
using HE.Investment.AHP.Contract.HomeTypes;
using HE.Investment.AHP.Domain.Delivery.Entities;
using HE.Investment.AHP.Domain.Delivery.ValueObjects;
using HE.Investment.AHP.Domain.HomeTypes.ValueObjects;
using HE.Investment.AHP.Domain.Tests.Delivery.Entities.TestDataBuilders;
using HE.Investments.Common.Contract.Exceptions;

namespace HE.Investment.AHP.Domain.Tests.Delivery.Entities.DeliveryPhasesEntityTests;

public class SetHomesToBeDeliveredInPhaseTests
{
    [Fact]
    public void ShouldThrowException_WhenPhaseWithGivenIdDoesNotExist()
    {
        // given
        var homeTypeId = new HomeTypeId("1 bed flat");
        var deliveryPhase = new DeliveryPhaseEntityBuilder().WithId("dp-1").Build();
        var testCandidate = new DeliveryPhasesEntityBuilder()
            .WithDeliveryPhase(deliveryPhase)
            .WithHomesToDeliver(new HomesToDeliverBuilder().WithHomeTypeId(homeTypeId.Value).WithTotalHomes(10).Build())
            .Build();

        // when
        var setHomes = () => testCandidate.SetHomesToBeDeliveredInPhase(new DeliveryPhaseId("dp-2"), new[] { new HomesToDeliverInPhase(homeTypeId, 1) });

        // then
        setHomes.Should().Throw<NotFoundException>().Which.EntityName.Should().Be(nameof(DeliveryPhaseEntity));
    }

    [Fact]
    public void ShouldThrowException_WhenHomeTypeWithGivenIdDoesNotExist()
    {
        // given
        var homeTypeId = new HomeTypeId("1 bed flat");
        var deliveryPhase = new DeliveryPhaseEntityBuilder().WithId("dp-1").Build();
        var testCandidate = new DeliveryPhasesEntityBuilder()
            .WithDeliveryPhase(deliveryPhase)
            .WithHomesToDeliver(new HomesToDeliverBuilder().WithHomeTypeId(homeTypeId.Value).WithTotalHomes(10).Build())
            .Build();

        // when
        var setHomes = () => testCandidate.SetHomesToBeDeliveredInPhase(deliveryPhase.Id, new[] { new HomesToDeliverInPhase(new HomeTypeId("2 bed flat"), 1) });

        // then
        setHomes.Should().Throw<NotFoundException>().Which.EntityName.Should().Be(nameof(HomesToDeliver));
    }

    [Fact]
    public void ShouldThrowException_WhenNumberOfHomesInThisPhaseExceedsTotalNumberOfHomes()
    {
        // given
        var homeType1Id = new HomeTypeId("1 bed flat");
        var homeType2Id = new HomeTypeId("2 bed flat");
        var homeType3Id = new HomeTypeId("3 bed flat");

        var deliveryPhase = new DeliveryPhaseEntityBuilder().WithId("dp-1").Build();
        var testCandidate = new DeliveryPhasesEntityBuilder()
            .WithDeliveryPhase(deliveryPhase)
            .WithHomesToDeliver(new HomesToDeliverBuilder().WithHomeTypeId(homeType1Id.Value).WithTotalHomes(10).Build())
            .WithHomesToDeliver(new HomesToDeliverBuilder().WithHomeTypeId(homeType2Id.Value).WithTotalHomes(5).Build())
            .WithHomesToDeliver(new HomesToDeliverBuilder().WithHomeTypeId(homeType3Id.Value).WithTotalHomes(2).Build())
            .Build();

        // when
        var setHomes = () => testCandidate.SetHomesToBeDeliveredInPhase(
            deliveryPhase.Id,
            new[]
            {
                new HomesToDeliverInPhase(homeType1Id, 11),
                new HomesToDeliverInPhase(homeType2Id, 6),
                new HomesToDeliverInPhase(homeType3Id, 1),
            });

        // then
        var errors = setHomes.Should().Throw<DomainValidationException>().Which.OperationResult.Errors;
        errors.Should().HaveCount(2);
        errors.Select(x => x.AffectedField).Should().BeEquivalentTo("HomeType-1 bed flat", "HomeType-2 bed flat");
    }

    [Fact]
    public void ShouldThrowException_WhenNumberOfHomesInAllPhasesExceedsTotalNumberOfHomes()
    {
        // given
        var homeTypeId = new HomeTypeId("1 bed flat");

        var deliveryPhase1 = new DeliveryPhaseEntityBuilder().WithId("dp-1").WithHomesToBeDelivered(6, homeTypeId.Value).Build();
        var deliveryPhase2 = new DeliveryPhaseEntityBuilder().WithId("dp-2").WithHomesToBeDelivered(4, homeTypeId.Value).Build();
        var testCandidate = new DeliveryPhasesEntityBuilder()
            .WithDeliveryPhase(deliveryPhase1)
            .WithDeliveryPhase(deliveryPhase2)
            .WithHomesToDeliver(new HomesToDeliverBuilder().WithHomeTypeId(homeTypeId.Value).WithTotalHomes(10).Build())
            .Build();

        // when
        var setHomes = () => testCandidate.SetHomesToBeDeliveredInPhase(deliveryPhase2.Id, new[] { new HomesToDeliverInPhase(homeTypeId, 5) });

        // then
        var errors = setHomes.Should().Throw<DomainValidationException>().Which.OperationResult.Errors;
        errors.Should().HaveCount(1);
        errors.Select(x => x.AffectedField).Should().BeEquivalentTo("HomeType-1 bed flat");
    }

    [Fact]
    public void ShouldSetHomesToDeliver_WhenNumberOfHomesInAllPhasesDoesNotExceedTotalNumberOfHomes()
    {
        // given
        var homeType1Id = new HomeTypeId("1 bed flat");
        var homeType2Id = new HomeTypeId("2 bed flat");

        var deliveryPhase1 = new DeliveryPhaseEntityBuilder().WithId("dp-1")
            .WithHomesToBeDelivered(6, homeType1Id.Value)
            .WithHomesToBeDelivered(6, homeType2Id.Value)
            .Build();
        var deliveryPhase2 = new DeliveryPhaseEntityBuilder().WithId("dp-2").WithHomesToBeDelivered(4, homeType1Id.Value).Build();
        var testCandidate = new DeliveryPhasesEntityBuilder()
            .WithDeliveryPhase(deliveryPhase1)
            .WithDeliveryPhase(deliveryPhase2)
            .WithHomesToDeliver(new HomesToDeliverBuilder().WithHomeTypeId(homeType1Id.Value).WithTotalHomes(10).Build())
            .WithHomesToDeliver(new HomesToDeliverBuilder().WithHomeTypeId(homeType2Id.Value).WithTotalHomes(10).Build())
            .Build();

        // when
        testCandidate.SetHomesToBeDeliveredInPhase(
            deliveryPhase1.Id,
            new[]
            {
                new HomesToDeliverInPhase(homeType1Id, 6),
                new HomesToDeliverInPhase(homeType2Id, 10),
            });

        // then
        testCandidate.UnusedHomeTypesCount.Should().Be(0);
    }
}
