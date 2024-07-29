using System.Diagnostics.CodeAnalysis;
using HE.Investments.AHP.IntegrationTests.AreaTests.O01Application.ApplicationManagement.Data;
using HE.Investments.AHP.IntegrationTests.Framework;
using Xunit;
using Xunit.Abstractions;

namespace HE.Investments.AHP.IntegrationTests.AreaTests.O01Application.ApplicationManagement;

[SuppressMessage("xUnit", "xUnit1004", Justification = "Waits for DevOps configuration - #76791")]
public class AdminUserInSitePartnerOrganisationOfConsortiumTests : ApplicationManagementTestBase
{
    public AdminUserInSitePartnerOrganisationOfConsortiumTests(AhpIntegrationTestFixture fixture, ITestOutputHelper output)
        : base(fixture, output)
    {
    }

    [Fact(Skip = AhpConfig.SkipTest)]
    public async Task GetProjectsForSitePartner()
    {
        await TestGetProjectsEndpoint(AdminUserInSitePartnerOrganisationOfConsortium.UserData, AdminUserInSitePartnerOrganisationOfConsortium.Projects);
    }

    [Fact(Skip = AhpConfig.SkipTest)]
    public async Task GetProjectForSitePartner()
    {
        await TestGetProjectEndpoint(AdminUserInSitePartnerOrganisationOfConsortium.UserData, AdminUserInSitePartnerOrganisationOfConsortium.ProjectSitePartner);
    }

    [Fact(Skip = AhpConfig.SkipTest)]
    public async Task GetSiteApplicationsForSitePartner()
    {
        await TestGetSiteApplicationsEndpoint(AdminUserInSitePartnerOrganisationOfConsortium.UserData, AdminUserInSitePartnerOrganisationOfConsortium.SitePartner);
    }

    [Fact(Skip = AhpConfig.SkipTest)]
    public async Task GetApplicationDetailsForSitePartner()
    {
        await TestGetApplicationEndpoint(
            AdminUserInSitePartnerOrganisationOfConsortium.UserData,
            AdminUserInSitePartnerOrganisationOfConsortium.ApplicationPartnerForSitePartner);
    }
}
