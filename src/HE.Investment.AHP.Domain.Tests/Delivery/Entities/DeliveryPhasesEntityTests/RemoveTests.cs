using FluentAssertions;
using HE.Investment.AHP.Contract.Delivery;
using HE.Investment.AHP.Contract.Delivery.Enums;
using HE.Investment.AHP.Domain.Delivery.Entities;
using HE.Investment.AHP.Domain.Delivery.ValueObjects;
using HE.Investment.AHP.Domain.Tests.Delivery.Entities.TestDataBuilders;
using HE.Investments.Common.Contract.Exceptions;

namespace HE.Investment.AHP.Domain.Tests.Delivery.Entities.DeliveryPhasesEntityTests;

public class RemoveTests
{
    [Fact]
    public void ShouldThrowException_WhenDeliveryPhaseDoesNotExist()
    {
        // given
        var deliveryPhase = new DeliveryPhaseEntityBuilder().WithId("dp-1").Build();
        var testCandidate = new DeliveryPhasesEntityBuilder().WithDeliveryPhase(deliveryPhase).Build();

        // when
        var remove = () => testCandidate.Remove(new DeliveryPhaseId("dp-2"), RemoveDeliveryPhaseAnswer.Yes);

        // then
        remove.Should().Throw<NotFoundException>().Which.EntityName.Should().Be(nameof(DeliveryPhaseEntity));
    }

    [Fact]
    public void ShouldThrowException_WhenAnswerIsUndefined()
    {
        // given
        var deliveryPhase = new DeliveryPhaseEntityBuilder().WithId("dp-1").Build();
        var testCandidate = new DeliveryPhasesEntityBuilder().WithDeliveryPhase(deliveryPhase).Build();

        // when
        var remove = () => testCandidate.Remove(deliveryPhase.Id, RemoveDeliveryPhaseAnswer.Undefined);

        // then
        var errors = remove.Should().Throw<DomainValidationException>().Which.OperationResult.Errors;
        errors.Should().HaveCount(1);
        errors.Single().AffectedField.Should().Be(nameof(RemoveDeliveryPhaseAnswer));
    }

    [Fact]
    public void ShouldDoNothing_WhenAnswerIsNo()
    {
        // given
        var deliveryPhase = new DeliveryPhaseEntityBuilder().WithId("dp-1").Build();
        var testCandidate = new DeliveryPhasesEntityBuilder().WithDeliveryPhase(deliveryPhase).Build();

        // when
        testCandidate.Remove(deliveryPhase.Id, RemoveDeliveryPhaseAnswer.No);

        // then
        testCandidate.DeliveryPhases.Should().HaveCount(1);
        testCandidate.DeliveryPhases.Single().Should().Be(deliveryPhase);
        testCandidate.ToRemove.Should().BeEmpty();
    }

    [Fact]
    public void ShouldRemoveDeliveryPhase_WhenAnswerIsYes()
    {
        // given
        var deliveryPhase = new DeliveryPhaseEntityBuilder().WithId("dp-1").Build();
        var testCandidate = new DeliveryPhasesEntityBuilder().WithDeliveryPhase(deliveryPhase).Build();

        // when
        testCandidate.Remove(deliveryPhase.Id, RemoveDeliveryPhaseAnswer.Yes);

        // then
        testCandidate.DeliveryPhases.Should().BeEmpty();
        testCandidate.ToRemove.Should().HaveCount(1);
        testCandidate.ToRemove.Single().Should().Be(deliveryPhase);
    }
}
