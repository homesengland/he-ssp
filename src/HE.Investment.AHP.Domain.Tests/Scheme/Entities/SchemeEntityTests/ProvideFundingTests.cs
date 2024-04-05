using FluentAssertions;
using HE.Investment.AHP.Contract.Scheme.Events;
using HE.Investment.AHP.Domain.Scheme.ValueObjects;
using HE.Investment.AHP.Domain.Tests.Scheme.TestObjectBuilders;
using HE.Investments.Common.Contract;

namespace HE.Investment.AHP.Domain.Tests.Scheme.Entities.SchemeEntityTests;

public class ProvideFundingTests
{
    [Fact]
    public void ShouldSetNewFundingAndPublishTwoDomainEvents()
    {
        // given
        var testCandidate = SchemeEntityBuilder.NewNotStarted().Build();
        var funding = new SchemeFunding(1_000_000, 10);

        // when
        testCandidate.ProvideFunding(funding);

        // then
        testCandidate.Funding.RequiredFunding.Should().Be(funding.RequiredFunding);
        testCandidate.Funding.HousesToDeliver.Should().Be(funding.HousesToDeliver);
        testCandidate.Status.Should().Be(SectionStatus.InProgress);
        testCandidate.GetDomainEventsAndRemove()
            .Should()
            .HaveCount(2)
            .And.Contain(new SchemeNumberOfHomesHasBeenUpdatedEvent(testCandidate.Application.Id))
            .And.Contain(new SchemeFundingHasBeenChangedEvent(testCandidate.Application.Id));
    }

    [Fact]
    public void ShouldNotPublishEvents_WhenNewFundingIsTheSameAsOldOne()
    {
        // given
        var funding = new SchemeFunding(1_000_000, 10);
        var testCandidate = SchemeEntityBuilder.NewNotStarted().WithSchemeFunding(funding).Build();

        // when
        testCandidate.ProvideFunding(funding);

        // then
        testCandidate.Funding.RequiredFunding.Should().Be(funding.RequiredFunding);
        testCandidate.Funding.HousesToDeliver.Should().Be(funding.HousesToDeliver);
        testCandidate.GetDomainEventsAndRemove()
            .Should()
            .BeEmpty();
    }

    [Fact]
    public void ShouldSetNewFundingAndPublishTwoDomainEvents_WhenNewFundingIsDifferent()
    {
        // given
        var funding = new SchemeFunding(1_000_000, 20);
        var testCandidate = SchemeEntityBuilder.NewNotStarted().WithSchemeFunding(new SchemeFunding(2_000_000, 20)).Build();

        // when
        testCandidate.ProvideFunding(funding);

        // then
        testCandidate.Funding.RequiredFunding.Should().Be(funding.RequiredFunding);
        testCandidate.Funding.HousesToDeliver.Should().Be(funding.HousesToDeliver);
        testCandidate.Status.Should().Be(SectionStatus.InProgress);
        testCandidate.GetDomainEventsAndRemove()
            .Should()
            .HaveCount(1)
            .And.Contain(new SchemeFundingHasBeenChangedEvent(testCandidate.Application.Id));
    }
}
