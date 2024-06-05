using System.Diagnostics.CodeAnalysis;
using HE.Investments.AHP.IntegrationTests.ApplicationManagement.Data;
using HE.Investments.AHP.IntegrationTests.Framework;
using Xunit;
using Xunit.Abstractions;

namespace HE.Investments.AHP.IntegrationTests.ApplicationManagement;

[SuppressMessage("xUnit", "xUnit1004", Justification = "Waits for DevOps configuration - #76791")]
public class LimitedUserInLeadOrganisationOfConsortiumTests : ApplicationManagementTestBase
{
    public LimitedUserInLeadOrganisationOfConsortiumTests(AhpIntegrationTestFixture fixture, ITestOutputHelper output)
        : base(fixture, output)
    {
    }

    [Fact(Skip = AhpConfig.SkipTest)]
    public async Task GetProjects()
    {
        await TestGetProjectsEndpoint(LimitedUserInLeadOrganisationOfConsortium.UserData, LimitedUserInLeadOrganisationOfConsortium.AhpProjects);
    }

    [Fact(Skip = AhpConfig.SkipTest)]
    public async Task GetProject()
    {
        await TestGetProjectEndpoint(LimitedUserInLeadOrganisationOfConsortium.UserData, LimitedUserInLeadOrganisationOfConsortium.AhpProjects[0]);
    }

    [Fact(Skip = AhpConfig.SkipTest)]
    public async Task GetSiteApplications()
    {
        await TestGetSiteApplicationsEndpoint(LimitedUserInLeadOrganisationOfConsortium.UserData, LimitedUserInLeadOrganisationOfConsortium.AhpSite);
    }
}
