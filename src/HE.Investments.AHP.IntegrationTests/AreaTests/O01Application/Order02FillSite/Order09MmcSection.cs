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

[Order(209)]
[SuppressMessage("xUnit", "xUnit1004", Justification = "Waits for DevOps configuration - #76791")]
public class Order09MmcSection : AhpSiteIntegrationTest
{
    public Order09MmcSection(AhpIntegrationTestFixture fixture, ITestOutputHelper output)
        : base(fixture, output)
    {
    }

    [Fact(Skip = AhpConfig.SkipTest)]
    [Order(1)]
    public async Task Order01_ShouldProvideSiteMmcUsing()
    {
        await TestSiteQuestionPage(
            SitePagesUrl.SiteMmcUsing(UserOrganisationData.OrganisationId, SiteData.SiteId),
            SitePageTitles.MmcUsing,
            SitePagesUrl.SiteMmcInformation(UserOrganisationData.OrganisationId, SiteData.SiteId),
            (nameof(SiteModernMethodsOfConstruction.UsingModernMethodsOfConstruction), SiteData.UsingMmc.ToString()));
    }

    [Fact(Skip = AhpConfig.SkipTest)]
    [Order(2)]
    public async Task Order02_ShouldProvideSiteMmcInformation()
    {
        await TestSiteQuestionPage(
            SitePagesUrl.SiteMmcInformation(UserOrganisationData.OrganisationId, SiteData.SiteId),
            SitePageTitles.MmcInformation,
            SitePagesUrl.SiteMmcCategories(UserOrganisationData.OrganisationId, SiteData.SiteId),
            (nameof(SiteModernMethodsOfConstruction.InformationBarriers), SiteData.GenerateInformationBarriers()),
            (nameof(SiteModernMethodsOfConstruction.InformationImpact), SiteData.GenerateInformationImpact()));
    }

    [Fact(Skip = AhpConfig.SkipTest)]
    [Order(3)]
    public async Task Order03_ShouldProvideSiteMmcCategories()
    {
        await TestSiteQuestionPage(
            SitePagesUrl.SiteMmcCategories(UserOrganisationData.OrganisationId, SiteData.SiteId),
            SitePageTitles.MmcCategories,
            SitePagesUrl.SiteMmcCategory3D(UserOrganisationData.OrganisationId, SiteData.SiteId),
            SiteData.MmcCategories.ToFormInputs(nameof(SiteModernMethodsOfConstruction.ModernMethodsConstructionCategories)));
    }

    [Fact(Skip = AhpConfig.SkipTest)]
    [Order(4)]
    public async Task Order04_ShouldProvideSiteMmcCategory3D()
    {
        await TestSiteQuestionPage(
            SitePagesUrl.SiteMmcCategory3D(UserOrganisationData.OrganisationId, SiteData.SiteId),
            SitePageTitles.Mmc3DCategory,
            SitePagesUrl.SiteMmcCategory2D(UserOrganisationData.OrganisationId, SiteData.SiteId),
            (nameof(SiteModernMethodsOfConstruction.ModernMethodsConstruction3DSubcategories), SiteData.Mmc3DSubcategory.ToString()));
    }

    [Fact(Skip = AhpConfig.SkipTest)]
    [Order(5)]
    public async Task Order05_ShouldProvideSiteMmcCategory2D()
    {
        await TestSiteQuestionPage(
            SitePagesUrl.SiteMmcCategory2D(UserOrganisationData.OrganisationId, SiteData.SiteId),
            SitePageTitles.Mmc2DCategory,
            SitePagesUrl.SiteProcurements(UserOrganisationData.OrganisationId, SiteData.SiteId),
            (nameof(SiteModernMethodsOfConstruction.ModernMethodsConstruction2DSubcategories), SiteData.Mmc2DSubcategory.ToString()));
    }
}
