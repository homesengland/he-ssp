using HE.Investment.AHP.Contract.Site;
using HE.Investments.AHP.ProjectDashboard.Domain.Project.ValueObjects;
using HE.Investments.FrontDoor.Shared.Project;
using HE.Investments.Organisation.LocalAuthorities.ValueObjects;
using LocalAuthority = HE.Investments.Organisation.LocalAuthorities.ValueObjects.LocalAuthority;

namespace HE.Investments.AHP.ProjectDashboard.Domain.Tests.Project.TestData;

public static class AhpProjectSiteTestData
{
    public static readonly AhpProjectSite FirstAhpProjectSite = new(
        FrontDoorSiteId.From(Guid.NewGuid().ToString()),
        SiteId.From(Guid.NewGuid().ToString()),
        "First site",
        SiteStatus.Submitted,
        new LocalAuthority(new LocalAuthorityCode("LA-1"), "Liverpool"));

    public static readonly AhpProjectSite SecondAhpProjectSite = new(
        FrontDoorSiteId.From(Guid.NewGuid().ToString()),
        SiteId.From(Guid.NewGuid().ToString()),
        "Second site",
        SiteStatus.Submitted,
        new LocalAuthority(new LocalAuthorityCode("LA-2"), "Warsaw"));

    public static readonly AhpProjectSite ThirdAhpProjectSite = new(
        FrontDoorSiteId.From(Guid.NewGuid().ToString()),
        SiteId.From(Guid.NewGuid().ToString()),
        "Third site",
        SiteStatus.Submitted,
        new LocalAuthority(new LocalAuthorityCode("LA-3"), "London"));

    public static readonly AhpProjectSite AhpProjectSiteWithoutLocalAuthority = new(
        FrontDoorSiteId.From(Guid.NewGuid().ToString()),
        SiteId.From(Guid.NewGuid().ToString()),
        "Site without local authority",
        SiteStatus.InProgress,
        null);
}
