using FluentAssertions;
using HE.Investment.AHP.Domain.Tests.Delivery.Entities.TestDataBuilders;
using HE.Investments.Common.Contract.Exceptions;

namespace HE.Investment.AHP.Domain.Tests.Delivery.ValueObjects.DeliveryPhaseMilestonesTests;

public class CheckCompleteTests
{
    [Fact]
    public void ShouldThrowException_WhenAcquisitionPaymentDateAfterStartOnSitePaymentDate()
    {
        // given
        var milestones = new DeliveryPhaseMilestonesBuilder()
            .WithAcquisitionPaymentDateAfterStartOnSitePaymentDate()
            .Build();

        // when
        var action = () => milestones.CheckComplete();

        // then
        AssertException(action, "The start on site milestone claim date cannot be before the acquisition milestone claim date");
    }

    [Fact]
    public void ShouldThrowException_WhenStartOnSitePaymentDateAfterCompletionPaymentDate()
    {
        // given
        var milestones = new DeliveryPhaseMilestonesBuilder()
            .WithStartOnSitePaymentDateAfterCompletionPaymentDate()
            .Build();

        // when
        var action = () => milestones.CheckComplete();

        // then
        AssertException(action, "The completion milestone claim date cannot be before the start on site milestone claim date");
    }

    [Fact]
    public void ShouldThrowException_WhenAcquisitionPaymentDateAfterCompletionPaymentDate()
    {
        // given
        var milestones = new DeliveryPhaseMilestonesBuilder()
            .WithAcquisitionPaymentDateAfterCompletionPaymentDate()
            .Build();

        // when
        var action = () => milestones.CheckComplete();

        // then
        AssertException(action, "The completion milestone claim date cannot be before the acquisition milestone claim date");
    }

    private static void AssertException(Action action, string errorMessage)
    {
        action
            .Should()
            .ThrowExactly<DomainValidationException>()
            .Which.OperationResult.Errors.Should()
            .ContainSingle(x => x.ErrorMessage == errorMessage);
    }
}
