using System.Diagnostics.CodeAnalysis;
using HE.Investment.AHP.Contract.Site;
using HE.Investment.AHP.WWW;
using HE.Investment.AHP.WWW.Views.HomeTypes.Const;
using HE.Investment.AHP.WWW.Views.Site;
using HE.Investments.AHP.IntegrationTests.Extensions;
using HE.Investments.AHP.IntegrationTests.Framework;
using HE.Investments.AHP.IntegrationTests.Pages;
using HE.Investments.IntegrationTestsFramework;
using HE.Investments.TestsUtils.Extensions;
using Xunit;
using Xunit.Extensions.Ordering;

namespace HE.Investments.AHP.IntegrationTests.FillSite;

[Order(1)]
[SuppressMessage("xUnit", "xUnit1004", Justification = "Waits for DevOps configuration - #76791")]
public class Order01StartAhpSite : AhpIntegrationTest
{
    public Order01StartAhpSite(IntegrationTestFixture<Program> fixture)
        : base(fixture)
    {
    }

    [Fact(Skip = AhpConfig.SkipTest)]
    [Order(1)]
    public async Task Order01_ShouldOpenStartSite()
    {
        // given & when
        var mainPage = await TestClient.NavigateTo(SitePagesUrl.SiteStart);

        // then
        mainPage
            .UrlEndWith(SitePagesUrl.SiteStart)
            .HasTitle(SitePageTitles.SiteDetails);

        SaveCurrentPage();
    }

    [Fact(Skip = AhpConfig.SkipTest)]
    [Order(2)]
    public async Task Order02_ShouldNavigateToSiteNamePage()
    {
        // given
        var siteStartPage = await GetCurrentPage(SitePagesUrl.SiteStart);
        siteStartPage.HasLinkButtonForTestId("site-start-continue", out var siteNamePageLink);

        // when
        var siteNamePage = await TestClient.NavigateTo(siteNamePageLink);

        // then
        siteNamePage
            .UrlEndWith(SitePagesUrl.SiteName)
            .HasTitle(SitePageTitles.SiteName);

        SaveCurrentPage();
    }

    [Fact(Skip = AhpConfig.SkipTest)]
    [Order(3)]
    public async Task Order03_ShouldProvideSiteNameAndNavigateToSection106GeneralAgreementPage()
    {
        // given
        var siteNamePage = await GetCurrentPage(SitePagesUrl.SiteName);
        siteNamePage.HasGdsSubmitButton("continue-button", out var continueButton);

        // when
        var section106GeneralAgreementPage = await TestClient.SubmitButton(
            continueButton,
            new Dictionary<string, string> { { nameof(SiteModel.Name), SiteData.GenerateSiteName() } });

        // then
        SiteData.SetSiteId(section106GeneralAgreementPage.Url.GetApplicationGuidFromUrl());

        section106GeneralAgreementPage
            .UrlWithoutQueryEndsWith(SitePagesUrl.SiteSection106GeneralAgreement(SiteData.SiteId))
            .HasTitle(SitePageTitles.SiteSection106Agreement);

        SaveCurrentPage();
    }

    [Fact(Skip = AhpConfig.SkipTest)]
    [Order(4)]
    public async Task Order04_ShouldProvideSection106GeneralAgreementAndNavigateToSection106AffordableHousing()
    {
        await TestQuestionPage(
            SitePagesUrl.SiteSection106GeneralAgreement(SiteData.SiteId),
            SitePageTitles.SiteSection106Agreement,
            SitePagesUrl.SiteSection106AffordableHousing(SiteData.SiteId),
            (nameof(SiteModel.Section106GeneralAgreement), "True"));
    }

    [Fact(Skip = AhpConfig.SkipTest)]
    [Order(5)]
    public async Task Order05_ShouldProvideSection106AffordableHousingAndNavigateToSection106OnlyAffordableHousing()
    {
        await TestQuestionPage(
            SitePagesUrl.SiteSection106AffordableHousing(SiteData.SiteId),
            SitePageTitles.SiteSection106AffordableHousing,
            SitePagesUrl.SiteSection106OnlyAffordableHousing(SiteData.SiteId),
            (nameof(SiteModel.Section106AffordableHousing), "True"));
    }

