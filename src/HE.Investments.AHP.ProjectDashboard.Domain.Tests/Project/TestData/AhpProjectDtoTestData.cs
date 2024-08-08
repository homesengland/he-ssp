using HE.Common.IntegrationModel.PortalIntegrationModel;

namespace HE.Investments.AHP.ProjectDashboard.Domain.Tests.Project.TestData;

public static class AhpProjectDtoTestData
{
    public static readonly AhpProjectDto FirstAhpProject = new()
    {
        AhpProjectId = "485b1f9a-d9ad-4c8a-a2f5-ee3da1c66f73",
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
        AhpProjectId = "7f240abe-50ab-4a30-bb93-59e9c65d8e72",
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
        AhpProjectId = "02ef3400-7e10-4fe3-b24e-315e52bc07af",
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
        AhpProjectId = "5f0e96ca-f618-4d6e-b17c-e54fdb913c24",
        AhpProjectName = "Fourth project",
        FrontDoorProjectId = "front-door-project-id-4",
        FrontDoorProjectName = "Fourth project",
        ListOfSites = [],
        ListOfApplications = [],
    };
}
