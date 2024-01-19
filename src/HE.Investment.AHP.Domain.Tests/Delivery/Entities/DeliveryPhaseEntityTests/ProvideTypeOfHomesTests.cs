using FluentAssertions;
using HE.Investment.AHP.Contract.Delivery.Enums;
using HE.Investment.AHP.Domain.Tests.Delivery.Entities.TestDataBuilders;
using HE.Investments.Common.Contract.Exceptions;

namespace HE.Investment.AHP.Domain.Tests.Delivery.Entities.DeliveryPhaseEntityTests;

public class ProvideTypeOfHomesTests
{
    [Fact]
    public void ShouldResetDependedValues_WhenTypeOfHomesIsChanged()
    {
        // given
        var testCandidate = new DeliveryPhaseEntityBuilder()
            .WithTypeOfHomes(TypeOfHomes.Rehab)
            .WithRehabBuildActivity()
            .WithDeliveryPhaseMilestones()
            .WithReconfiguringExisting()
            .Build();

        // when
        testCandidate.ProvideTypeOfHomes(TypeOfHomes.NewBuild);

        // then
        testCandidate.TypeOfHomes.Should().Be(TypeOfHomes.NewBuild);
        testCandidate.BuildActivity.IsAnswered().Should().BeFalse();
        testCandidate.ReconfiguringExisting.Should().BeNull();
    }

    [Fact]
    public void ShouldNotResetDependedValues_WhenTypeOfHomesIsNotChanged()
    {
        // given
        var testCandidate = new DeliveryPhaseEntityBuilder()
                    .WithTypeOfHomes(TypeOfHomes.Rehab)
                    .WithRehabBuildActivity()
                    .WithDeliveryPhaseMilestones()
                    .WithReconfiguringExisting()
                    .Build();

        // when
        testCandidate.ProvideTypeOfHomes(TypeOfHomes.Rehab);

        // then
        testCandidate.TypeOfHomes.Should().Be(TypeOfHomes.Rehab);
        testCandidate.BuildActivity.IsAnswered().Should().BeTrue();
        testCandidate.ReconfiguringExisting.Should().NotBeNull();
    }
}
