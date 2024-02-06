using System.Diagnostics.CodeAnalysis;
using HE.Investment.AHP.Contract.Site;
using HE.Investment.AHP.Contract.Site.Enums;
using HE.Investment.AHP.WWW;
using HE.Investment.AHP.WWW.Models.Site;
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
        SiteData.SetSiteId("20");
        await TestQuestionPage(
            SitePagesUrl.SiteSection106GeneralAgreement(SiteData.SiteId),
            SitePageTitles.SiteSection106Agreement,
            SitePagesUrl.SiteSection106AffordableHousing(SiteData.SiteId),
            (nameof(SiteModel.Section106.GeneralAgreement), "True"));
    }

    [Fact(Skip = AhpConfig.SkipTest)]
    [Order(5)]
    public async Task Order05_ShouldProvideSection106AffordableHousingAndNavigateToSection106OnlyAffordableHousing()
    {
        await TestQuestionPage(
            SitePagesUrl.SiteSection106AffordableHousing(SiteData.SiteId),
            SitePageTitles.SiteSection106AffordableHousing,
            SitePagesUrl.SiteSection106OnlyAffordableHousing(SiteData.SiteId),
            (nameof(SiteModel.Section106.AffordableHousing), "True"));
    }

    [Fact(Skip = AhpConfig.SkipTest)]
    [Order(6)]
    public async Task Order06_ShouldProvideSection106OnlyAffordableHousingAndNavigateToSection106AdditionalAffordableHousing()
    {
        await TestQuestionPage(
            SitePagesUrl.SiteSection106OnlyAffordableHousing(SiteData.SiteId),
            SitePageTitles.SiteSection106OnlyAffordableHousing,
            SitePagesUrl.SiteSection106AdditionalAffordableHousing(SiteData.SiteId),
            (nameof(SiteModel.Section106.OnlyAffordableHousing), "False"));
    }

    [Fact(Skip = AhpConfig.SkipTest)]
    [Order(7)]
    public async Task Order07_ShouldProvideSection106AdditionalAffordableHousingAndNavigateToSection106CapitalFundingEligibility()
    {
        await TestQuestionPage(
            SitePagesUrl.SiteSection106AdditionalAffordableHousing(SiteData.SiteId),
            SitePageTitles.SiteSection106AdditionalAffordableHousing,
            SitePagesUrl.SiteSection106CapitalFundingEligibility(SiteData.SiteId),
            (nameof(SiteModel.Section106.AdditionalAffordableHousing), "True"));
    }

    [Fact(Skip = AhpConfig.SkipTest)]
    [Order(8)]
    public async Task Order08_ShouldProvideSection106CapitalFundingEligibilityAndNavigateToSection106LocalAuthorityConfirmation()
    {
        await TestQuestionPage(
            SitePagesUrl.SiteSection106CapitalFundingEligibility(SiteData.SiteId),
            SitePageTitles.SiteSection106CapitalFundingEligibility,
            SitePagesUrl.SiteSection106LocalAuthorityConfirmation(SiteData.SiteId),
            (nameof(SiteModel.Section106.CapitalFundingEligibility), "False"));
    }

    [Fact(Skip = AhpConfig.SkipTest)]
    [Order(9)]
    public async Task Order09_ShouldProvideSection106LocalAuthorityConfirmationAndNavigateToLocalAuthoritySearch()
    {
        await TestQuestionPage(
            SitePagesUrl.SiteSection106LocalAuthorityConfirmation(SiteData.SiteId),
            SitePageTitles.SiteSection106LocalAuthorityConfirmation,
            SitePagesUrl.SiteLocalAuthoritySearch(SiteData.SiteId),
            (nameof(SiteModel.Section106.LocalAuthorityConfirmation), "Local authority confirmed"));
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

    [Fact(Skip = AhpConfig.SkipTest)]
    [Order(12)]
    public async Task Order12_ShouldProvidePlanningStatus()
    {
        await TestQuestionPage(
            SitePagesUrl.SitePlanningStatus(SiteData.SiteId),
            SitePageTitles.PlanningStatus,
            SitePagesUrl.SitePlanningDetails(SiteData.SiteId),
            (nameof(SitePlanningDetails.PlanningStatus), SitePlanningStatus.PlanningDiscussionsUnderwayWithThePlanningOffice.ToString()));
    }

    [Fact(Skip = AhpConfig.SkipTest)]
    [Order(13)]
    public async Task Order13_ShouldProvidePlanningDetails()
    {
        await TestQuestionPage(
            SitePagesUrl.SitePlanningDetails(SiteData.SiteId),
            "Enter when you expect to get detailed planning approval",
            SitePagesUrl.SiteLandRegistry(SiteData.SiteId),
            ("ExpectedPlanningApprovalDate.Day", "1"),
            ("ExpectedPlanningApprovalDate.Month", "12"),
            ("ExpectedPlanningApprovalDate.Year", "2024"),
            (nameof(SitePlanningDetails.IsLandRegistryTitleNumberRegistered), "True"));
    }

    [Fact(Skip = AhpConfig.SkipTest)]
    [Order(14)]
    public async Task Order14_ShouldProvideLandRegistry()
    {
        await TestQuestionPage(
            SitePagesUrl.SiteLandRegistry(SiteData.SiteId),
            SitePageTitles.LandRegistry,
            SitePagesUrl.SiteNationalDesignGuide(SiteData.SiteId),
            (nameof(SitePlanningDetails.LandRegistryTitleNumber), "some title"),
            (nameof(SitePlanningDetails.IsGrantFundingForAllHomesCoveredByTitleNumber), "True"));
    }

    [Fact(Skip = AhpConfig.SkipTest)]
    [Order(15)]
    public async Task Order15_ShouldProvideNationalDesignGuidePriorities()
    {
        await TestQuestionPage(
            SitePagesUrl.SiteNationalDesignGuide(SiteData.SiteId),
            SitePageTitles.NationalDesignGuide,
            SitePagesUrl.SiteNationalDesignGuide(SiteData.SiteId),
            (nameof(NationalDesignGuidePrioritiesModel.DesignPriorities), NationalDesignGuidePriority.Nature.ToString()));
    }

    [Fact(Skip = AhpConfig.SkipTest)]
    [Order(16)]
    public async Task Order16_ShouldProvideTenderingStatus()
    {
        await TestQuestionPage(
            SitePagesUrl.SiteTenderingStatus(SiteData.SiteId),
            SitePageTitles.TenderingStatus,
            SitePagesUrl.SiteContractorDetails(SiteData.SiteId),
            (nameof(SiteTenderingStatusDetails.TenderingStatus), SiteTenderingStatus.ConditionalWorksContract.ToString()));
    }

    [Fact(Skip = AhpConfig.SkipTest)]
    [Order(17)]
    public async Task Order17_ShouldProvideContractorDetails()
    {
        await TestQuestionPage(
            SitePagesUrl.SiteContractorDetails(SiteData.SiteId),
            SitePageTitles.ContractorDetails,
            SitePagesUrl.SiteCheckAnswers(SiteData.SiteId),
            (nameof(SiteTenderingStatusDetails.ContractorName), "traktor john deere"),
            (nameof(SiteTenderingStatusDetails.IsSmeContractor), "False"));
    }
}
