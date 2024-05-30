using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using HE.Investment.AHP.Contract.Common.Enums;
using HE.Investment.AHP.Contract.Site;
using HE.Investment.AHP.WWW.Views.Site.Const;
using HE.Investments.AHP.IntegrationTests.Extensions;
using HE.Investments.AHP.IntegrationTests.Framework;
using HE.Investments.AHP.IntegrationTests.Pages;
using HE.Investments.Common.Contract;
using HE.Investments.IntegrationTestsFramework.Assertions;
using HE.Investments.TestsUtils.Extensions;
using Xunit;
using Xunit.Abstractions;
using Xunit.Extensions.Ordering;

namespace HE.Investments.AHP.IntegrationTests.Order02FillSite;

[Order(208)]
[SuppressMessage("xUnit", "xUnit1004", Justification = "Waits for DevOps configuration - #76791")]
public class Order08SiteUseSection : AhpIntegrationTest
{
    public Order08SiteUseSection(AhpIntegrationTestFixture fixture, ITestOutputHelper output)
        : base(fixture, output)
    {
    }

    [Fact(Skip = AhpConfig.SkipTest)]
    [Order(1)]
    public async Task Order01_ShouldProvideSiteUse()
    {
        await TestQuestionPage(
            SitePagesUrl.SiteUse(SiteData.SiteId),
            SitePageTitles.SiteUse,
            SitePagesUrl.SiteTravellerPitchType(SiteData.SiteId),
            (nameof(SiteUseDetails.IsPartOfStreetFrontInfill), SiteData.IsPartOfStreetFrontInfill.ToBoolAnswer()),
            (nameof(SiteUseDetails.IsForTravellerPitchSite), SiteData.IsForTravellerPitchSite.ToBoolAnswer()));
    }

    [Fact(Skip = AhpConfig.SkipTest)]
    [Order(2)]
    public async Task Order02_ShouldProvideTravellerPitchType()
    {
        await TestQuestionPage(
            SitePagesUrl.SiteTravellerPitchType(SiteData.SiteId),
            SitePageTitles.TravellerPitchType,
            SitePagesUrl.SiteRuralClassification(SiteData.SiteId),
            (nameof(SiteUseDetails.TravellerPitchSiteType), SiteData.TravellerPitchSiteType.ToString()));
    }

    [Fact(Skip = AhpConfig.SkipTest)]
    [Order(3)]
    public async Task Order03_ShouldProvideRuralClassification()
    {
        await TestQuestionPage(
            SitePagesUrl.SiteRuralClassification(SiteData.SiteId),
            SitePageTitles.RuralClassification,
            SitePagesUrl.SiteEnvironmentalImpact(SiteData.SiteId),
            (nameof(SiteRuralClassification.IsWithinRuralSettlement), SiteData.IsWithinRuralSettlement.ToBoolAnswer()),
            (nameof(SiteRuralClassification.IsRuralExceptionSite), SiteData.IsRuralExceptionSite.ToBoolAnswer()));
    }

    [Fact(Skip = AhpConfig.SkipTest)]
    [Order(4)]
    public async Task Order04_ShouldProvideEnvironmentalImpact()
    {
        await TestQuestionPage(
            SitePagesUrl.SiteEnvironmentalImpact(SiteData.SiteId),
            SitePageTitles.EnvironmentalImpact,
            SitePagesUrl.SiteMmcUsing(SiteData.SiteId),
            ("EnvironmentalImpact", SiteData.GenerateEnvironmentalImpact()));
    }
}
