using FluentAssertions;
using HE.Investment.AHP.Contract.HomeTypes.Events;
using HE.Investment.AHP.Domain.Tests.HomeTypes.TestDataBuilders;

namespace HE.Investment.AHP.Domain.Tests.HomeTypes.Entities.HomeTypeEntityTests;

public class ChangeNumberOfHomesTests
{
    [Fact]
    public void ShouldPublishDomainEvent_WhenNumberOfHomesWasChanged()
    {
        // given
        var testCandidate = new HomeTypeEntityBuilder().WithSegments(new HomeInformationBuilder().Build()).Build();

        // when
        testCandidate.HomeInformation.ChangeNumberOfHomes("10");

        // then
        testCandidate.HomeInformation.NumberOfHomes.Should().NotBeNull();
        testCandidate.HomeInformation.NumberOfHomes!.Value.Should().Be(10);
        testCandidate.IsModified.Should().BeTrue();
        testCandidate.GetDomainEventsAndRemove()
            .Should()
            .HaveCount(1)
            .And.Subject.Single()
            .Should()
            .Be(new HomeTypeNumberOfHomesHasBeenUpdatedEvent(testCandidate.Application.Id));
    }

    [Fact]
    public void ShouldNotChangeEntity_WhenNumberOfHomesIsTheSame()
    {
        // given
        var testCandidate = new HomeTypeEntityBuilder().WithSegments(new HomeInformationBuilder().WithNumberOfHomes(10).Build()).Build();

        // when
        testCandidate.HomeInformation.ChangeNumberOfHomes("10");

        // then
        testCandidate.HomeInformation.NumberOfHomes.Should().NotBeNull();
        testCandidate.HomeInformation.NumberOfHomes!.Value.Should().Be(10);
        testCandidate.IsModified.Should().BeFalse();
        testCandidate.GetDomainEventsAndRemove().Should().BeEmpty();
    }
}
