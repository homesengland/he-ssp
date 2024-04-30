using FluentAssertions;
using HE.Investment.AHP.Domain.Delivery.ValueObjects;
using HE.Investment.AHP.Domain.Tests.Delivery.Entities.TestDataBuilders;
using HE.Investments.Common.Contract.Exceptions;

namespace HE.Investment.AHP.Domain.Tests.Delivery.ValueObjects.DeliveryPhaseMilestonesTests;

public class CreateTests
{
    [Fact]
    public void ShouldThrowException_WhenAcquisitionMilestoneDetailsProvidedAndIsOnlyCompletionMilestone()
    {
        // given && when
        DeliveryPhaseMilestones Action() =>
            new DeliveryPhaseMilestonesBuilder().WithIsOnlyCompletionMilestone()
                .Build();

        // then
        AssertException(Action, "Cannot provide Acquisition Milestone details.");
    }

    [Fact]
    public void ShouldThrowException_WhenStartOnSiteMilestoneDetailsProvidedAndIsOnlyCompletionMilestone()
    {
        // given && when
        DeliveryPhaseMilestones Action() =>
            new DeliveryPhaseMilestonesBuilder().WithIsOnlyCompletionMilestone()
                .WithoutAcquisitionMilestoneDetails()
                .Build();

        // then
        AssertException(Action, "Cannot provide Start On Site Milestone details.");
    }

    private static void AssertException(Func<DeliveryPhaseMilestones> action, string errorMessage)
    {
        action
            .Should()
            .ThrowExactly<DomainValidationException>()
            .Which.OperationResult.Errors.Should()
            .ContainSingle(x => x.ErrorMessage == errorMessage);
    }
}
