using System.Diagnostics.CodeAnalysis;
using HE.Investments.AHP.IntegrationTests.AreaTests.O01Application.ApplicationManagement.Data;
using HE.Investments.AHP.IntegrationTests.Framework;
using Xunit;
using Xunit.Abstractions;

namespace HE.Investments.AHP.IntegrationTests.AreaTests.O01Application.ApplicationManagement;

[SuppressMessage("xUnit", "xUnit1004", Justification = "Waits for DevOps configuration - #76791")]
public class AdminUserInOnlyMemberOrganisationOfConsortiumTests : ApplicationManagementTestBase
{
    public AdminUserInOnlyMemberOrganisationOfConsortiumTests(AhpIntegrationTestFixture fixture, ITestOutputHelper output)
        : base(fixture, output)
    {
    }

    [Fact(Skip = AhpConfig.SkipTest)]
    public async Task GetProjectsShouldNotReturnProjects()
    {
        await TestGetProjectsEndpoint(AdminUserInOnlyMemberOrganisationOfConsortium.UserData, AdminUserInOnlyMemberOrganisationOfConsortium.Projects);
    }
}
