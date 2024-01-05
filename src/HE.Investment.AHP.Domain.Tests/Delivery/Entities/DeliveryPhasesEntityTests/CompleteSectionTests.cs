using FluentAssertions;
using HE.Investment.AHP.Contract.Common.Enums;
using HE.Investment.AHP.Domain.HomeTypes.ValueObjects;
using HE.Investment.AHP.Domain.Tests.Delivery.Entities.TestDataBuilders;
using HE.Investments.Common.Contract;
using HE.Investments.Common.Exceptions;

namespace HE.Investment.AHP.Domain.Tests.Delivery.Entities.DeliveryPhasesEntityTests;

public class CompleteSectionTests
{
    [Fact]
    public void ShouldThrowException_WhenAnswerIsUndefined()
    {
        // given
        var deliveryPhase = new DeliveryPhaseEntityBuilder().WithStatus(SectionStatus.Completed).Build();
        var testCandidate = new DeliveryPhasesEntityBuilder().WithDeliveryPhase(deliveryPhase).Build();

        // when
        var complete = () => testCandidate.CompleteSection(IsSectionCompleted.Undefied);

        // then
        var errors = complete.Should().Throw<DomainValidationException>().Which.OperationResult.Errors;
        errors.Should().HaveCount(1);
        errors.Single().AffectedField.Should().Be(nameof(IsSectionCompleted));
    }

    [Fact]
    public void ShouldThrowException_WhenAnswerIsYesAndThereAreNoDeliveryPhases()
    {
        // given
        var testCandidate = new DeliveryPhasesEntityBuilder().Build();

        // when
        var complete = () => testCandidate.CompleteSection(IsSectionCompleted.Yes);

        // then
        var errors = complete.Should().Throw<DomainValidationException>().Which.OperationResult.Errors;
        errors.Should().HaveCount(1);
        errors.Single().AffectedField.Should().Be("DeliveryPhases");
    }

    [Fact]
    public void ShouldThrowException_WhenAnswerIsYesAndOnePhaseIsNotCompleted()
    {
        // given
        var deliveryPhase1 = new DeliveryPhaseEntityBuilder().WithId("dp-1").WithStatus(SectionStatus.InProgress).Build();
        var deliveryPhase2 = new DeliveryPhaseEntityBuilder().WithStatus(SectionStatus.Completed).Build();
        var testCandidate = new DeliveryPhasesEntityBuilder()
            .WithStatus(SectionStatus.InProgress)
            .WithDeliveryPhase(deliveryPhase1)
            .WithDeliveryPhase(deliveryPhase2)
            .Build();

        // when
        var complete = () => testCandidate.CompleteSection(IsSectionCompleted.Yes);

        // then
        var errors = complete.Should().Throw<DomainValidationException>().Which.OperationResult.Errors;
        errors.Should().HaveCount(1);
        errors.Single().AffectedField.Should().Be("DeliveryPhase-dp-1");

        testCandidate.Status.Should().Be(SectionStatus.InProgress);
    }

    [Fact]
    public void ShouldThrowException_WhenNotAllHomeTypesAreUsed()
    {
        // given
        var homeType1Id = new HomeTypeId("1 bed flat");
        var homeType2Id = new HomeTypeId("2 bed flat");
        var deliveryPhase1 = new DeliveryPhaseEntityBuilder()
            .WithHomesToBeDelivered(homeType1Id.Value, 5)
            .WithHomesToBeDelivered(homeType2Id.Value, 5)
            .WithStatus(SectionStatus.Completed).Build();
        var deliveryPhase2 = new DeliveryPhaseEntityBuilder()
            .WithHomesToBeDelivered(homeType1Id.Value, 5)
            .WithStatus(SectionStatus.Completed)
            .Build();
        var testCandidate = new DeliveryPhasesEntityBuilder()
            .WithStatus(SectionStatus.InProgress)
            .WithDeliveryPhase(deliveryPhase1)
            .WithDeliveryPhase(deliveryPhase2)
            .WithHomesToDeliver(new HomesToDeliverBuilder().WithHomeTypeId(homeType1Id.Value).WithTotalHomes(10).Build())
            .WithHomesToDeliver(new HomesToDeliverBuilder().WithHomeTypeId(homeType2Id.Value).WithTotalHomes(10).Build())
            .Build();

        // when
        var complete = () => testCandidate.CompleteSection(IsSectionCompleted.Yes);

        // then
        var errors = complete.Should().Throw<DomainValidationException>().Which.OperationResult.Errors;
        errors.Should().HaveCount(1);
        errors.Single().AffectedField.Should().Be("DeliveryPhases");

        testCandidate.Status.Should().Be(SectionStatus.InProgress);
    }

    [Fact]
    public void ShouldThrowException_WhenTooManyHomeTypesAreUsed()
    {
        // given
        var homeTypeId = new HomeTypeId("1 bed flat");
        var deliveryPhase = new DeliveryPhaseEntityBuilder()
            .WithHomesToBeDelivered(homeTypeId.Value, 11)
            .WithStatus(SectionStatus.Completed)
            .Build();
        var testCandidate = new DeliveryPhasesEntityBuilder()
            .WithStatus(SectionStatus.InProgress)
            .WithDeliveryPhase(deliveryPhase)
            .WithHomesToDeliver(new HomesToDeliverBuilder().WithHomeTypeId(homeTypeId.Value).WithTotalHomes(10).Build())
            .Build();

        // when
        var complete = () => testCandidate.CompleteSection(IsSectionCompleted.Yes);

        // then
        var errors = complete.Should().Throw<DomainValidationException>().Which.OperationResult.Errors;
        errors.Should().HaveCount(1);
        errors.Single().AffectedField.Should().Be("DeliveryPhases");

        testCandidate.Status.Should().Be(SectionStatus.InProgress);
    }

    [Fact]
    public void ShouldChangeStatusToCompleted_WhenAnswerIsYesAndAllDeliveryPhasesAreUsed()
    {
        // given
        var homeTypeId = new HomeTypeId("1 bed flat");
        var deliveryPhase = new DeliveryPhaseEntityBuilder()
            .WithHomesToBeDelivered(homeTypeId.Value, 10)
            .WithStatus(SectionStatus.Completed)
            .Build();
        var testCandidate = new DeliveryPhasesEntityBuilder()
            .WithStatus(SectionStatus.InProgress)
            .WithDeliveryPhase(deliveryPhase)
            .WithHomesToDeliver(new HomesToDeliverBuilder().WithHomeTypeId(homeTypeId.Value).WithTotalHomes(10).Build())
            .Build();

        // when
        testCandidate.CompleteSection(IsSectionCompleted.Yes);

        // then
        testCandidate.Status.Should().Be(SectionStatus.Completed);
    }

    [Fact]
    public void ShouldMarkDeliveryAsInProgress_WhenAnswerIsNo()
    {
        // given
        var homeTypeId = new HomeTypeId("1 bed flat");
        var deliveryPhase = new DeliveryPhaseEntityBuilder()
            .WithHomesToBeDelivered(homeTypeId.Value, 10)
            .WithStatus(SectionStatus.Completed)
            .Build();
        var testCandidate = new DeliveryPhasesEntityBuilder()
            .WithStatus(SectionStatus.Completed)
            .WithDeliveryPhase(deliveryPhase)
            .WithHomesToDeliver(new HomesToDeliverBuilder().WithHomeTypeId(homeTypeId.Value).WithTotalHomes(10).Build())
            .Build();

        // when
        testCandidate.CompleteSection(IsSectionCompleted.No);

        // then
        testCandidate.Status.Should().Be(SectionStatus.InProgress);
    }
}
