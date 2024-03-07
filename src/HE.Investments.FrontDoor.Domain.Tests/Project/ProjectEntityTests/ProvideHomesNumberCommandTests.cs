using FluentAssertions;
using HE.Investments.FrontDoor.Domain.Project.ValueObjects;
using HE.Investments.FrontDoor.Domain.Tests.Project.TestDataBuilders;
using Xunit;

namespace HE.Investments.FrontDoor.Domain.Tests.Project.ProjectEntityTests;

public class ProvideHomesNumberCommandTests
{
    [Fact]
    public void ShouldChangeHomesNumber_WhenHomesNumberIsProvided()
    {
        // given
        var project = ProjectEntityBuilder.New().Build();
        var homesNumber = new HomesNumber("50");

        // when
        project.ProvideHomesNumber(homesNumber);

        // then
        project.HomesNumber.Should().Be(homesNumber);
    }
}
