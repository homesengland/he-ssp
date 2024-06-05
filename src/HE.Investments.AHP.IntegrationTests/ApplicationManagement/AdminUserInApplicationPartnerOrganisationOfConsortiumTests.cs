using System.Diagnostics.CodeAnalysis;
using HE.Investments.AHP.IntegrationTests.ApplicationManagement.Data;
using HE.Investments.AHP.IntegrationTests.Framework;
using Xunit;
using Xunit.Abstractions;

namespace HE.Investments.AHP.IntegrationTests.ApplicationManagement;

[SuppressMessage("xUnit", "xUnit1004", Justification = "Waits for DevOps configuration - #76791")]
public class AdminUserInApplicationPartnerOrganisationOfConsortiumTests : ApplicationManagementTestBase
{
    public AdminUserInApplicationPartnerOrganisationOfConsortiumTests(AhpIntegrationTestFixture fixture, ITestOutputHelper output)
        : base(fixture, output)
    {
    }

    [Fact(Skip = AhpConfig.SkipTest)]
    public async Task GetProjectsForApplicationPartner()
    {
        await TestGetProjectsEndpoint(
            AdminUserInApplicationPartnerOrganisationOfConsortium.UserData,
            AdminUserInApplicationPartnerOrganisationOfConsortium.Projects);
    }

    [Fact(Skip = AhpConfig.SkipTest)]
    public async Task GetProjectForSitePartner()
    {
        await TestGetProjectEndpoint(
            AdminUserInApplicationPartnerOrganisationOfConsortium.UserData,
            AdminUserInApplicationPartnerOrganisationOfConsortium.ProjectSitePartner);
    }

    [Fact(Skip = AhpConfig.SkipTest)]
    public async Task GetSiteApplicationsForSitePartner()
    {
        await TestGetSiteApplicationsEndpoint(
            AdminUserInApplicationPartnerOrganisationOfConsortium.UserData,
            AdminUserInApplicationPartnerOrganisationOfConsortium.SitePartner);
    }
}
