using System.Diagnostics.CodeAnalysis;
using HE.Investment.AHP.Contract.Site;
using HE.Investment.AHP.WWW.Models.Site;
using HE.Investment.AHP.WWW.Views.Site.Const;
using HE.Investments.AHP.IntegrationTests.Extensions;
using HE.Investments.AHP.IntegrationTests.Framework;
using HE.Investments.AHP.IntegrationTests.Pages;
using Xunit;
using Xunit.Abstractions;
using Xunit.Extensions.Ordering;

namespace HE.Investments.AHP.IntegrationTests.Order02FillSite;

[Order(205)]
[SuppressMessage("xUnit", "xUnit1004", Justification = "Waits for DevOps configuration - #76791")]
public class Order05DesignGuidelinesSection : AhpSiteIntegrationTest
{
    public Order05DesignGuidelinesSection(AhpIntegrationTestFixture fixture, ITestOutputHelper output)
        : base(fixture, output)
    {
    }

    [Fact(Skip = AhpConfig.SkipTest)]
    [Order(1)]
    public async Task Order01_ShouldProvideNationalDesignGuidePriorities()
    {
        await TestSiteQuestionPage(
            SitePagesUrl.SiteNationalDesignGuide(UserOrganisationData.OrganisationId, SiteData.SiteId),
            SitePageTitles.NationalDesignGuide,
            SitePagesUrl.SiteBuildingForHealthyLife(UserOrganisationData.OrganisationId, SiteData.SiteId),
            SiteData.NationalDesignGuidePriorities.ToFormInputs(nameof(NationalDesignGuidePrioritiesModel.DesignPriorities)));
    }

    [Fact(Skip = AhpConfig.SkipTest)]
    [Order(2)]
    public async Task Order02_ShouldProvideBuildingForHealthyLife()
    {
        await TestSiteQuestionPage(
            SitePagesUrl.SiteBuildingForHealthyLife(UserOrganisationData.OrganisationId, SiteData.SiteId),
            SitePageTitles.BuildingForHealthyLife,
            SitePagesUrl.SiteProvideNumberOfGreenLights(UserOrganisationData.OrganisationId, SiteData.SiteId),
            (nameof(SiteModel.BuildingForHealthyLife), SiteData.BuildingForHealthyLife.ToString()));
    }

    [Fact(Skip = AhpConfig.SkipTest)]
    [Order(3)]
    public async Task Order03_ShouldProvideNumberOfGreenLights()
    {
        await TestSiteQuestionPage(
            SitePagesUrl.SiteProvideNumberOfGreenLights(UserOrganisationData.OrganisationId, SiteData.SiteId),
            SitePageTitles.NumberOfGreenLights,
            SitePagesUrl.SiteDevelopingPartner(UserOrganisationData.OrganisationId, SiteData.SiteId),
            (nameof(SiteModel.NumberOfGreenLights), SiteData.NumberOfGreenLights));
    }
}
