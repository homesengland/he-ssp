using System.Diagnostics.CodeAnalysis;
using HE.Investment.AHP.WWW.Views.Site.Const;
using HE.Investments.AHP.IntegrationTests.AreaTests.O01Application.Pages;
using HE.Investments.AHP.IntegrationTests.Framework;
using HE.Investments.TestsUtils.Extensions;
using Xunit;
using Xunit.Abstractions;
using Xunit.Extensions.Ordering;

namespace HE.Investments.AHP.IntegrationTests.AreaTests.O01Application.Order02FillSite;

[Order(211)]
[SuppressMessage("xUnit", "xUnit1004", Justification = "Waits for DevOps configuration - #76791")]
public class Order11SaveAndReturnToSites : AhpSiteIntegrationTest
{
    public Order11SaveAndReturnToSites(AhpIntegrationTestFixture fixture, ITestOutputHelper output)
        : base(fixture, output)
    {
    }

    [Fact(Skip = AhpConfig.SkipTest)]
    [Order(1)]
    public async Task Order01_SaveAndReturnToSitesList()
    {
        // given
        var currentPage = await GetCurrentPage(SitePagesUrl.SiteProcurements(UserOrganisationData.OrganisationId, SiteData.SiteId));
        currentPage.HasSaveAndReturnToSiteListButton(out var saveAndReturnButton);

        // when
        var siteList = await TestClient.SubmitButton(saveAndReturnButton);

        // then
        siteList.HasTitle(SitePageTitles.SiteList);
    }
}
