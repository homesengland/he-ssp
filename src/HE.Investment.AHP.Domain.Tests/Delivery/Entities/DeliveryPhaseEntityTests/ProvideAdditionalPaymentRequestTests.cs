using FluentAssertions;
using HE.Investment.AHP.Domain.Delivery.ValueObjects;
using HE.Investment.AHP.Domain.Tests.Delivery.Entities.TestDataBuilders;
using HE.Investments.Common.Exceptions;

namespace HE.Investment.AHP.Domain.Tests.Delivery.Entities.DeliveryPhaseEntityTests;

public class ProvideAdditionalPaymentRequestTests
{
    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void ShouldThrowException_WhenAdditionalPaymentRequestCannotBeProvided(bool isAdditionalPaymentRequested)
    {
        // given
        var isPaymentRequested = new IsAdditionalPaymentRequested(isAdditionalPaymentRequested);
        var testCandidate = new DeliveryPhaseEntityBuilder().Build();

        // when
        var action = () => testCandidate.ProvideAdditionalPaymentRequest(isPaymentRequested);

        // then
        action.Should().Throw<DomainValidationException>();
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void ShouldProvideAdditionalPaymentRequest_WhenUnregisteredBodyRequested(bool isAdditionalPaymentRequested)
    {
        // given
        var isPaymentRequested = new IsAdditionalPaymentRequested(isAdditionalPaymentRequested);
        var testCandidate = new DeliveryPhaseEntityBuilder().WithUnregisteredBody().Build();

        // when
        testCandidate.ProvideAdditionalPaymentRequest(isPaymentRequested);

        // then
        testCandidate.IsAdditionalPaymentRequested.Should().Be(isPaymentRequested);
        testCandidate.IsModified.Should().BeTrue();
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void ShouldDoNothing_WhenProvidedValueNotChanged(bool isAdditionalPaymentRequested)
    {
        // given
        var isPaymentRequested = new IsAdditionalPaymentRequested(isAdditionalPaymentRequested);
        var testCandidate = new DeliveryPhaseEntityBuilder().WithUnregisteredBody().WithAdditionalPaymentRequested(isPaymentRequested).Build();

        // when
        testCandidate.ProvideAdditionalPaymentRequest(isPaymentRequested);

        // then
        testCandidate.IsAdditionalPaymentRequested.Should().Be(isPaymentRequested);
        testCandidate.IsModified.Should().BeFalse();
    }
}
