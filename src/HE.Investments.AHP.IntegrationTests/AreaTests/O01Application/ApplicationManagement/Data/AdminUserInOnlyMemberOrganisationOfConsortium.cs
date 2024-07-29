using HE.Investments.AHP.IntegrationTests.AreaTests.O01Application.ApplicationManagement.Data.Model;

namespace HE.Investments.AHP.IntegrationTests.AreaTests.O01Application.ApplicationManagement.Data;

public static class AdminUserInOnlyMemberOrganisationOfConsortium
{
    public static UserData UserData { get; } = new(
        "admin.consortium.member-integration.tests@gmail.com",
        "auth0|665c49dfcd159640ae0d9e32",
        "dba3274b-cb20-ef11-840a-000d3a0cff9a",
        "ee269470-c620-ef11-840a-002248c83519");

    public static AhpProject[] Projects => [];
}
