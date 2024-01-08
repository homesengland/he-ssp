using FluentAssertions;
using HE.Investment.AHP.Domain.Delivery.ValueObjects;
using HE.Investment.AHP.Domain.Tests.Delivery.Entities.TestDataBuilders;

namespace HE.Investment.AHP.Domain.Tests.Delivery.Entities.DeliveryPhaseEntityTests;

public class ProvideAcquisitionMilestoneDetailsTests
{
    private readonly AcquisitionMilestoneDetails _testAcquisitionMilestoneDetails = new(new AcquisitionDate("1", "2", "3"), null);

    [Fact]
    public void ShouldThrowException_WhenMilestoneDetailsCannotBeProvided()
    {
        // given
        var testCandidate = new DeliveryPhaseEntityBuilder().WithUnregisteredBody().Build();

        // when
        var action = () => testCandidate.ProvideAcquisitionMilestoneDetails(_testAcquisitionMilestoneDetails);

        // then
        action.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void ShouldProvideNullableMilestoneDetails()
    {
        // given
        var testCandidate = new DeliveryPhaseEntityBuilder().Build();

        // when
        testCandidate.ProvideAcquisitionMilestoneDetails(null);

        // then
        testCandidate.AcquisitionMilestone.Should().BeNull();
        testCandidate.IsModified.Should().BeFalse();
    }

    [Fact]
    public void ShouldProvideMilestoneDetails()
    {
        // given
        var testCandidate = new DeliveryPhaseEntityBuilder().Build();

        // when
        testCandidate.ProvideAcquisitionMilestoneDetails(_testAcquisitionMilestoneDetails);

        // then
        testCandidate.AcquisitionMilestone.Should().Be(_testAcquisitionMilestoneDetails);
        testCandidate.IsModified.Should().BeTrue();
    }

    [Fact]
    public void ShouldDoNothing_WhenProvidedMilestoneDetailsTheSame()
    {
        // given
        var testCandidate = new DeliveryPhaseEntityBuilder().WithAcquisitionMilestoneDetails(_testAcquisitionMilestoneDetails).Build();

        // when
        testCandidate.ProvideAcquisitionMilestoneDetails(_testAcquisitionMilestoneDetails);

        // then
        testCandidate.AcquisitionMilestone.Should().Be(_testAcquisitionMilestoneDetails);
        testCandidate.IsModified.Should().BeFalse();
    }
}
