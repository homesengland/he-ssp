using HE.Common.IntegrationModel.PortalIntegrationModel;

namespace HE.Investment.AHP.Domain.Tests.Project.TestData;

public static class AhpProjectDtoTestData
{
    public static readonly AhpProjectDto FirstAhpProject = new()
    {
        AhpProjectId = "ahp-project-id-1",
        AhpProjectName = "First project",
        FrontDoorProjectId = "front-door-project-id-1",
        FrontDoorProjectName = "First project",
        ListOfSites =
        [
            SiteDtoTestData.FirstSite,
            SiteDtoTestData.SecondSite
        ],
        ListOfApplications =
        [
            AhpApplicationDtoTestData.FirstAhpApplication,
            AhpApplicationDtoTestData.SecondAhpApplication
        ],
    };

    public static readonly AhpProjectDto SecondAhpProject = new()
    {
        AhpProjectId = "ahp-project-id-2",
        AhpProjectName = "Second project",
        FrontDoorProjectId = "front-door-project-id-2",
        FrontDoorProjectName = "Second project",
        ListOfSites =
        [
            SiteDtoTestData.ThirdSite
        ],
        ListOfApplications =
        [
            AhpApplicationDtoTestData.ThirdAhpApplication
        ],
    };

    public static readonly AhpProjectDto ThirdAhpProject = new()
    {
        AhpProjectId = "ahp-project-id-3",
        AhpProjectName = "Third project",
        FrontDoorProjectId = "front-door-project-id-3",
        FrontDoorProjectName = "Third project",
        ListOfSites =
        [
            SiteDtoTestData.FourthSite
        ],
        ListOfApplications =
        [
            AhpApplicationDtoTestData.FirstAhpApplication,
            AhpApplicationDtoTestData.SecondAhpApplication,
            AhpApplicationDtoTestData.ThirdAhpApplication,
            AhpApplicationDtoTestData.FourthAhpApplication,
            AhpApplicationDtoTestData.FifthAhpApplication,
        ],
    };

    public static readonly AhpProjectDto FourthAhpProjectWithoutSitesAndApplications = new()
    {
        AhpProjectId = "ahp-project-id-4",
        AhpProjectName = "Fourth project",
        FrontDoorProjectId = "front-door-project-id-4",
        FrontDoorProjectName = "Fourth project",
        ListOfSites = [],
        ListOfApplications = [],
    };
}
