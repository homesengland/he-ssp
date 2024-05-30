using HE.Investment.AHP.Domain.Project.ValueObjects;
using HE.Investments.FrontDoor.Shared.Project;

namespace HE.Investment.AHP.Domain.Tests.Project.TestData;

public static class AhpProjectApplicationsTestData
{
    public static readonly AhpProjectApplications FirstAhpProjectWithApplications = new(
        new FrontDoorProjectId("project-id-1"),
        new AhpProjectName("First project"),
        [AhpProjectApplicationTestData.FirstAhpProjectApplication, AhpProjectApplicationTestData.SecondAhpProjectApplication]);

    public static readonly AhpProjectApplications SecondAhpProjectWithApplications = new(
        new FrontDoorProjectId("project-id-2"),
        new AhpProjectName("Second project"),
        [AhpProjectApplicationTestData.ThirdAhpProjectApplication]);

    public static readonly AhpProjectApplications ThirdAhpProjectWithApplications = new(
        new FrontDoorProjectId("project-id-3"),
        new AhpProjectName("Third project"),
        [AhpProjectApplicationTestData.FirstAhpProjectApplication, AhpProjectApplicationTestData.SecondAhpProjectApplication, AhpProjectApplicationTestData.ThirdAhpProjectApplication]);

    public static readonly AhpProjectApplications AhpProjectWithoutApplications = new(
        new FrontDoorProjectId("project-id-4"),
        new AhpProjectName("Fourth project"),
        []);
}
