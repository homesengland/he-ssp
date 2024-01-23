using FluentAssertions;
using HE.Investment.AHP.Domain.Delivery.ValueObjects;
using HE.Investment.AHP.Domain.Tests.Delivery.Entities.TestDataBuilders;
using HE.Investments.Common.Contract.Exceptions;

namespace HE.Investment.AHP.Domain.Tests.Delivery.ValueObjects.DeliveryPhaseMilestonesTests;

public class CreateTests
{
    [Fact]
    public void ShouldThrowException_WhenAcquisitionMilestoneDetailsProvidedForUnregisteredBody()
    {
        // given && when
        var action = () => new DeliveryPhaseMilestonesBuilder()
            .WithUnregisteredBody()
            .Build();

        // then
        AssertException(action, "Cannot provide Acquisition Milestone details.");
    }

    [Fact]
    public void ShouldThrowException_WhenAcquisitionMilestoneDetailsProvidedFor()
    {
        // given && when
        var action = () => new DeliveryPhaseMilestonesBuilder()
            .WithBuildActivityOnlyForCompletionMilestone()
            .Build();

        // then
        AssertException(action, "Cannot provide Acquisition Milestone details.");
    }

    [Fact]
    public void ShouldThrowException_WhenStartOnSiteMilestoneDetailsProvidedForUnregisteredBody()
    {
        // given && when
        var action = () => new DeliveryPhaseMilestonesBuilder()
            .WithUnregisteredBody()
            .WithoutAcquisitionMilestoneDetails()
            .Build();

        // then
        AssertException(action, "Cannot provide Start On Site Milestone details.");
    }

    [Fact]
    public void ShouldThrowException_WhenStartOnSiteMilestoneDetailsProvidedFor()
    {
        // given && when
        var action = () => new DeliveryPhaseMilestonesBuilder()
            .WithoutAcquisitionMilestoneDetails()
            .WithBuildActivityOnlyForCompletionMilestone()
            .Build();

        // then
        AssertException(action, "Cannot provide Start On Site Milestone details.");
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
