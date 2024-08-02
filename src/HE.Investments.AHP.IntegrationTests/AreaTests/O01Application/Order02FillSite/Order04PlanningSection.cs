using System.Diagnostics.CodeAnalysis;
using HE.Investment.AHP.Contract.Site;
using HE.Investment.AHP.WWW.Views.Site.Const;
using HE.Investments.AHP.IntegrationTests.AreaTests.O01Application.Pages;
using HE.Investments.AHP.IntegrationTests.Framework;
using HE.Investments.AHP.IntegrationTests.Framework.Extensions;
using Xunit;
using Xunit.Abstractions;
using Xunit.Extensions.Ordering;

namespace HE.Investments.AHP.IntegrationTests.AreaTests.O01Application.Order02FillSite;

[Order(204)]
[SuppressMessage("xUnit", "xUnit1004", Justification = "Waits for DevOps configuration - #76791")]
public class Order04PlanningSection : AhpSiteIntegrationTest
{
    public Order04PlanningSection(AhpIntegrationTestFixture fixture, ITestOutputHelper output)
        : base(fixture, output)
    {
    }

    [Fact(Skip = AhpConfig.SkipTest)]
    [Order(1)]
    public async Task Order01_ShouldProvidePlanningStatus()
    {
        await TestSiteQuestionPage(
            SitePagesUrl.SitePlanningStatus(UserOrganisationData.OrganisationId, SiteData.SiteId),
            SitePageTitles.PlanningStatus,
            SitePagesUrl.SitePlanningDetails(UserOrganisationData.OrganisationId, SiteData.SiteId),
            (nameof(SitePlanningDetails.PlanningStatus), SiteData.PlanningStatus.ToString()));
    }

    [Fact(Skip = AhpConfig.SkipTest)]
    [Order(2)]
    public async Task Order02_ShouldProvidePlanningDetails()
    {
        await TestSiteQuestionPage(
            SitePagesUrl.SitePlanningDetails(UserOrganisationData.OrganisationId, SiteData.SiteId),
            "Planning details",
            SitePagesUrl.SiteLandRegistry(UserOrganisationData.OrganisationId, SiteData.SiteId),
            ("ExpectedPlanningApprovalDate.Day", SiteData.ExpectedPlanningApprovalDate.Day!),
            ("ExpectedPlanningApprovalDate.Month", SiteData.ExpectedPlanningApprovalDate.Month!),
            ("ExpectedPlanningApprovalDate.Year", SiteData.ExpectedPlanningApprovalDate.Year!),
            (nameof(SitePlanningDetails.IsLandRegistryTitleNumberRegistered), SiteData.IsLandRegistryTitleNumberRegistered.ToBoolAnswer()));
    }

    [Fact(Skip = AhpConfig.SkipTest)]
    [Order(3)]
    public async Task Order03_ShouldProvideLandRegistry()
    {
        await TestSiteQuestionPage(
            SitePagesUrl.SiteLandRegistry(UserOrganisationData.OrganisationId, SiteData.SiteId),
            SitePageTitles.LandRegistry,
            SitePagesUrl.SiteNationalDesignGuide(UserOrganisationData.OrganisationId, SiteData.SiteId),
            (nameof(SitePlanningDetails.LandRegistryTitleNumber), SiteData.GenerateLandRegistryTitleNumber()),
            (nameof(SitePlanningDetails.IsGrantFundingForAllHomesCoveredByTitleNumber), SiteData.IsGrantFundingForAllHomesCoveredByTitleNumber.ToBoolAnswer()));
    }
}
