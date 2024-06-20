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

[Order(207)]
[SuppressMessage("xUnit", "xUnit1004", Justification = "Waits for DevOps configuration - #76791")]
public class Order07LandDetailsSection : AhpSiteIntegrationTest
{
    public Order07LandDetailsSection(AhpIntegrationTestFixture fixture, ITestOutputHelper output)
        : base(fixture, output)
    {
    }

    [Fact(Skip = AhpConfig.SkipTest)]
    [Order(1)]
    public async Task Order01_ShouldProvideLandAcquisitionStatus()
    {
        await TestSiteQuestionPage(
            SitePagesUrl.SiteLandAcquisitionStatus(UserOrganisationData.OrganisationId, SiteData.SiteId),
            SitePageTitles.LandAcquisitionStatus,
            SitePagesUrl.SiteTenderingStatus(UserOrganisationData.OrganisationId, SiteData.SiteId),
            (nameof(SiteModel.LandAcquisitionStatus), SiteData.LandAcquisitionStatus.ToString()));
    }

    [Fact(Skip = AhpConfig.SkipTest)]
    [Order(2)]
    public async Task Order02_ShouldProvideTenderingStatus()
    {
        await TestSiteQuestionPage(
            SitePagesUrl.SiteTenderingStatus(UserOrganisationData.OrganisationId, SiteData.SiteId),
            SitePageTitles.TenderingStatus,
            SitePagesUrl.SiteContractorDetails(UserOrganisationData.OrganisationId, SiteData.SiteId),
            (nameof(SiteTenderingStatusDetails.TenderingStatus), SiteData.TenderingStatus.ToString()));
    }

    [Fact(Skip = AhpConfig.SkipTest)]
    [Order(3)]
    public async Task Order03_ShouldProvideContractorDetails()
    {
        await TestSiteQuestionPage(
            SitePagesUrl.SiteContractorDetails(UserOrganisationData.OrganisationId, SiteData.SiteId),
            SitePageTitles.ContractorDetails,
            SitePagesUrl.SiteStrategicSite(UserOrganisationData.OrganisationId, SiteData.SiteId),
            (nameof(SiteTenderingStatusDetails.ContractorName), SiteData.GenerateContractorName()),
            (nameof(SiteTenderingStatusDetails.IsSmeContractor), SiteData.IsSmeContractor.ToBoolAnswer()));
    }

    [Fact(Skip = AhpConfig.SkipTest)]
    [Order(4)]
    public async Task Order04_ShouldProvideStrategicSite()
    {
        await TestSiteQuestionPage(
            SitePagesUrl.SiteStrategicSite(UserOrganisationData.OrganisationId, SiteData.SiteId),
            SitePageTitles.StrategicSite,
            SitePagesUrl.SiteType(UserOrganisationData.OrganisationId, SiteData.SiteId),
            (nameof(StrategicSite.IsStrategicSite), SiteData.IsStrategicSite.ToBoolAnswer()),
            (nameof(StrategicSite.StrategicSiteName), SiteData.GenerateStrategicSiteName()));
    }

    [Fact(Skip = AhpConfig.SkipTest)]
    [Order(5)]
    public async Task Order05_ShouldProvideSiteType()
    {
        await TestSiteQuestionPage(
            SitePagesUrl.SiteType(UserOrganisationData.OrganisationId, SiteData.SiteId),
            SitePageTitles.SiteType,
            SitePagesUrl.SiteUse(UserOrganisationData.OrganisationId, SiteData.SiteId),
            (nameof(SiteTypeDetails.SiteType), SiteData.SiteType.ToString()),
            (nameof(SiteTypeDetails.IsOnGreenBelt), SiteData.IsOnGreenBelt.ToBoolAnswer()),
            (nameof(SiteTypeDetails.IsRegenerationSite), SiteData.IsRegenerationSite.ToBoolAnswer()));
    }
}
