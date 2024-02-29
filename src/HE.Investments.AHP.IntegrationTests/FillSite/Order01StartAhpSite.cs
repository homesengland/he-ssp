using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using FluentAssertions;
using HE.Investment.AHP.Contract.Common.Enums;
using HE.Investment.AHP.Contract.Site;
using HE.Investment.AHP.Domain.Site.ValueObjects;
using HE.Investment.AHP.WWW;
using HE.Investment.AHP.WWW.Models.Site;
using HE.Investment.AHP.WWW.Views.Site.Const;
using HE.Investments.AHP.IntegrationTests.Extensions;
using HE.Investments.AHP.IntegrationTests.Framework;
using HE.Investments.AHP.IntegrationTests.Pages;
using HE.Investments.Common.Contract.Constants;
using HE.Investments.IntegrationTestsFramework;
using HE.Investments.IntegrationTestsFramework.Assertions;
using HE.Investments.TestsUtils.Extensions;
using Xunit;
using Xunit.Extensions.Ordering;
using SiteRuralClassification = HE.Investment.AHP.Contract.Site.SiteRuralClassification;
using SiteTypeDetails = HE.Investment.AHP.Domain.Site.ValueObjects.SiteTypeDetails;
using SiteUseDetails = HE.Investment.AHP.Contract.Site.SiteUseDetails;

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
        await TestQuestionPage(
            SitePagesUrl.SiteSection106GeneralAgreement(SiteData.SiteId),
            SitePageTitles.SiteSection106Agreement,
            SitePagesUrl.SiteSection106AffordableHousing(SiteData.SiteId),
            (nameof(SiteModel.Section106.GeneralAgreement), SiteData.Section106GeneralAgreement.ToBoolAnswer()));
    }

    [Fact(Skip = AhpConfig.SkipTest)]
    [Order(5)]
    public async Task Order05_ShouldProvideSection106AffordableHousingAndNavigateToSection106OnlyAffordableHousing()
    {
        await TestQuestionPage(
            SitePagesUrl.SiteSection106AffordableHousing(SiteData.SiteId),
            SitePageTitles.SiteSection106AffordableHousing,
            SitePagesUrl.SiteSection106OnlyAffordableHousing(SiteData.SiteId),
            (nameof(SiteModel.Section106.AffordableHousing), SiteData.Section106AffordableHousing.ToBoolAnswer()));
    }

    [Fact(Skip = AhpConfig.SkipTest)]
    [Order(6)]
    public async Task Order06_ShouldProvideSection106OnlyAffordableHousingAndNavigateToSection106AdditionalAffordableHousing()
    {
        await TestQuestionPage(
            SitePagesUrl.SiteSection106OnlyAffordableHousing(SiteData.SiteId),
            SitePageTitles.SiteSection106OnlyAffordableHousing,
            SitePagesUrl.SiteSection106AdditionalAffordableHousing(SiteData.SiteId),
            (nameof(SiteModel.Section106.OnlyAffordableHousing), SiteData.Section106OnlyAffordableHousing.ToBoolAnswer()));
    }

    [Fact(Skip = AhpConfig.SkipTest)]
    [Order(7)]
    public async Task Order07_ShouldProvideSection106AdditionalAffordableHousingAndNavigateToSection106CapitalFundingEligibility()
    {
        await TestQuestionPage(
            SitePagesUrl.SiteSection106AdditionalAffordableHousing(SiteData.SiteId),
            SitePageTitles.SiteSection106AdditionalAffordableHousing,
            SitePagesUrl.SiteSection106CapitalFundingEligibility(SiteData.SiteId),
            (nameof(SiteModel.Section106.AdditionalAffordableHousing), SiteData.Section106AdditionalAffordableHousing.ToBoolAnswer()));
    }

    [Fact(Skip = AhpConfig.SkipTest)]
    [Order(8)]
    public async Task Order08_ShouldProvideSection106CapitalFundingEligibilityAndNavigateToSection106LocalAuthorityConfirmation()
    {
        await TestQuestionPage(
            SitePagesUrl.SiteSection106CapitalFundingEligibility(SiteData.SiteId),
            SitePageTitles.SiteSection106CapitalFundingEligibility,
            SitePagesUrl.SiteSection106LocalAuthorityConfirmation(SiteData.SiteId),
            (nameof(SiteModel.Section106.CapitalFundingEligibility), SiteData.Section106CapitalFundingEligibility.ToBoolAnswer()));
    }

    [Fact(Skip = AhpConfig.SkipTest)]
    [Order(9)]
    public async Task Order09_ShouldProvideSection106LocalAuthorityConfirmationAndNavigateToLocalAuthoritySearch()
    {
        await TestQuestionPage(
            SitePagesUrl.SiteSection106LocalAuthorityConfirmation(SiteData.SiteId),
            SitePageTitles.SiteSection106LocalAuthorityConfirmation,
            SitePagesUrl.SiteLocalAuthoritySearch(SiteData.SiteId),
            (nameof(SiteModel.Section106.LocalAuthorityConfirmation), SiteData.GenerateLocalAuthorityConfirmation()));
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
            (nameof(SitePlanningDetails.PlanningStatus), SiteData.PlanningStatus.ToString()));
    }

    [Fact(Skip = AhpConfig.SkipTest)]
    [Order(14)]
    public async Task Order14_ShouldProvidePlanningDetails()
    {
        await TestQuestionPage(
            SitePagesUrl.SitePlanningDetails(SiteData.SiteId),
            "Enter when you expect to get detailed planning approval",
            SitePagesUrl.SiteLandRegistry(SiteData.SiteId),
            ("ExpectedPlanningApprovalDate.Day", SiteData.ExpectedPlanningApprovalDate.Day.ToString(CultureInfo.InvariantCulture)),
            ("ExpectedPlanningApprovalDate.Month", SiteData.ExpectedPlanningApprovalDate.Month.ToString(CultureInfo.InvariantCulture)),
            ("ExpectedPlanningApprovalDate.Year", SiteData.ExpectedPlanningApprovalDate.Year.ToString(CultureInfo.InvariantCulture)),
            (nameof(SitePlanningDetails.IsLandRegistryTitleNumberRegistered), SiteData.IsLandRegistryTitleNumberRegistered.ToBoolAnswer()));
    }

    [Fact(Skip = AhpConfig.SkipTest)]
    [Order(15)]
    public async Task Order15_ShouldProvideLandRegistry()
    {
        await TestQuestionPage(
            SitePagesUrl.SiteLandRegistry(SiteData.SiteId),
            SitePageTitles.LandRegistry,
            SitePagesUrl.SiteNationalDesignGuide(SiteData.SiteId),
            (nameof(SitePlanningDetails.LandRegistryTitleNumber), SiteData.GenerateLandRegistryTitleNumber()),
            (nameof(SitePlanningDetails.IsGrantFundingForAllHomesCoveredByTitleNumber), SiteData.IsGrantFundingForAllHomesCoveredByTitleNumber.ToBoolAnswer()));
    }

    [Fact(Skip = AhpConfig.SkipTest)]
    [Order(16)]
    public async Task Order16_ShouldProvideNationalDesignGuidePriorities()
    {
        await TestQuestionPage(
            SitePagesUrl.SiteNationalDesignGuide(SiteData.SiteId),
            SitePageTitles.NationalDesignGuide,
            SitePagesUrl.SiteBuildingForHealthyLife(SiteData.SiteId),
            SiteData.NationalDesignGuidePriorities.ToFormInputs(nameof(NationalDesignGuidePrioritiesModel.DesignPriorities)));
    }

    [Fact(Skip = AhpConfig.SkipTest)]
    [Order(17)]
    public async Task Order17_ShouldProvideBuildingForHealthyLife()
    {
        await TestQuestionPage(
            SitePagesUrl.SiteBuildingForHealthyLife(SiteData.SiteId),
            SitePageTitles.BuildingForHealthyLife,
            SitePagesUrl.SiteProvideNumberOfGreenLights(SiteData.SiteId),
            (nameof(SiteModel.BuildingForHealthyLife), SiteData.BuildingForHealthyLife.ToString()));
    }

    [Fact(Skip = AhpConfig.SkipTest)]
    [Order(18)]
    public async Task Order18_ShouldProvideNumberOfGreenLights()
    {
        await TestQuestionPage(
            SitePagesUrl.SiteProvideNumberOfGreenLights(SiteData.SiteId),
            SitePageTitles.NumberOfGreenLights,
            SitePagesUrl.SiteLandAcquisitionStatus(SiteData.SiteId),
            (nameof(SiteModel.NumberOfGreenLights), SiteData.NumberOfGreenLights));
    }

    [Fact(Skip = AhpConfig.SkipTest)]
    [Order(19)]
    public async Task Order19_ShouldProvideLandAcquisitionStatus()
    {
        await TestQuestionPage(
            SitePagesUrl.SiteLandAcquisitionStatus(SiteData.SiteId),
            SitePageTitles.LandAcquisitionStatus,
            SitePagesUrl.SiteTenderingStatus(SiteData.SiteId),
            (nameof(SiteModel.LandAcquisitionStatus), SiteData.LandAcquisitionStatus.ToString()));
    }

    [Fact(Skip = AhpConfig.SkipTest)]
    [Order(20)]
    public async Task Order20_ShouldProvideTenderingStatus()
    {
        await TestQuestionPage(
            SitePagesUrl.SiteTenderingStatus(SiteData.SiteId),
            SitePageTitles.TenderingStatus,
            SitePagesUrl.SiteContractorDetails(SiteData.SiteId),
            (nameof(SiteTenderingStatusDetails.TenderingStatus), SiteData.TenderingStatus.ToString()));
    }

    [Fact(Skip = AhpConfig.SkipTest)]
    [Order(21)]
    public async Task Order21_ShouldProvideContractorDetails()
    {
        await TestQuestionPage(
            SitePagesUrl.SiteContractorDetails(SiteData.SiteId),
            SitePageTitles.ContractorDetails,
            SitePagesUrl.SiteStrategicSite(SiteData.SiteId),
            (nameof(SiteTenderingStatusDetails.ContractorName), SiteData.GenerateContractorName()),
            (nameof(SiteTenderingStatusDetails.IsSmeContractor), SiteData.IsSmeContractor.ToBoolAnswer()));
    }

    [Fact(Skip = AhpConfig.SkipTest)]
    [Order(22)]
    public async Task Order22_ShouldProvideStrategicSite()
    {
        await TestQuestionPage(
            SitePagesUrl.SiteStrategicSite(SiteData.SiteId),
            SitePageTitles.StrategicSite,
            SitePagesUrl.SiteType(SiteData.SiteId),
            (nameof(StrategicSite.IsStrategicSite), SiteData.IsStrategicSite.ToBoolAnswer()),
            (nameof(StrategicSite.StrategicSiteName), SiteData.GenerateStrategicSiteName()));
    }

    [Fact(Skip = AhpConfig.SkipTest)]
    [Order(23)]
    public async Task Order23_ShouldProvideSiteType()
    {
        await TestQuestionPage(
            SitePagesUrl.SiteType(SiteData.SiteId),
            SitePageTitles.SiteType,
            SitePagesUrl.SiteUse(SiteData.SiteId),
            (nameof(SiteTypeDetails.SiteType), SiteData.SiteType.ToString()),
            (nameof(SiteTypeDetails.IsOnGreenBelt), SiteData.IsOnGreenBelt.ToBoolAnswer()),
            (nameof(SiteTypeDetails.IsRegenerationSite), SiteData.IsRegenerationSite.ToBoolAnswer()));
    }

    [Fact(Skip = AhpConfig.SkipTest)]
    [Order(24)]
    public async Task Order24_ShouldProvideSiteUse()
    {
        await TestQuestionPage(
            SitePagesUrl.SiteUse(SiteData.SiteId),
            SitePageTitles.SiteUse,
            SitePagesUrl.SiteTravellerPitchType(SiteData.SiteId),
            (nameof(SiteUseDetails.IsPartOfStreetFrontInfill), SiteData.IsPartOfStreetFrontInfill.ToBoolAnswer()),
            (nameof(SiteUseDetails.IsForTravellerPitchSite), SiteData.IsForTravellerPitchSite.ToBoolAnswer()));
    }

    [Fact(Skip = AhpConfig.SkipTest)]
    [Order(25)]
    public async Task Order25_ShouldProvideTravellerPitchType()
    {
        await TestQuestionPage(
            SitePagesUrl.SiteTravellerPitchType(SiteData.SiteId),
            SitePageTitles.TravellerPitchType,
            SitePagesUrl.SiteRuralClassification(SiteData.SiteId),
            (nameof(SiteUseDetails.TravellerPitchSiteType), SiteData.TravellerPitchSiteType.ToString()));
    }

    [Fact(Skip = AhpConfig.SkipTest)]
    [Order(26)]
    public async Task Order26_ShouldProvideRuralClassification()
    {
        await TestQuestionPage(
            SitePagesUrl.SiteRuralClassification(SiteData.SiteId),
            SitePageTitles.RuralClassification,
            SitePagesUrl.SiteEnvironmentalImpact(SiteData.SiteId),
            (nameof(SiteRuralClassification.IsWithinRuralSettlement), SiteData.IsWithinRuralSettlement.ToBoolAnswer()),
            (nameof(SiteRuralClassification.IsRuralExceptionSite), SiteData.IsRuralExceptionSite.ToBoolAnswer()));
    }

    [Fact(Skip = AhpConfig.SkipTest)]
    [Order(27)]
    public async Task Order27_ShouldProvideEnvironmentalImpact()
    {
        await TestQuestionPage(
            SitePagesUrl.SiteEnvironmentalImpact(SiteData.SiteId),
            SitePageTitles.EnvironmentalImpact,
            SitePagesUrl.SiteMmcUsing(SiteData.SiteId),
            (nameof(EnvironmentalImpact), SiteData.GenerateEnvironmentalImpact()));
    }

    [Fact(Skip = AhpConfig.SkipTest)]
    [Order(28)]
    public async Task Order28_ShouldProvideSiteMmcUsing()
    {
        await TestQuestionPage(
            SitePagesUrl.SiteMmcUsing(SiteData.SiteId),
            SitePageTitles.MmcUsing,
            SitePagesUrl.SiteMmcInformation(SiteData.SiteId),
            (nameof(SiteModernMethodsOfConstruction.UsingModernMethodsOfConstruction), SiteData.UsingMmc.ToString()));
    }

    [Fact(Skip = AhpConfig.SkipTest)]
    [Order(29)]
    public async Task Order29_ShouldProvideSiteMmcInformation()
    {
        await TestQuestionPage(
            SitePagesUrl.SiteMmcInformation(SiteData.SiteId),
            SitePageTitles.MmcInformation,
            SitePagesUrl.SiteMmcCategories(SiteData.SiteId),
            (nameof(SiteModernMethodsOfConstruction.InformationBarriers), SiteData.GenerateInformationBarriers()),
            (nameof(SiteModernMethodsOfConstruction.InformationImpact), SiteData.GenerateInformationImpact()));
    }

    [Fact(Skip = AhpConfig.SkipTest)]
    [Order(30)]
    public async Task Order30_ShouldProvideSiteMmcCategories()
    {
        await TestQuestionPage(
            SitePagesUrl.SiteMmcCategories(SiteData.SiteId),
            SitePageTitles.MmcCategories,
            SitePagesUrl.SiteMmcCategory3D(SiteData.SiteId),
            SiteData.MmcCategories.ToFormInputs(nameof(SiteModernMethodsOfConstruction.ModernMethodsConstructionCategories)));
    }

    [Fact(Skip = AhpConfig.SkipTest)]
    [Order(31)]
    public async Task Order31_ShouldProvideSiteMmcCategory3D()
    {
        await TestQuestionPage(
            SitePagesUrl.SiteMmcCategory3D(SiteData.SiteId),
            SitePageTitles.Mmc3DCategory,
            SitePagesUrl.SiteMmcCategory2D(SiteData.SiteId),
            (nameof(SiteModernMethodsOfConstruction.ModernMethodsConstruction3DSubcategories), SiteData.Mmc3DSubcategory.ToString()));
    }

    [Fact(Skip = AhpConfig.SkipTest)]
    [Order(32)]
    public async Task Order32_ShouldProvideSiteMmcCategory2D()
    {
        await TestQuestionPage(
            SitePagesUrl.SiteMmcCategory2D(SiteData.SiteId),
            SitePageTitles.Mmc2DCategory,
            SitePagesUrl.SiteProcurements(SiteData.SiteId),
            (nameof(SiteModernMethodsOfConstruction.ModernMethodsConstruction2DSubcategories), SiteData.Mmc2DSubcategory.ToString()));
    }

    [Fact(Skip = AhpConfig.SkipTest)]
    [Order(33)]
    public async Task Order33_ShouldProvideProcurements()
    {
        await TestQuestionPage(
            SitePagesUrl.SiteProcurements(SiteData.SiteId),
            SitePageTitles.Procurements,
            SitePagesUrl.SiteCheckAnswers(SiteData.SiteId),
            SiteData.Procurements.ToFormInputs(nameof(SiteModel.SiteProcurements)));
    }

    [Fact(Skip = AhpConfig.SkipTest)]
    [Order(34)]
    public async Task Order34_CheckAnswersHasValidSummary()
    {
        // given
        var checkAnswersPage = await GetCurrentPage(SitePagesUrl.SiteCheckAnswers(SiteData.SiteId));
        checkAnswersPage
            .UrlEndWith(SitePagesUrl.SiteCheckAnswers(SiteData.SiteId))
            .HasTitle(SitePageTitles.CheckAnswers);

        // when
        var summary = checkAnswersPage.GetSummaryListItems();
        summary.Should().ContainKey("Site name").WithValue(SiteData.SiteName);
        summary.Should().ContainKey("106 agreement").WithValue(SiteData.Section106GeneralAgreement);
        summary.Should().ContainKey("Secure delivery through developer contributions").WithValue(SiteData.Section106AffordableHousing);
        summary.Should().ContainKey("100% affordable housing").WithValue(SiteData.Section106OnlyAffordableHousing);
        summary.Should().ContainKey("Additional affordable housing").WithValue(SiteData.Section106AdditionalAffordableHousing);
        summary.Should().ContainKey("Capital funding guide eligibility").WithValue(SiteData.Section106CapitalFundingEligibility);
        summary.Should().ContainKey("Local authority confirmation").WithValue(SiteData.LocalAuthorityConfirmation);
        summary.Should().ContainKey("Local authority").WithValue(SiteData.LocalAuthorityName);
        summary.Should().ContainKey("Planning status").WithValue(SiteData.PlanningStatus);
        summary.Should().ContainKey("Expected detailed planning approval date").WithValue(SiteData.ExpectedPlanningApprovalDisplayDate);
        summary.Should().ContainKey("Registered title to the land").WithValue(SiteData.IsLandRegistryTitleNumberRegistered);
        summary.Should().ContainKey("Land Registry title number").WithValue(SiteData.LandRegistryTitleNumber);
        summary.Should().ContainKey("All the homes covered by title number").WithValue(SiteData.IsGrantFundingForAllHomesCoveredByTitleNumber);
        summary.Should().ContainKey("National Design Guide priorities").WithOnlyValues(SiteData.NationalDesignGuidePriorities);
        summary.Should().ContainKey("Building for a Healthy Life criteria").WithValue(SiteData.BuildingForHealthyLife.ToString());
        summary.Should().ContainKey("Number of green lights").WithValue(SiteData.NumberOfGreenLights);
        summary.Should().ContainKey("Developing partner").WithValue("TODO");
        summary.Should().ContainKey("Owner of the land").WithValue("TODO");
        summary.Should().ContainKey("Owner of the homes").WithValue("TODO");
        summary.Should().ContainKey("URB - Owner of the homes").WithValue("TODO");
        summary.Should().ContainKey("Land status").WithValue(SiteData.LandAcquisitionStatus);
        summary.Should().ContainKey("Tendering progress for main works contract").WithValue(SiteData.TenderingStatus);
        summary.Should().ContainKey("Name of contractor").WithValue(SiteData.ContractorName);
        summary.Should().ContainKey("Contractor SME").WithValue(SiteData.IsSmeContractor);
        summary.Should().ContainKey("Strategic site").WithValue($"Yes, {SiteData.StrategicSiteName}");
        summary.Should().ContainKey("Site type").WithValue(SiteData.SiteType);
        summary.Should().ContainKey("Green belt").WithValue(SiteData.IsOnGreenBelt);
        summary.Should().ContainKey("Regeneration site").WithValue(SiteData.IsRegenerationSite);
        summary.Should().ContainKey("Street front infill").WithValue(SiteData.IsPartOfStreetFrontInfill);
        summary.Should().ContainKey("Traveller pitch site").WithValue(SiteData.IsForTravellerPitchSite);
        summary.Should().ContainKey("Type of traveller pitch site").WithValue(SiteData.TravellerPitchSiteType);
        summary.Should().ContainKey("Rural settlement").WithValue(SiteData.IsWithinRuralSettlement);
        summary.Should().ContainKey("Rural exception site").WithValue(SiteData.IsRuralExceptionSite);
        summary.Should().ContainKey("Actions taken to reduce environmental impact").WithValue(SiteData.EnvironmentalImpact);
        summary.Should().ContainKey("MMC").WithValue(SiteData.UsingMmc);
        summary.Should().ContainKey("Barriers").WithValue(SiteData.InformationBarriers);
        summary.Should().ContainKey("Impact on developments").WithValue(SiteData.InformationImpact);
        summary.Should().ContainKey("MMC categories").WithOnlyValues(SiteData.MmcCategories);
        summary.Should().ContainKey("Sub-categories of 3D primary structural systems").WithValue(SiteData.Mmc3DSubcategory);
        summary.Should().ContainKey("Sub-categories of 2D primary structural systems").WithValue(SiteData.Mmc2DSubcategory);
        summary.Should().ContainKey("Procurement mechanisms").WithOnlyValues(SiteData.Procurements);
    }

    [Fact(Skip = AhpConfig.SkipTest)]
    [Order(35)]
    public async Task Order35_CheckAnswersChangeMmcAnswer()
    {
        // given
        var checkAnswersPage = await GetCurrentPage(SitePagesUrl.SiteCheckAnswers(SiteData.SiteId));
        var summary = checkAnswersPage
            .UrlEndWith(SitePagesUrl.SiteCheckAnswers(SiteData.SiteId))
            .HasTitle(SitePageTitles.CheckAnswers)
            .GetSummaryListItems();

        summary.Should().ContainKey("MMC");
        summary["MMC"].ChangeAnswerLink.Should().NotBeNull();

        // when
        var mmcPage = await TestClient.NavigateTo(summary["MMC"].ChangeAnswerLink!);

        // then
        mmcPage.UrlWithoutQueryEndsWith(SitePagesUrl.SiteMmcUsing(SiteData.SiteId))
            .HasTitle(SitePageTitles.MmcUsing);
        SaveCurrentPage();
    }

    [Fact(Skip = AhpConfig.SkipTest)]
    [Order(36)]
    public async Task Order36_ShouldProvideSiteMmcUsing()
    {
        var mmcUsing = SiteData.ChangeMmcUsingAnswer();
        await TestQuestionPage(
            SitePagesUrl.SiteMmcUsing(SiteData.SiteId),
            SitePageTitles.MmcUsing,
            SitePagesUrl.SiteCheckAnswers(SiteData.SiteId),
            (nameof(SiteModernMethodsOfConstruction.UsingModernMethodsOfConstruction), mmcUsing.ToString()));
    }

    [Fact(Skip = AhpConfig.SkipTest)]
    [Order(37)]
    public async Task Order37_CheckAnswersHasValidSummaryAfterChangingMmc()
    {
        // given
        var checkAnswersPage = await GetCurrentPage(SitePagesUrl.SiteCheckAnswers(SiteData.SiteId));
        checkAnswersPage
            .UrlEndWith(SitePagesUrl.SiteCheckAnswers(SiteData.SiteId))
            .HasTitle(SitePageTitles.CheckAnswers);

        // when
        var summary = checkAnswersPage.GetSummaryListItems();
        summary.Should().ContainKey("MMC").WithValue(SiteData.UsingMmc);
        summary.Should().NotContainKey("Barriers");
        summary.Should().NotContainKey("Impact on developments");
        summary.Should().NotContainKey("MMC categories");
        summary.Should().NotContainKey("Sub-categories of 3D primary structural systems");
        summary.Should().NotContainKey("Sub-categories of 2D primary structural systems");
    }

    [Fact(Skip = AhpConfig.SkipTest)]
    [Order(35)]
    public async Task Order35_CheckAnswersCompleteSite()
    {
        var siteListPage = await TestQuestionPage(
            SitePagesUrl.SiteCheckAnswers(SiteData.SiteId),
            SitePageTitles.CheckAnswers,
            SitePagesUrl.SiteList,
            (nameof(IsSectionCompleted), IsSectionCompleted.Yes.ToString()));

        siteListPage.HasTitle(SitePageTitles.SiteList)
            .HasLinkWithHref(SitePagesUrl.SiteDetails(SiteData.SiteId), out _);
    }
}
