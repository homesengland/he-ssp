using System.Diagnostics.CodeAnalysis;
using HE.Investment.AHP.Contract.Site;
using HE.Investment.AHP.WWW.Views.Site.Const;
using HE.Investments.AHP.IntegrationTests.Extensions;
using HE.Investments.AHP.IntegrationTests.Framework;
using HE.Investments.AHP.IntegrationTests.Pages;
using HE.Investments.Common.Contract.Constants;
using HE.Investments.TestsUtils.Extensions;
using Xunit;
using Xunit.Abstractions;
using Xunit.Extensions.Ordering;

namespace HE.Investments.AHP.IntegrationTests.Order02FillSite;

[Order(203)]
[SuppressMessage("xUnit", "xUnit1004", Justification = "Waits for DevOps configuration - #76791")]
public class Order03LocalAuthoritySection : AhpIntegrationTest
{
    public Order03LocalAuthoritySection(AhpIntegrationTestFixture fixture, ITestOutputHelper output)
        : base(fixture, output)
    {
    }

    [Fact(Skip = AhpConfig.SkipTest)]
    [Order(1)]
    public async Task Order01_ShouldProvideLocalAuthoritySearchPhraseAndNavigateToLocalAuthorityResult()
    {
        // given
        var currentPage = await GetCurrentPage(SitePagesUrl.SiteLocalAuthoritySearch(SiteData.SiteId));
        currentPage
            .UrlWithoutQueryEndsWith(SitePagesUrl.SiteLocalAuthoritySearch(SiteData.SiteId))
            .HasTitle(SitePageTitles.LocalAuthoritySearch)
            .HasBackLink(out _)
            .HasSubmitButton(out var searchButton, "Search");

        // when
        var searchResultPage = await TestClient.SubmitButton(searchButton, (nameof(LocalAuthorities.Phrase), SiteData.LocalAuthorityName));

        // then
        searchResultPage.UrlWithoutQueryEndsWith(SitePagesUrl.SiteLocalAuthorityResult(SiteData.SiteId))
            .HasTitle(SitePageTitles.LocalAuthorityResult);
        SaveCurrentPage();
    }

    [Fact(Skip = AhpConfig.SkipTest)]
    [Order(2)]
    public async Task Order02_ShouldSelectLocalAuthorityAndNavigateToLocalAuthorityConfirm()
    {
        // given
        var localAuthorityResultPage = await GetCurrentPage(SitePagesUrl.SiteLocalAuthorityResult(SiteData.SiteId, SiteData.LocalAuthorityName));
        localAuthorityResultPage.HasNavigationListItem("select-list", out var selectLocalAuthorityLink);

        // when
        var localAuthorityConfirmPage = await TestClient.NavigateTo(selectLocalAuthorityLink);

        // then
        localAuthorityConfirmPage
            .UrlEndWith(SitePagesUrl.SiteLocalAuthorityConfirm(SiteData.SiteId, SiteData.LocalAuthorityCode, SiteData.LocalAuthorityName))
            .HasTitle(SitePageTitles.LocalAuthorityConfirm);
        SaveCurrentPage();
    }

    [Fact(Skip = AhpConfig.SkipTest)]
    [Order(03)]
    public async Task Order03_ShouldConfirmLocalAuthorityAndNavigateToPlanningStatus()
    {
        // given
        var currentPage = await GetCurrentPage(SitePagesUrl.SiteLocalAuthorityConfirm(SiteData.SiteId, SiteData.LocalAuthorityCode, SiteData.LocalAuthorityName));
        currentPage
            .UrlWithoutQueryEndsWith(SitePagesUrl.SiteLocalAuthorityConfirmWithoutQuery(SiteData.SiteId, SiteData.LocalAuthorityCode))
            .HasTitle(SitePageTitles.LocalAuthorityConfirm)
            .HasBackLink(out _)
            .HasSubmitButton(out var confirmButton, "Continue");

        // when
        var planningStatusPage = await TestClient.SubmitButton(
            confirmButton,
            ("Response", CommonResponse.Yes));

        // then
        planningStatusPage.UrlWithoutQueryEndsWith(SitePagesUrl.SitePlanningStatus(SiteData.SiteId))
            .HasTitle(SitePageTitles.PlanningStatus);
        SaveCurrentPage();
    }
}
