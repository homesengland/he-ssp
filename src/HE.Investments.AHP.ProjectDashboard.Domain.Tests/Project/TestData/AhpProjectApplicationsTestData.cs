using HE.Investments.AHP.ProjectDashboard.Domain.Project.ValueObjects;
using HE.Investments.FrontDoor.Shared.Project;

namespace HE.Investments.AHP.ProjectDashboard.Domain.Tests.Project.TestData;

public static class AhpProjectApplicationsTestData
{
    public static readonly AhpProjectOverview FirstAhpProjectWithApplications = new(
        new FrontDoorProjectId("project-id-1"),
        new AhpProjectName("First project"),
        [AhpProjectApplicationTestData.FirstAhpProjectApplication, AhpProjectApplicationTestData.SecondAhpProjectApplication]);

    public static readonly AhpProjectOverview SecondAhpProjectWithApplications = new(
        new FrontDoorProjectId("project-id-2"),
        new AhpProjectName("Second project"),
        [AhpProjectApplicationTestData.ThirdAhpProjectApplication]);

    public static readonly AhpProjectOverview ThirdAhpProjectWithApplications = new(
        new FrontDoorProjectId("project-id-3"),
        new AhpProjectName("Third project"),
        [AhpProjectApplicationTestData.FirstAhpProjectApplication, AhpProjectApplicationTestData.SecondAhpProjectApplication, AhpProjectApplicationTestData.ThirdAhpProjectApplication]);

    public static readonly AhpProjectOverview AhpProjectWithoutApplications = new(
        new FrontDoorProjectId("project-id-4"),
        new AhpProjectName("Fourth project"),
        []);
}
