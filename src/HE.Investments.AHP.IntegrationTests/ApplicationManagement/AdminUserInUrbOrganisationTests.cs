using System.Diagnostics.CodeAnalysis;
using HE.Investments.AHP.IntegrationTests.ApplicationManagement.Data;
using HE.Investments.AHP.IntegrationTests.Framework;
using Xunit;
using Xunit.Abstractions;

namespace HE.Investments.AHP.IntegrationTests.ApplicationManagement;

[SuppressMessage("xUnit", "xUnit1004", Justification = "Waits for DevOps configuration - #76791")]
public class AdminUserInUrbOrganisationTests : ApplicationManagementTestBase
{
    public AdminUserInUrbOrganisationTests(AhpIntegrationTestFixture fixture, ITestOutputHelper output)
        : base(fixture, output)
    {
    }

    [Fact(Skip = AhpConfig.SkipTest)]
    public async Task GetProjects()
    {
        await TestGetProjectsEndpoint(AdminUserInUrbOrganisation.UserData, AdminUserInUrbOrganisation.AhpProjects);
    }

    [Fact(Skip = AhpConfig.SkipTest)]
    public async Task GetProject()
    {
        await TestGetProjectEndpoint(AdminUserInUrbOrganisation.UserData, AdminUserInUrbOrganisation.AhpProjects[0]);
    }

    [Fact(Skip = AhpConfig.SkipTest)]
    public async Task GetSiteApplications()
    {
        await TestGetSiteApplicationsEndpoint(AdminUserInUrbOrganisation.UserData, AdminUserInUrbOrganisation.AhpSite);
    }
}
