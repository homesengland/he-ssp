using FluentAssertions;
using HE.Investments.FrontDoor.Domain.Project.ValueObjects;
using HE.Investments.FrontDoor.Domain.Tests.Project.TestDataBuilders;
using HE.Investments.FrontDoor.Shared.Project.Contract;
using Xunit;

namespace HE.Investments.FrontDoor.Domain.Tests.Project.ProjectEntityTests;

public class ProvideRegionsTests
{
    [Fact]
    public void ShouldChangeRegions_WhenRegionsAreProvided()
    {
        // given
        var project = ProjectEntityBuilder.New().Build();
        var regions = new Regions([RegionType.London, RegionType.NorthWest]);

        // when
        project.ProvideRegions(regions);

        // then
        project.Regions.Should().Be(regions);
    }
}
