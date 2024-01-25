using System.Diagnostics.CodeAnalysis;
using HE.Investment.AHP.Contract.Site;
using HE.Investment.AHP.WWW;
using HE.Investment.AHP.WWW.Views.Site;
using HE.Investments.AHP.IntegrationTests.Extensions;
using HE.Investments.AHP.IntegrationTests.FillSite.Data;
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
        siteStartPage.HasGdsLinkButton("continue-button", out var siteNameLink);

        // when
        var siteNamePage = await TestClient.NavigateTo(siteNameLink);

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
        // given
        var section106GeneralAgreementPage = await GetCurrentPage(SitePagesUrl.SiteSection106GeneralAgreement(SiteData.SiteId));
        section106GeneralAgreementPage.HasGdsSubmitButton("continue-button", out var continueButton);

        // when
        var section106AffordableHousingPage = await TestClient.SubmitButton(
            continueButton,
            (nameof(SiteModel.Section106GeneralAgreement), "true"));

        // then
        section106AffordableHousingPage
            .UrlEndWith(SitePagesUrl.SiteSection106AffordableHousing(SiteData.SiteId))
            .HasTitle(SitePageTitles.SiteSection106AffordableHousing);

        SaveCurrentPage();
    }

    [Fact(Skip = AhpConfig.SkipTest)]
    [Order(5)]
    public async Task Order05_ShouldProvideSection106AffordableHousingAndNavigateToSection106OnlyAffordableHousing()
    {
        // given
        var section106AffordableHouaingPage = await GetCurrentPage(SitePagesUrl.SiteSection106AffordableHousing(SiteData.SiteId));
        section106AffordableHouaingPage.HasGdsSubmitButton("continue-button", out var continueButton);

        // when
        var section106OnlyAffordableHousingPage = await TestClient.SubmitButton(
            continueButton,
            (nameof(SiteModel.Section106AffordableHousing), "true"));

        // then
        section106OnlyAffordableHousingPage
            .UrlEndWith(SitePagesUrl.SiteSection106OnlyAffordableHousing(SiteData.SiteId))
            .HasTitle(SitePageTitles.SiteSection106OnlyAffordableHousing);

        SaveCurrentPage();
    }

    [Fact(Skip = AhpConfig.SkipTest)]
    [Order(6)]
    public async Task Order06_ShouldProvideSection106OnlyAffordableHousingAndNavigateToSection106AdditionalAffordableHousing()
    {
        // given
        var section106OnlyAffordableHousingPage = await GetCurrentPage(SitePagesUrl.SiteSection106OnlyAffordableHousing(SiteData.SiteId));
        section106OnlyAffordableHousingPage.HasGdsSubmitButton("continue-button", out var continueButton);

        // when
        var section106AdditionalAffordableHousingPage = await TestClient.SubmitButton(
            continueButton,
            (nameof(SiteModel.Section106OnlyAffordableHousing), "false"));

        // then
        section106AdditionalAffordableHousingPage
            .UrlEndWith(SitePagesUrl.SiteSection106AdditionalAffordableHousing(SiteData.SiteId))
            .HasTitle(SitePageTitles.SiteSection106AdditionalAffordableHousing);

        SaveCurrentPage();
    }

    [Fact(Skip = AhpConfig.SkipTest)]
    [Order(7)]
    public async Task Order07_ShouldProvideSection106AdditionalAffordableHousingAndNavigateToSection106CapitalFundingEligibility()
    {
        // given
        var section106AdditionalAffordableHousingPage = await GetCurrentPage(SitePagesUrl.SiteSection106AdditionalAffordableHousing(SiteData.SiteId));
        section106AdditionalAffordableHousingPage.HasGdsSubmitButton("continue-button", out var continueButton);

        // when
        var section106CapitalFundingEligibilityPage = await TestClient.SubmitButton(
            continueButton,
            (nameof(SiteModel.Section106AdditionalAffordableHousing), "true"));

        // then
        section106CapitalFundingEligibilityPage
            .UrlEndWith(SitePagesUrl.SiteSection106CapitalFundingEligibility(SiteData.SiteId))
            .HasTitle(SitePageTitles.SiteSection106CapitalFundingEligibility);

        SaveCurrentPage();
    }

    [Fact(Skip = AhpConfig.SkipTest)]
    [Order(8)]
    public async Task Order08_ShouldProvideSection106CapitalFundingEligibilityAndNavigateToSection106LocalAuthorityConfirmation()
    {
        // given
        var section106CapitalFundingEligibilityPage = await GetCurrentPage(SitePagesUrl.SiteSection106CapitalFundingEligibility(SiteData.SiteId));
        section106CapitalFundingEligibilityPage.HasGdsSubmitButton("continue-button", out var continueButton);

        // when
        var section106LocalAuthorityConfirmationPage = await TestClient.SubmitButton(
            continueButton,
            (nameof(SiteModel.Section106CapitalFundingEligibility), "false"));

        // then
        section106LocalAuthorityConfirmationPage
            .UrlEndWith(SitePagesUrl.SiteSection106LocalAuthorityConfirmation(SiteData.SiteId))
            .HasTitle(SitePageTitles.SiteSection106LocalAuthorityConfirmation);

        SaveCurrentPage();
    }
}
