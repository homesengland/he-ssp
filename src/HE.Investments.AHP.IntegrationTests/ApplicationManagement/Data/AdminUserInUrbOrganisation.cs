using HE.Investments.AHP.IntegrationTests.ApplicationManagement.Data.Model;

namespace HE.Investments.AHP.IntegrationTests.ApplicationManagement.Data;

public static class AdminUserInUrbOrganisation
{
    public static UserData UserData { get; } = new(
        "admin.urb-integration.tests@gmail.com",
        "auth0|66617af7ae7ff8ab091aede5",
        "4f295787-e323-ef11-840a-0022481b594a",
        null);

    public static AhpApplication Application { get; } = new("f57d38c8-0d33-ef11-8e4e-6045bd0b0565", "AhpApplication-Urb");

    public static AhpSite AhpSite => new("7c7f10a3-e423-ef11-840a-000d3a0cff9a", "AhpProjectSite-Urb", [Application]);

    public static AhpProject[] AhpProjects => [new AhpProject("7146af9c-e423-ef11-840a-000d3a0cff9a", "AhpProject-Urb", [AhpSite])];
}
