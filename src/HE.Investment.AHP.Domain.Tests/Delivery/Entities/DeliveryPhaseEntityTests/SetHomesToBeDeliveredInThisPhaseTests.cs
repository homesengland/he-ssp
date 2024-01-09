using FluentAssertions;
using HE.Investment.AHP.Contract.HomeTypes;
using HE.Investment.AHP.Domain.Delivery.Entities;
using HE.Investment.AHP.Domain.HomeTypes.ValueObjects;
using HE.Investment.AHP.Domain.Tests.Delivery.Entities.TestDataBuilders;
using HE.Investments.Common.Contract;

namespace HE.Investment.AHP.Domain.Tests.Delivery.Entities.DeliveryPhaseEntityTests;

public class SetHomesToBeDeliveredInThisPhaseTests
{
    [Fact]
    public void ShouldThrowException_WhenTheSameHomeTypeIsUsedMultipleTimes()
    {
        // given
        var homesToDeliver = new[]
        {
            new HomesToDeliverInPhase(new HomeTypeId("ht-1"), 1),
            new HomesToDeliverInPhase(new HomeTypeId("ht-1"), 2),
        };
        var testCandidate = new DeliveryPhaseEntityBuilder().Build();

        // when
        var setHomes = () => testCandidate.SetHomesToBeDeliveredInThisPhase(homesToDeliver);

        // then
        setHomes.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void ShouldUpdateCollection_WhenValidHomesToDeliverAreGiven()
    {
        // given
        var homesToDeliver = new[]
        {
            new HomesToDeliverInPhase(new HomeTypeId("ht-1"), 9),
            new HomesToDeliverInPhase(new HomeTypeId("ht-3"), 1),
        };
        var testCandidate = new DeliveryPhaseEntityBuilder()
            .WithHomesToBeDelivered("ht-1", 10)
            .WithHomesToBeDelivered("ht-2", 2)
            .WithStatus(SectionStatus.Completed)
            .Build();

        // when
        testCandidate.SetHomesToBeDeliveredInThisPhase(homesToDeliver);

        // then
        testCandidate.HomesToDeliver.Should().BeEquivalentTo(homesToDeliver);
        testCandidate.IsModified.Should().BeTrue();
    }

    [Fact]
    public void ShouldNotMarkEntityAsModified_WhenTheSameHomeTypesAreGiven()
    {
        // given
        var homesToDeliver = new[]
        {
            new HomesToDeliverInPhase(new HomeTypeId("ht-1"), 1),
            new HomesToDeliverInPhase(new HomeTypeId("ht-2"), 2),
        };
        var testCandidate = new DeliveryPhaseEntityBuilder()
            .WithHomesToBeDelivered("ht-1", 1)
            .WithHomesToBeDelivered("ht-2", 2)
            .WithStatus(SectionStatus.Completed)
            .Build();

        // when
        testCandidate.SetHomesToBeDeliveredInThisPhase(homesToDeliver);

        // then
        testCandidate.HomesToDeliver.Should().BeEquivalentTo(homesToDeliver);
        testCandidate.IsModified.Should().BeFalse();
    }
}
