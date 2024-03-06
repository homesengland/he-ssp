using FluentAssertions;
using HE.Investments.FrontDoor.Domain.Project.ValueObjects;
using HE.Investments.FrontDoor.Domain.Tests.Project.TestDataBuilders;
using Xunit;

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
}
