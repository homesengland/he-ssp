using System.Diagnostics.CodeAnalysis;
using HE.Investments.AHP.IntegrationTests.AreaTests.O01Application.ApplicationManagement.Data;
using HE.Investments.AHP.IntegrationTests.Framework;
using Xunit;
using Xunit.Abstractions;

namespace HE.Investments.AHP.IntegrationTests.AreaTests.O01Application.ApplicationManagement;

[SuppressMessage("xUnit", "xUnit1004", Justification = "Waits for DevOps configuration - #76791")]
public class AdminUserInLeadOrganisationOfConsortiumTests : ApplicationManagementTestBase
{
    public AdminUserInLeadOrganisationOfConsortiumTests(AhpIntegrationTestFixture fixture, ITestOutputHelper output)
        : base(fixture, output)
    {
    }

    [Fact(Skip = AhpConfig.SkipTest)]
    public async Task GetProjects()
    {
        await TestGetProjectsEndpoint(AdminUserInLeadOrganisationOfConsortium.UserData, AdminUserInLeadOrganisationOfConsortium.Projects);
    }

    [Fact(Skip = AhpConfig.SkipTest)]
    public async Task GetProject_ProjectAdminLead()
    {
        await TestGetProjectEndpoint(AdminUserInLeadOrganisationOfConsortium.UserData, AdminUserInLeadOrganisationOfConsortium.ProjectAdminLead);
    }

    [Fact(Skip = AhpConfig.SkipTest)]
    public async Task GetProject_ProjectSitePartner()
    {
        await TestGetProjectEndpoint(AdminUserInLeadOrganisationOfConsortium.UserData, AdminUserInLeadOrganisationOfConsortium.ProjectSitePartner);
    }

    [Fact(Skip = AhpConfig.SkipTest)]
    public async Task GetSiteApplications_SiteLead()
    {
        await TestGetSiteApplicationsEndpoint(AdminUserInLeadOrganisationOfConsortium.UserData, AdminUserInLeadOrganisationOfConsortium.SiteLead);
    }

    [Fact(Skip = AhpConfig.SkipTest)]
    public async Task GetSiteApplications_SitePartner()
    {
        await TestGetSiteApplicationsEndpoint(AdminUserInLeadOrganisationOfConsortium.UserData, AdminUserInLeadOrganisationOfConsortium.SitePartner);
    }
}
