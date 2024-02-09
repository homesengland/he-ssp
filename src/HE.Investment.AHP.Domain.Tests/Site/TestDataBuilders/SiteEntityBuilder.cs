using HE.Investment.AHP.Contract.Site.Enums;
using HE.Investment.AHP.Domain.Site.Entities;
using HE.Investment.AHP.Domain.Site.ValueObjects;
using HE.Investments.TestsUtils;

namespace HE.Investment.AHP.Domain.Tests.Site.TestDataBuilders;

public class SiteEntityBuilder
{
    private readonly SiteEntity _item;

    private SiteEntityBuilder()
    {
        _item = new SiteEntity();
    }

    public static SiteEntityBuilder New() => new();

    public SiteEntityBuilder WithName(string name)
    {
        PrivatePropertySetter.SetPropertyWithNoSetter(
            _item,
            nameof(_item.Name),
            new SiteName(name));

        return this;
    }

    public SiteEntityBuilder WithBuildingForHealthyLife(BuildingForHealthyLifeType buildingForHealthyLife)
    {
        PrivatePropertySetter.SetPropertyWithNoSetter(
            _item,
            nameof(_item.BuildingForHealthyLife),
            buildingForHealthyLife);

        return this;
    }

    public SiteEntityBuilder WithNumberOfGreenLights(NumberOfGreenLights? numberOfGreenLights)
    {
        PrivatePropertySetter.SetPropertyWithNoSetter(
            _item,
            nameof(_item.NumberOfGreenLights),
            numberOfGreenLights);

        return this;
    }

    public SiteEntity Build() => _item;
}
