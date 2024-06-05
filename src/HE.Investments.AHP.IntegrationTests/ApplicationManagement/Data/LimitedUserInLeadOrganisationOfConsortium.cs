using HE.Investments.AHP.IntegrationTests.ApplicationManagement.Data.Model;

namespace HE.Investments.AHP.IntegrationTests.ApplicationManagement.Data;

public static class LimitedUserInLeadOrganisationOfConsortium
{
    public static UserData UserData { get; } = new(
        "limited-integration.tests@gmail.com",
        "auth0|665c3c30e627bbe9ed3b2a6f",
        "2a7ef26a-c320-ef11-840a-000d3a0cff9a",
        "ee269470-c620-ef11-840a-002248c83519");

    public static AhpApplication Application { get; } = new("AhpApplication-Limited-01");

    public static AhpSite AhpSite => new("6953bfa3-c420-ef11-840a-002248c5d15f", "AhpProjectSite-Limited", [Application]);

    public static AhpProject[] AhpProjects => [new AhpProject("84562ba0-c420-ef11-840a-002248c83519", "AhpProject-Limited", [AhpSite])];
}
