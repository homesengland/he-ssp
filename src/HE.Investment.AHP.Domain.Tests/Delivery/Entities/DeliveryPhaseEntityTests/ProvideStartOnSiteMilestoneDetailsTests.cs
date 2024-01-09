using FluentAssertions;
using HE.Investment.AHP.Domain.Delivery.ValueObjects;
using HE.Investment.AHP.Domain.Tests.Delivery.Entities.TestDataBuilders;
using HE.Investments.Common.Exceptions;

namespace HE.Investment.AHP.Domain.Tests.Delivery.Entities.DeliveryPhaseEntityTests;

public class ProvideStartOnSiteMilestoneDetailsTests
{
    private readonly StartOnSiteMilestoneDetails _testMilestoneDetails = new(null, new MilestonePaymentDate("1", "2", "3"));

    [Fact]
    public void ShouldThrowException_WhenMilestoneDetailsCannotBeProvided()
    {
        // given
        var testCandidate = new DeliveryPhaseEntityBuilder().WithUnregisteredBody().Build();

        // when
        var action = () => testCandidate.ProvideStartOnSiteMilestoneDetails(_testMilestoneDetails);

        // then
        action.Should().Throw<DomainValidationException>();
    }

    [Fact]
    public void ShouldProvideNullableMilestoneDetails()
    {
        // given
        var testCandidate = new DeliveryPhaseEntityBuilder().Build();

        // when
        testCandidate.ProvideStartOnSiteMilestoneDetails(null);

        // then
        testCandidate.StartOnSiteMilestone.Should().BeNull();
        testCandidate.IsModified.Should().BeFalse();
    }

    [Fact]
    public void ShouldProvideMilestoneDetails()
    {
        // given
        var testCandidate = new DeliveryPhaseEntityBuilder().Build();

        // when
        testCandidate.ProvideStartOnSiteMilestoneDetails(_testMilestoneDetails);

        // then
        testCandidate.StartOnSiteMilestone.Should().Be(_testMilestoneDetails);
        testCandidate.IsModified.Should().BeTrue();
    }

    [Fact]
    public void ShouldDoNothing_WhenProvidedMilestoneDetailsTheSame()
    {
        // given
        var testCandidate = new DeliveryPhaseEntityBuilder().WithStartOnSiteMilestoneDetails(_testMilestoneDetails).Build();

        // when
        testCandidate.ProvideStartOnSiteMilestoneDetails(_testMilestoneDetails);

        // then
        testCandidate.StartOnSiteMilestone.Should().Be(_testMilestoneDetails);
        testCandidate.IsModified.Should().BeFalse();
    }
}
