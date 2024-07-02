using HE.Investment.AHP.Domain.Project.ValueObjects;
using HE.Investments.FrontDoor.Shared.Project;

namespace HE.Investment.AHP.Domain.Tests.Project.TestData;

public static class AhpProjectSitesTestData
{
    public static readonly AhpProjectSites FirstAhpProjectWithSites = new(
        new FrontDoorProjectId("project-id-1"),
        new AhpProjectName("First project"),
        [AhpProjectSiteTestData.FirstAhpProjectSite, AhpProjectSiteTestData.SecondAhpProjectSite]);

    public static readonly AhpProjectSites SecondAhpProjectWithSites = new(
        new FrontDoorProjectId("project-id-1"),
        new AhpProjectName("First project"),
        [AhpProjectSiteTestData.ThirdAhpProjectSite]);

    public static readonly AhpProjectSites AhpProjectWithNotCompletedSite = new(
        new FrontDoorProjectId("project-id-1"),
        new AhpProjectName("First project"),
        [AhpProjectSiteTestData.AhpProjectSiteWithoutLocalAuthority]);

    public static readonly AhpProjectSites ProjectWithoutSites = new(
        new FrontDoorProjectId("project-id-3"),
        new AhpProjectName("Third project"),
        []);
}
