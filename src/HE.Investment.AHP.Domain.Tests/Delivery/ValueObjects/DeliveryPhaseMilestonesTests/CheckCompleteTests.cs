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

        // then
        AssertException(Action, "The start on site milestone claim date cannot be before the acquisition milestone claim date");

        // when
        void Action() => milestones.CheckComplete();
    }

    [Fact]
    public void ShouldThrowException_WhenStartOnSitePaymentDateAfterCompletionPaymentDate()
    {
        // given
        var milestones = new DeliveryPhaseMilestonesBuilder()
            .WithStartOnSitePaymentDateAfterCompletionPaymentDate()
            .Build();

        // when
        void Action() => milestones.CheckComplete();

        // then
        AssertException(Action, "The completion milestone claim date cannot be before the start on site milestone claim date");
    }

    [Fact]
    public void ShouldThrowException_WhenAcquisitionPaymentDateAfterCompletionPaymentDate()
    {
        // given
        var milestones = new DeliveryPhaseMilestonesBuilder()
            .WithAcquisitionPaymentDateAfterCompletionPaymentDate()
            .Build();

        // when
        void Action() => milestones.CheckComplete();

        // then
        AssertException(Action, "The completion milestone claim date cannot be before the acquisition milestone claim date");
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
