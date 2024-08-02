using FluentAssertions;
using HE.Investment.AHP.Contract.Delivery.Enums;
using HE.Investment.AHP.Domain.Delivery.ValueObjects;
using HE.Investment.AHP.Domain.Tests.Delivery.Entities.TestDataBuilders;
using Moq;

namespace HE.Investment.AHP.Domain.Tests.Delivery.Entities.DeliveryPhaseEntityTests;

public class ProvideBuildActivityTests
{
    [Fact]
    public void ShouldResetDependedValues_WhenBuildActivityIsChanged()
    {
        // given
        var builder = new DeliveryPhaseEntityBuilder()
            .WithTypeOfHomes(TypeOfHomes.Rehab)
            .WithRehabBuildActivity(BuildActivityType.Reimprovement)
            .WithAcquisitionMilestone(new AcquisitionMilestoneDetailsBuilder().Build())
            .WithStartOnSiteMilestone(new StartOnSiteMilestoneDetailsBuilder().Build());

        var testCandidate = builder.Build();

        var newBuildActivity = new BuildActivity(testCandidate.Application.Tenure, TypeOfHomes.Rehab, BuildActivityType.ExistingSatisfactory);

        // when
        testCandidate.ProvideBuildActivity(newBuildActivity);

        // then
        testCandidate.TypeOfHomes.Should().Be(TypeOfHomes.Rehab);
        testCandidate.BuildActivity.Should().Be(newBuildActivity);
        builder.MockOnlyCompletionMilestonePolicy
            .Verify(x => x.IsOnlyCompletionMilestone(It.IsAny<bool>(), It.IsAny<BuildActivity>()), Times.Exactly(3));
    }

    [Fact]
    public void ShouldNotResetDependedValues_WhenBuildActivityIsNotChanged()
    {
        // given
        var testCandidate = new DeliveryPhaseEntityBuilder()
            .WithTypeOfHomes(TypeOfHomes.Rehab)
            .WithRehabBuildActivity(BuildActivityType.Reimprovement)
            .WithAcquisitionMilestone(new AcquisitionMilestoneDetailsBuilder().Build())
            .WithStartOnSiteMilestone(new StartOnSiteMilestoneDetailsBuilder().Build())
            .Build();

        // when
        testCandidate.ProvideBuildActivity(testCandidate.BuildActivity);

        // then
        testCandidate.DeliveryPhaseMilestones.AcquisitionMilestone.Should().NotBeNull();
        testCandidate.DeliveryPhaseMilestones.StartOnSiteMilestone.Should().NotBeNull();
    }
}
