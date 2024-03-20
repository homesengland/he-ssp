using FluentAssertions;
using HE.Investments.FrontDoor.Domain.Project.ValueObjects;
using HE.Investments.FrontDoor.Domain.Tests.Project.TestDataBuilders;
using Xunit;
using ProjectGeographicFocus = HE.Investments.FrontDoor.Shared.Project.Contract.ProjectGeographicFocus;

namespace HE.Investments.FrontDoor.Domain.Tests.Project.ProjectEntityTests;

public class ProvideIsSiteIdentifiedTests
{
    [Fact]
    public void ShouldChangeIsSiteIdentified_WhenIsSiteIdentifiedIsProvided()
    {
        // given
        var project = ProjectEntityBuilder.New().Build();
        var isSiteIdentified = new IsSiteIdentified(true);

        // when
        project.ProvideIsSiteIdentified(isSiteIdentified);

        // then
        project.IsSiteIdentified.Should().Be(isSiteIdentified);
    }

    [Fact]
    public void ShouldNotChangeIsSiteIdentified_WhenIsSiteIdentifiedIsNotProvided()
    {
        // given
        var project = ProjectEntityBuilder.New().WithNonSiteQuestionFulfilled().Build();

        // when
        project.ProvideIsSiteIdentified(new IsSiteIdentified(true));

        // then
        project.IsSiteIdentified!.Value.Should().BeTrue();
        project.GeographicFocus.GeographicFocus.Should().Be(ProjectGeographicFocus.Undefined);
        project.Regions.IsAnswered().Should().BeFalse();
        project.HomesNumber.Should().BeNull();
    }
}