    [Fact(Skip = AhpConfig.SkipTest)]
    [Order(6)]
    public async Task Order06_ShouldProvideSection106OnlyAffordableHousingAndNavigateToSection106AdditionalAffordableHousing()
    {
        await TestQuestionPage(
            SitePagesUrl.SiteSection106OnlyAffordableHousing(SiteData.SiteId),
            SitePageTitles.SiteSection106OnlyAffordableHousing,
            SitePagesUrl.SiteSection106AdditionalAffordableHousing(SiteData.SiteId),
            (nameof(SiteModel.Section106OnlyAffordableHousing), "False"));
    }

    [Fact(Skip = AhpConfig.SkipTest)]
    [Order(7)]
    public async Task Order07_ShouldProvideSection106AdditionalAffordableHousingAndNavigateToSection106CapitalFundingEligibility()
    {
        await TestQuestionPage(
            SitePagesUrl.SiteSection106AdditionalAffordableHousing(SiteData.SiteId),
            SitePageTitles.SiteSection106AdditionalAffordableHousing,
            SitePagesUrl.SiteSection106CapitalFundingEligibility(SiteData.SiteId),
            (nameof(SiteModel.Section106AdditionalAffordableHousing), "True"));
    }

    [Fact(Skip = AhpConfig.SkipTest)]
    [Order(8)]
    public async Task Order08_ShouldProvideSection106CapitalFundingEligibilityAndNavigateToSection106LocalAuthorityConfirmation()
    {
        await TestQuestionPage(
            SitePagesUrl.SiteSection106CapitalFundingEligibility(SiteData.SiteId),
            SitePageTitles.SiteSection106CapitalFundingEligibility,
            SitePagesUrl.SiteSection106LocalAuthorityConfirmation(SiteData.SiteId),
            (nameof(SiteModel.Section106CapitalFundingEligibility), "False"));
    }

    [Fact(Skip = AhpConfig.SkipTest)]
    [Order(9)]
    public async Task Order09_ShouldProvideSection106LocalAuthorityConfirmationAndNavigateToLocalAuthoritySearch()
    {
        await TestQuestionPage(
            SitePagesUrl.SiteSection106LocalAuthorityConfirmation(SiteData.SiteId),
            SitePageTitles.SiteSection106LocalAuthorityConfirmation,
            SitePagesUrl.SiteLocalAuthoritySearch(SiteData.SiteId),
            (nameof(SiteModel.Section106LocalAuthorityConfirmation), "Local authority confirmed"));
    }

    [Fact(Skip = AhpConfig.SkipTest)]
    [Order(10)]
    public async Task Order10_ShouldProvideLocalAuthoritySearchPhraseAndNavigateToLocalAuthorityResult()
    {
        await TestQuestionPage(
            SitePagesUrl.SiteLocalAuthoritySearch(SiteData.SiteId),
            SitePageTitles.LocalAuthoritySearch,
            SitePagesUrl.SiteLocalAuthorityResult(SiteData.SiteId),
            (nameof(LocalAuthorities.Phrase), SiteData.LocalAuthorityName));
    }

    [Fact(Skip = AhpConfig.SkipTest)]
    [Order(11)]
    public async Task Order11_ShouldSelectLocalAuthorityAndNavigateToLocalAuthorityConfirm()
    {
        // given
        var localAuthorityResultPage = await GetCurrentPage(SitePagesUrl.SiteLocalAuthorityResult(SiteData.SiteId));
        localAuthorityResultPage.HasNavigationListItem("select-list", out var selectLocalAuthorityLink);

        // when
        var localAuthorityConfirmPage = await TestClient.NavigateTo(selectLocalAuthorityLink);

        // then
        ApplicationData.SetSiteId(localAuthorityConfirmPage.Url.GetSiteGuidFromUrl());
        localAuthorityConfirmPage
            .UrlEndWith(SitePagesUrl.SiteLocalAuthorityConfirm(SiteData.SiteId, SiteData.LocalAuthorityId, SiteData.LocalAuthorityName, SiteData.LocalAuthorityName))
            .HasTitle(SitePageTitles.LocalAuthorityConfirm);

        SaveCurrentPage();
    }
}
