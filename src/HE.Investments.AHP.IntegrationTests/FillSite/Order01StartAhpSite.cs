using System.Diagnostics.CodeAnalysis;
using HE.Investment.AHP.Contract.Site;
using HE.Investment.AHP.Contract.Site.Enums;
using HE.Investment.AHP.WWW;
using HE.Investment.AHP.WWW.Models.Site;
using HE.Investment.AHP.WWW.Views.Site.Const;
using HE.Investments.AHP.IntegrationTests.Extensions;
using HE.Investments.AHP.IntegrationTests.Framework;
using HE.Investments.AHP.IntegrationTests.Pages;
using HE.Investments.IntegrationTestsFramework;
using HE.Investments.Loans.Common.Utils.Constants.FormOption;
using HE.Investments.TestsUtils.Extensions;
using Xunit;
using Xunit.Extensions.Ordering;
using SiteTypeDetails = HE.Investment.AHP.Domain.Site.ValueObjects.SiteTypeDetails;

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
        siteNamePage.HasSaveAndContinueButton(out var continueButton);

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
        // given
        var currentPage = await GetCurrentPage(SitePagesUrl.SiteLocalAuthoritySearch(SiteData.SiteId));
        currentPage
            .UrlWithoutQueryEndsWith(SitePagesUrl.SiteLocalAuthoritySearch(SiteData.SiteId))
            .HasTitle(SitePageTitles.LocalAuthoritySearch)
            .HasBackLink()
            .HasSubmitButton(out var searchButton, "Search");

        // when
        var searchResultPage = await TestClient.SubmitButton(searchButton, (nameof(LocalAuthorities.Phrase), SiteData.LocalAuthorityName));

        // then
        searchResultPage.UrlWithoutQueryEndsWith(SitePagesUrl.SiteLocalAuthorityResult(SiteData.SiteId))
            .HasTitle(SitePageTitles.LocalAuthorityResult);
        SaveCurrentPage();
    }

    [Fact(Skip = AhpConfig.SkipTest)]
    [Order(11)]
    public async Task Order11_ShouldSelectLocalAuthorityAndNavigateToLocalAuthorityConfirm()
    {
        // given
        var localAuthorityResultPage = await GetCurrentPage(SitePagesUrl.SiteLocalAuthorityResult(SiteData.SiteId, SiteData.LocalAuthorityName));
        localAuthorityResultPage.HasNavigationListItem("select-list", out var selectLocalAuthorityLink);

        // when
        var localAuthorityConfirmPage = await TestClient.NavigateTo(selectLocalAuthorityLink);

        // then
        localAuthorityConfirmPage
            .UrlEndWith(SitePagesUrl.SiteLocalAuthorityConfirm(SiteData.SiteId, SiteData.LocalAuthorityId, SiteData.LocalAuthorityName))
            .HasTitle(SitePageTitles.LocalAuthorityConfirm);
        SaveCurrentPage();
    }

    [Fact(Skip = AhpConfig.SkipTest)]
    [Order(12)]
    public async Task Order12_ShouldConfirmLocalAuthorityAndNavigateToPlanningStatus()
    {
        // given
        var currentPage = await GetCurrentPage(SitePagesUrl.SiteLocalAuthorityConfirm(SiteData.SiteId, SiteData.LocalAuthorityId, SiteData.LocalAuthorityName));
        currentPage
            .UrlWithoutQueryEndsWith(SitePagesUrl.SiteLocalAuthorityConfirmWithoutQuery(SiteData.SiteId, SiteData.LocalAuthorityId))
            .HasTitle(SitePageTitles.LocalAuthorityConfirm)
            .HasBackLink()
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

    [Fact(Skip = AhpConfig.SkipTest)]
    [Order(13)]
    public async Task Order13_ShouldProvidePlanningStatus()
    {
        await TestQuestionPage(
            SitePagesUrl.SitePlanningStatus(SiteData.SiteId),
            SitePageTitles.PlanningStatus,
            SitePagesUrl.SitePlanningDetails(SiteData.SiteId),
            (nameof(SitePlanningDetails.PlanningStatus), SitePlanningStatus.PlanningDiscussionsUnderwayWithThePlanningOffice.ToString()));
    }

    [Fact(Skip = AhpConfig.SkipTest)]
    [Order(14)]
    public async Task Order14_ShouldProvidePlanningDetails()
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
    [Order(15)]
    public async Task Order15_ShouldProvideLandRegistry()
    {
        await TestQuestionPage(
            SitePagesUrl.SiteLandRegistry(SiteData.SiteId),
            SitePageTitles.LandRegistry,
            SitePagesUrl.SiteNationalDesignGuide(SiteData.SiteId),
            (nameof(SitePlanningDetails.LandRegistryTitleNumber), "some title"),
            (nameof(SitePlanningDetails.IsGrantFundingForAllHomesCoveredByTitleNumber), "True"));
    }

    [Fact(Skip = AhpConfig.SkipTest)]
    [Order(16)]
    public async Task Order16_ShouldProvideNationalDesignGuidePriorities()
    {
        await TestQuestionPage(
            SitePagesUrl.SiteNationalDesignGuide(SiteData.SiteId),
            SitePageTitles.NationalDesignGuide,
            SitePagesUrl.SiteBuildingForHealthyLife(SiteData.SiteId),
            (nameof(NationalDesignGuidePrioritiesModel.DesignPriorities), NationalDesignGuidePriority.Nature.ToString()));
    }

    [Fact(Skip = AhpConfig.SkipTest)]
    [Order(17)]
    public async Task Order17_ShouldProvideBuildingForHealthyLife()
    {
        await TestQuestionPage(
            SitePagesUrl.SiteBuildingForHealthyLife(SiteData.SiteId),
            SitePageTitles.BuildingForHealthyLife,
            SitePagesUrl.SiteProvideNumberOfGreenLights(SiteData.SiteId),
            (nameof(SiteModel.BuildingForHealthyLife), BuildingForHealthyLifeType.Yes.ToString()));
    }

    [Fact(Skip = AhpConfig.SkipTest)]
    [Order(18)]
    public async Task Order18_ShouldProvideNumberOfGreenLights()
    {
        await TestQuestionPage(
            SitePagesUrl.SiteProvideNumberOfGreenLights(SiteData.SiteId),
            SitePageTitles.NumberOfGreenLights,
            SitePagesUrl.SiteLandAcquisitionStatus(SiteData.SiteId),
            (nameof(SiteModel.NumberOfGreenLights), "5"));
    }

    [Fact(Skip = AhpConfig.SkipTest)]
    [Order(19)]
    public async Task Order19_ShouldProvideLandAcquisitionStatus()
    {
        await TestQuestionPage(
            SitePagesUrl.SiteLandAcquisitionStatus(SiteData.SiteId),
            SitePageTitles.LandAcquisitionStatus,
            SitePagesUrl.SiteTenderingStatus(SiteData.SiteId),
            (nameof(SiteModel.LandAcquisitionStatus), nameof(SiteLandAcquisitionStatus.FullOwnership)));
    }

    [Fact(Skip = AhpConfig.SkipTest)]
    [Order(20)]
    public async Task Order20_ShouldProvideTenderingStatus()
    {
        await TestQuestionPage(
            SitePagesUrl.SiteTenderingStatus(SiteData.SiteId),
            SitePageTitles.TenderingStatus,
            SitePagesUrl.SiteContractorDetails(SiteData.SiteId),
            (nameof(SiteTenderingStatusDetails.TenderingStatus), SiteTenderingStatus.ConditionalWorksContract.ToString()));
    }

    [Fact(Skip = AhpConfig.SkipTest)]
    [Order(21)]
    public async Task Order21_ShouldProvideContractorDetails()
    {
        await TestQuestionPage(
            SitePagesUrl.SiteContractorDetails(SiteData.SiteId),
            SitePageTitles.ContractorDetails,
            SitePagesUrl.SiteStrategicSite(SiteData.SiteId),
            (nameof(SiteTenderingStatusDetails.ContractorName), "traktor john deere"),
            (nameof(SiteTenderingStatusDetails.IsSmeContractor), "False"));
    }

    [Fact(Skip = AhpConfig.SkipTest)]
    [Order(22)]
    public async Task Order22_ShouldProvideStrategicSite()
    {
        await TestQuestionPage(
            SitePagesUrl.SiteStrategicSite(SiteData.SiteId),
            SitePageTitles.StrategicSite,
            SitePagesUrl.SiteType(SiteData.SiteId),
            (nameof(StrategicSite.IsStrategicSite), "True"),
            (nameof(StrategicSite.StrategicSiteName), "super-duper ważna strona"));
    }

    [Fact(Skip = AhpConfig.SkipTest)]
    [Order(23)]
    public async Task Order23_ShouldProvideSiteType()
    {
        await TestQuestionPage(
            SitePagesUrl.SiteType(SiteData.SiteId),
            SitePageTitles.SiteType,
            SitePagesUrl.SiteUse(SiteData.SiteId),
            (nameof(SiteTypeDetails.SiteType), SiteType.Brownfield.ToString()),
            (nameof(SiteTypeDetails.IsOnGreenBelt), "True"),
            (nameof(SiteTypeDetails.IsRegenerationSite), "False"));
    }

    [Fact(Skip = AhpConfig.SkipTest)]
    [Order(24)]
    public async Task Order24_ShouldProvideSiteUse()
    {
        await TestQuestionPage(
            SitePagesUrl.SiteUse(SiteData.SiteId),
            SitePageTitles.SiteUse,
            SitePagesUrl.SiteTravellerPitchType(SiteData.SiteId),
            (nameof(SiteUseDetails.IsPartOfStreetFrontInfill), "True"),
            (nameof(SiteUseDetails.IsForTravellerPitchSite), "True"));
    }

    [Fact(Skip = AhpConfig.SkipTest)]
    [Order(25)]
    public async Task Order25_ShouldProvideTravellerPitchType()
    {
        await TestQuestionPage(
            SitePagesUrl.SiteTravellerPitchType(SiteData.SiteId),
            SitePageTitles.TravellerPitchType,
            SitePagesUrl.SiteRuralClassification(SiteData.SiteId),
            (nameof(SiteUseDetails.TravellerPitchSiteType), TravellerPitchSiteType.Permanent.ToString()));
    }

    [Fact(Skip = AhpConfig.SkipTest)]
    [Order(26)]
    public async Task Order26_ShouldProvideRuralClassification()
    {
        await TestQuestionPage(
            SitePagesUrl.SiteRuralClassification(SiteData.SiteId),
            SitePageTitles.RuralClassification,
            SitePagesUrl.SiteProcurements(SiteData.SiteId),
            (nameof(SiteRuralClassification.IsWithinRuralSettlement), "True"));
    }

    [Fact(Skip = AhpConfig.SkipTest)]
    [Order(27)]
    public async Task Order27_ShouldProvideProcurements()
    {
        await TestQuestionPage(
            SitePagesUrl.SiteProcurements(SiteData.SiteId),
            SitePageTitles.Procurements,
            SitePagesUrl.SiteCheckAnswers(SiteData.SiteId),
            (nameof(SiteModel.SiteProcurements), SiteProcurement.PartneringSupplyChain.ToString()));
    }
}
