using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using HE.Investment.AHP.Contract.Common.Enums;
using HE.Investment.AHP.Contract.Site;
using HE.Investment.AHP.WWW.Views.Site.Const;
using HE.Investments.AHP.IntegrationTests.Framework;
using HE.Investments.AHP.IntegrationTests.Pages;
using HE.Investments.Common.Contract;
using HE.Investments.IntegrationTestsFramework.Assertions;
using HE.Investments.TestsUtils.Extensions;
using Xunit;
using Xunit.Abstractions;
using Xunit.Extensions.Ordering;

namespace HE.Investments.AHP.IntegrationTests.Order02FillSite;

[Order(212)]
[SuppressMessage("xUnit", "xUnit1004", Justification = "Waits for DevOps configuration - #76791")]
public class Order12CheckAnswers : AhpIntegrationTest
{
    public Order12CheckAnswers(AhpIntegrationTestFixture fixture, ITestOutputHelper output)
        : base(fixture, output)
    {
    }

    [Fact(Skip = AhpConfig.SkipTest)]
    [Order(1)]
    public async Task Order01_CheckAnswersHasValidSummary()
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
        summary.Should().ContainKey("Expected detailed planning approval date").WithValue(SiteData.ExpectedPlanningApprovalDate);
        summary.Should().ContainKey("Registered title to the land").WithValue(SiteData.IsLandRegistryTitleNumberRegistered);
        summary.Should().ContainKey("Land Registry title number").WithValue(SiteData.LandRegistryTitleNumber);
        summary.Should().ContainKey("All the homes covered by title number").WithValue(SiteData.IsGrantFundingForAllHomesCoveredByTitleNumber);
        summary.Should().ContainKey("National Design Guide priorities").WithOnlyValues(SiteData.NationalDesignGuidePriorities);
        summary.Should().ContainKey("Building for a Healthy Life criteria").WithValue(SiteData.BuildingForHealthyLife.ToString());
        summary.Should().ContainKey("Number of green lights").WithValue(SiteData.NumberOfGreenLights);
        summary.Should().ContainKey("Developing partner").WithValue(SiteData.DevelopingPartner.Name);
        summary.Should().ContainKey("Owner of the land").WithValue(SiteData.OwnerOfTheLand.Name);
        summary.Should().ContainKey("Owner of the homes").WithValue(SiteData.OwnerOfTheHomes.Name);
        summary.Should().NotContainKey("URB - Owner of the homes");
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
    [Order(2)]
    public async Task Order02_CheckAnswersChangeMmcAnswer()
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
    [Order(3)]
    public async Task Order03_ShouldProvideSiteMmcUsing()
    {
        var mmcUsing = SiteData.ChangeMmcUsingAnswer();
        await TestQuestionPage(
            SitePagesUrl.SiteMmcUsing(SiteData.SiteId),
            SitePageTitles.MmcUsing,
            SitePagesUrl.SiteCheckAnswers(SiteData.SiteId),
            (nameof(SiteModernMethodsOfConstruction.UsingModernMethodsOfConstruction), mmcUsing.ToString()));
    }

    [Fact(Skip = AhpConfig.SkipTest)]
    [Order(4)]
    public async Task Order04_CheckAnswersHasValidSummaryAfterChangingMmc()
    {
        // given
        var checkAnswersPage = await GetCurrentPage(SitePagesUrl.SiteCheckAnswers(SiteData.SiteId));
        checkAnswersPage
            .UrlWithoutQueryEndsWith(SitePagesUrl.SiteCheckAnswers(SiteData.SiteId))
            .HasTitle(SitePageTitles.CheckAnswers);

        // when
        var summary = checkAnswersPage.GetSummaryListItems();
        summary.Should().ContainKey("MMC").WithValue(SiteData.UsingMmc);
        summary.Should().ContainKey("Barriers").WithValue(SiteData.InformationBarriers);
        summary.Should().ContainKey("Impact on developments").WithValue(SiteData.InformationImpact);
        summary.Should().NotContainKey("MMC categories");
        summary.Should().NotContainKey("Sub-categories of 3D primary structural systems");
        summary.Should().NotContainKey("Sub-categories of 2D primary structural systems");
    }

    [Fact(Skip = AhpConfig.SkipTest)]
    [Order(5)]
    public async Task Order05_CheckAnswersCompleteSite()
    {
        await TestQuestionPage(
            SitePagesUrl.SiteCheckAnswers(SiteData.SiteId),
            SitePageTitles.CheckAnswers,
            SitePagesUrl.SiteDetails(ShortGuid.FromString(SiteData.SiteId).Value),
            (nameof(IsSectionCompleted), IsSectionCompleted.Yes.ToString()));
    }

    [Fact(Skip = AhpConfig.SkipTest)]
    [Order(6)]
    public async Task Order06_SiteIsNotEditableAfterCompletion()
    {
        var checkAnswersPage = await GetCurrentPage(SitePagesUrl.SiteCheckAnswers(SiteData.SiteId));

        var summaryItems = checkAnswersPage.GetSummaryListItems();
        foreach (var item in summaryItems)
        {
            item.Value.ChangeAnswerLink.Should().BeNull();
        }
    }
}
