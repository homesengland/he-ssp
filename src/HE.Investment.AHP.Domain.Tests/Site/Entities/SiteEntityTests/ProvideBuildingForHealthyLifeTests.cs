using FluentAssertions;
using HE.Investment.AHP.Contract.Site.Enums;
using HE.Investment.AHP.Domain.Site.ValueObjects;
using HE.Investment.AHP.Domain.Tests.Site.TestDataBuilders;

namespace HE.Investment.AHP.Domain.Tests.Site.Entities.SiteEntityTests;

public class ProvideBuildingForHealthyLifeTests
{
    [Fact]
    public void ShouldSaveBuildingForHealthyLife_WhenBuildingForHealthyLifeIsProvided()
    {
        // given
        var buildingForHealthyLife = BuildingForHealthyLifeType.Undefined;
        var siteEntity = SiteEntityBuilder.New().WithBuildingForHealthyLife(buildingForHealthyLife).Build();

        // when
        siteEntity.ProvideBuildingForHealthyLife(buildingForHealthyLife);

        // when
        siteEntity.BuildingForHealthyLife.Should().Be(buildingForHealthyLife);
    }

    [Fact]
    public void ShouldClearNumberOfGreenLightsValue_WhenBuildingForHealthyLifeIsChangedFromYes()
    {
        // given
        var buildingForHealthyLife = BuildingForHealthyLifeType.Yes;
        var numberOfGreenLights = new NumberOfGreenLights("5");
        var siteEntity = SiteEntityBuilder
            .New()
            .WithBuildingForHealthyLife(buildingForHealthyLife)
            .WithNumberOfGreenLights(numberOfGreenLights)
            .Build();
        var newBuildingForHealthyLife = BuildingForHealthyLifeType.No;

        // when
        siteEntity.ProvideBuildingForHealthyLife(newBuildingForHealthyLife);

        // when
        siteEntity.BuildingForHealthyLife.Should().Be(newBuildingForHealthyLife);
        siteEntity.NumberOfGreenLights.Should().BeNull();
    }
}
