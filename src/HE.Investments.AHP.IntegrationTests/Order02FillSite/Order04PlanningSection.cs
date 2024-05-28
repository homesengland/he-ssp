using System.Diagnostics.CodeAnalysis;
using HE.Investment.AHP.Contract.Site;
using HE.Investment.AHP.WWW.Views.Site.Const;
using HE.Investments.AHP.IntegrationTests.Extensions;
using HE.Investments.AHP.IntegrationTests.Framework;
using HE.Investments.AHP.IntegrationTests.Pages;
using Xunit;
using Xunit.Abstractions;
using Xunit.Extensions.Ordering;

namespace HE.Investments.AHP.IntegrationTests.Order02FillSite;

[Order(204)]
[SuppressMessage("xUnit", "xUnit1004", Justification = "Waits for DevOps configuration - #76791")]
public class Order04PlanningSection : AhpIntegrationTest
{
    public Order04PlanningSection(AhpIntegrationTestFixture fixture, ITestOutputHelper output)
        : base(fixture, output)
    {
    }

    [Fact(Skip = AhpConfig.SkipTest)]
    [Order(1)]
    public async Task Order01_ShouldProvidePlanningStatus()
    {
        await TestQuestionPage(
            SitePagesUrl.SitePlanningStatus(SiteData.SiteId),
            SitePageTitles.PlanningStatus,
            SitePagesUrl.SitePlanningDetails(SiteData.SiteId),
            (nameof(SitePlanningDetails.PlanningStatus), SiteData.PlanningStatus.ToString()));
    }

    [Fact(Skip = AhpConfig.SkipTest)]
    [Order(2)]
    public async Task Order02_ShouldProvidePlanningDetails()
    {
        await TestQuestionPage(
            SitePagesUrl.SitePlanningDetails(SiteData.SiteId),
            "Planning details",
            SitePagesUrl.SiteLandRegistry(SiteData.SiteId),
            ("ExpectedPlanningApprovalDate.Day", SiteData.ExpectedPlanningApprovalDate.Day!),
            ("ExpectedPlanningApprovalDate.Month", SiteData.ExpectedPlanningApprovalDate.Month!),
            ("ExpectedPlanningApprovalDate.Year", SiteData.ExpectedPlanningApprovalDate.Year!),
            (nameof(SitePlanningDetails.IsLandRegistryTitleNumberRegistered), SiteData.IsLandRegistryTitleNumberRegistered.ToBoolAnswer()));
    }

    [Fact(Skip = AhpConfig.SkipTest)]
    [Order(3)]
    public async Task Order03_ShouldProvideLandRegistry()
    {
        await TestQuestionPage(
            SitePagesUrl.SiteLandRegistry(SiteData.SiteId),
            SitePageTitles.LandRegistry,
            SitePagesUrl.SiteNationalDesignGuide(SiteData.SiteId),
            (nameof(SitePlanningDetails.LandRegistryTitleNumber), SiteData.GenerateLandRegistryTitleNumber()),
            (nameof(SitePlanningDetails.IsGrantFundingForAllHomesCoveredByTitleNumber), SiteData.IsGrantFundingForAllHomesCoveredByTitleNumber.ToBoolAnswer()));
    }
}
