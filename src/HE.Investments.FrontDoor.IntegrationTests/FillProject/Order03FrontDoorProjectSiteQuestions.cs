using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using FluentAssertions;
using HE.Investments.Common.CRM.Extensions;
using HE.Investments.Common.Extensions;
using HE.Investments.Common.WWW.Extensions;
using HE.Investments.FrontDoor.Contract.Project;
using HE.Investments.FrontDoor.Contract.Site;
using HE.Investments.FrontDoor.IntegrationTests.Framework;
using HE.Investments.FrontDoor.IntegrationTests.Pages;
using HE.Investments.FrontDoor.WWW.Views.LocalAuthority.Const;
using HE.Investments.FrontDoor.WWW.Views.Project.Const;
using HE.Investments.FrontDoor.WWW.Views.Site.Const;
using HE.Investments.IntegrationTestsFramework.Assertions;
using HE.Investments.TestsUtils.Extensions;
using Xunit;
using Xunit.Abstractions;
using Xunit.Extensions.Ordering;

namespace HE.Investments.FrontDoor.IntegrationTests.FillProject;

[Order(3)]
[SuppressMessage("xUnit", "xUnit1004", Justification = "Waits for DevOps configuration - #76791")]
public class Order03FrontDoorProjectSiteQuestions : FrontDoorIntegrationTest
{
    public Order03FrontDoorProjectSiteQuestions(FrontDoorIntegrationTestFixture fixture, ITestOutputHelper output)
        : base(fixture, output)
    {
    }

    [Fact(Skip = FrontDoorConfig.SkipTest)]
    [Order(1)]
    public async Task Order01_ProvideIdentifiedSite()
    {
        ProjectData.SwitchIsSiteIdentified();

        await TestQuestionPage(
            ProjectPagesUrl.IdentifiedSite(ProjectData.Id),
            ProjectPageTitles.IdentifiedSite,
            SitePagesUrl.Name(ProjectData.Id),
            (nameof(ProjectDetails.IsSiteIdentified), ProjectData.IsSiteIdentified.MapToTrueFalse()));
    }

    [Fact(Skip = FrontDoorConfig.SkipTest)]
    [Order(2)]
    public async Task Order02_ProvideSiteName()
    {
        // given
        var currentPage = await GetCurrentPage(SitePagesUrl.Name(ProjectData.Id));
        currentPage
            .UrlEndWith(SitePagesUrl.Name(ProjectData.Id))
            .HasTitle(SitePageTitles.Name)
            .HasBackLink(out _)
            .HasSaveAndContinueButton(out var continueButton);

        // when
        var nextPage = await TestClient.SubmitButton(
            continueButton,
            (nameof(SiteDetails.Name), SiteData.GenerateSiteName()));

        // then
        SiteData.SetId(nextPage.Url.GetSiteGuidFromUrl());
        nextPage.UrlEndWith(SitePagesUrl.HomesNumber(ProjectData.Id, SiteData.Id));

        SaveCurrentPage();
    }

    [Fact(Skip = FrontDoorConfig.SkipTest)]
    [Order(3)]
    public async Task Order03_ProvideHomesNumber()
    {
        await TestQuestionPage(
            SitePagesUrl.HomesNumber(ProjectData.Id, SiteData.Id),
            SitePageTitles.HomesNumber,
            SitePagesUrl.LocalAuthoritySearch(ProjectData.Id, SiteData.Id),
            (nameof(SiteDetails.HomesNumber), SiteData.HomesNumber.ToString(CultureInfo.InvariantCulture)));
    }

    [Fact(Skip = FrontDoorConfig.SkipTest)]
    [Order(4)]
    public async Task Order04_SearchLocalAuthority()
    {
        // given
        var currentPage = await GetCurrentPage(SitePagesUrl.LocalAuthoritySearch(ProjectData.Id, SiteData.Id));
        currentPage
            .UrlWithoutQueryEndsWith(SitePagesUrl.LocalAuthoritySearch(ProjectData.Id, SiteData.Id))
            .HasTitle(LocalAuthorityPageTitles.SearchForSite)
            .HasBackLink(out _)
            .HasSubmitButton(out var continueButton, "Search");

        // when
        var nextPage = await TestClient.SubmitButton(
            continueButton,
            ("Phrase", "oxford"));

        // then
        nextPage
            .UrlWithoutQueryEndsWith(SitePagesUrl.LocalAuthorityResult(ProjectData.Id, SiteData.Id))
            .HasTitle(LocalAuthorityPageTitles.SearchResult)
            .HasBackLink(out _);

        SaveCurrentPage();
    }

    [Fact(Skip = FrontDoorConfig.SkipTest)]
    [Order(5)]
    public async Task Order05_SelectLocalAuthority()
    {
        // given
        var useHeTablesParameter = await FeatureManager.GetUseHeTablesParameter();
        var currentPage = await GetCurrentPage(SitePagesUrl.LocalAuthorityResult(ProjectData.Id, SiteData.Id));
        var confirmLocalAuthorityLink = currentPage
            .UrlWithoutQueryEndsWith(SitePagesUrl.LocalAuthorityResult(ProjectData.Id, SiteData.Id))
            .HasTitle(LocalAuthorityPageTitles.SearchResult)
            .HasBackLink(out _)
            .GetLinkByTestId(SiteData.LocalAuthorityName.ToIdTag());

        // when
        var nextPage = await TestClient.NavigateTo(confirmLocalAuthorityLink);

        // then
        nextPage
            .UrlEndWith(SitePagesUrl.LocalAuthorityConfirm(ProjectData.Id, SiteData.Id, SiteData.LocalAuthorityCode(useHeTablesParameter)))
            .HasTitle(SitePageTitles.LocalAuthorityConfirm)
            .HasBackLink(out _)
            .HasSaveAndContinueButton();

        SaveCurrentPage();
    }

    [Fact(Skip = FrontDoorConfig.SkipTest)]
    [Order(6)]
    public async Task Order06_ConfirmLocalAuthority()
    {
        await TestQuestionPage(
            SitePagesUrl.LocalAuthorityConfirmSuffix(ProjectData.Id, SiteData.Id),
            SitePageTitles.LocalAuthorityConfirm,
            SitePagesUrl.PlanningStatus(ProjectData.Id, SiteData.Id),
            ("IsConfirmed", "True"));
    }

    [Fact(Skip = FrontDoorConfig.SkipTest)]
    [Order(6)]
    public async Task Order06_ProvidePlanningStatus()
    {
        await TestQuestionPage(
            SitePagesUrl.PlanningStatus(ProjectData.Id, SiteData.Id),
            SitePageTitles.PlanningStatus,
            SitePagesUrl.AddAnotherSite(ProjectData.Id, SiteData.Id),
            (nameof(SiteDetails.PlanningStatus), SiteData.PlanningStatus.ToString()));
    }

    [Fact(Skip = FrontDoorConfig.SkipTest)]
    [Order(7)]
    public async Task Order07_ProvideAnotherSite()
    {
        await TestQuestionPage(
            SitePagesUrl.AddAnotherSite(ProjectData.Id, SiteData.Id),
            SitePageTitles.AddAnotherSite,
            ProjectPagesUrl.Progress(ProjectData.Id),
            (nameof(SiteDetails.AddAnotherSite), SiteData.AddAnotherSite.MapToCommonResponse()));
    }

    [Fact(Skip = FrontDoorConfig.SkipTest)]
    [Order(8)]
    public async Task Order08_ProvideProgress()
    {
        await TestQuestionPage(
            ProjectPagesUrl.Progress(ProjectData.Id),
            ProjectPageTitles.Progress,
            ProjectPagesUrl.RequiresFunding(ProjectData.Id),
            (nameof(ProjectDetails.IsSupportRequired), ProjectData.IsSupportRequired.MapToTrueFalse()));
    }

    [Fact(Skip = FrontDoorConfig.SkipTest)]
    [Order(9)]
    public async Task Order09_ProvideRequiresFunding()
    {
        await TestQuestionPage(
            ProjectPagesUrl.RequiresFunding(ProjectData.Id),
            ProjectPageTitles.RequiresFunding,
            ProjectPagesUrl.FundingAmount(ProjectData.Id),
            (nameof(ProjectDetails.IsFundingRequired), ProjectData.IsFundingRequired.MapToTrueFalse()));
    }

    [Fact(Skip = FrontDoorConfig.SkipTest)]
    [Order(10)]
    public async Task Order10_ProvideFundingAmount()
    {
        await TestQuestionPage(
            ProjectPagesUrl.FundingAmount(ProjectData.Id),
            ProjectPageTitles.FundingAmount,
            ProjectPagesUrl.Profit(ProjectData.Id),
            (nameof(ProjectDetails.RequiredFunding), ProjectData.RequiredFunding.ToString()));
    }

    [Fact(Skip = FrontDoorConfig.SkipTest)]
    [Order(11)]
    public async Task Order11_ProvideProfit()
    {
        await TestQuestionPage(
            ProjectPagesUrl.Profit(ProjectData.Id),
            ProjectPageTitles.Profit,
            ProjectPagesUrl.ExpectedStart(ProjectData.Id),
            (nameof(ProjectDetails.IsProfit), ProjectData.IsProfit.MapToTrueFalse()));
    }

    [Fact(Skip = FrontDoorConfig.SkipTest)]
    [Order(12)]
    public async Task Order12_ProvideExpectedStart()
    {
        await TestQuestionPage(
            ProjectPagesUrl.ExpectedStart(ProjectData.Id),
            ProjectPageTitles.ExpectedStart,
            ProjectPagesUrl.CheckAnswers(ProjectData.Id),
            ("ExpectedStartDate.Month", ProjectData.ExpectedStartDate.Month.ToString(CultureInfo.InvariantCulture)),
            ("ExpectedStartDate.Year", ProjectData.ExpectedStartDate.Year.ToString(CultureInfo.InvariantCulture)));
    }

    [Fact(Skip = FrontDoorConfig.SkipTest)]
    [Order(13)]
    public async Task Order13_CheckAnswers()
    {
        // given
        var checkAnswersPage = await GetCurrentPage(ProjectPagesUrl.CheckAnswers(ProjectData.Id));
        checkAnswersPage
            .UrlEndWith(ProjectPagesUrl.CheckAnswers(ProjectData.Id))
            .HasTitle(ProjectPageTitles.CheckAnswers)
            .HasBackLink(out _);

        // when & then
        var summary = checkAnswersPage.GetSummaryListItems();
        summary.Should().ContainKey("Project in England").WithValue(ProjectData.IsEnglandHousingDelivery.MapToCommonResponse());
        summary.Should().ContainKey("Project name").WithValue(ProjectData.Name);
        summary.Should().ContainKey("Activities you require support for").WithValue(ProjectData.ActivityType.GetDescription());
        summary.Should().ContainKey("Amount of affordable homes").WithValue(ProjectData.AffordableHomeAmount.GetDescription());
        summary.Should().ContainKey("Previous residential building experience").WithValue(ProjectData.OrganisationHomesBuilt.ToString(CultureInfo.InvariantCulture));

        summary.Should().ContainKey("Identified site").WithValue(ProjectData.IsSiteIdentified.MapToCommonResponse());
        summary.Should().ContainKey("Site name").WithValue(SiteData.Name);
        summary.Should().ContainKey("Number of homes").WithValue(SiteData.HomesNumber.ToString(CultureInfo.InvariantCulture));
        summary.Should().ContainKey("Local authority").WithValue(SiteData.LocalAuthorityName);
        summary.Should().ContainKey("Planning status").WithValue(SiteData.PlanningStatus.GetDescription());

        summary.Should().ContainKey("Project progress more slowly or stall").WithValue(ProjectData.IsSupportRequired.MapToCommonResponse());
        summary.Should().ContainKey("Funding required").WithValue(ProjectData.IsFundingRequired.MapToCommonResponse());
        summary.Should().ContainKey("How much funding").WithValue(ProjectData.RequiredFunding.GetDescription());
        summary.Should().ContainKey("Intention to make a profit").WithValue(ProjectData.IsProfit.MapToCommonResponse());
        summary.Should().ContainKey("Expected project start date").WithValue($"{ProjectData.ExpectedStartDate.Month:00}/{ProjectData.ExpectedStartDate.Year}");
    }

    [Fact(Skip = FrontDoorConfig.SkipTest)]
    [Order(14)]
    public async Task Order14_CheckAnswersChangeSitePlanningStatusAnswer()
    {
        // given
        var checkAnswersPage = await GetCurrentPage(ProjectPagesUrl.CheckAnswers(ProjectData.Id));
        var summary = checkAnswersPage
            .UrlEndWith(ProjectPagesUrl.CheckAnswers(ProjectData.Id))
            .HasTitle(ProjectPageTitles.CheckAnswers)
            .GetSummaryListItems();

        summary.Should().ContainKey("Planning status");
        summary["Planning status"].ChangeAnswerLink.Should().NotBeNull();

        // when
        var planningStatusPage = await TestClient.NavigateTo(summary["Planning status"].ChangeAnswerLink!);

        // then
        planningStatusPage.UrlWithoutQueryEndsWith(SitePagesUrl.PlanningStatus(ProjectData.Id, SiteData.Id))
            .HasTitle(SitePageTitles.PlanningStatus);
        SaveCurrentPage();
    }

    [Fact(Skip = FrontDoorConfig.SkipTest)]
    [Order(15)]
    public async Task Order15_ProvideChangedSitePlanningStatus()
    {
        var newPlanningStatus = SiteData.NewPlanningStatus;
        await TestQuestionPage(
            SitePagesUrl.PlanningStatus(ProjectData.Id, SiteData.Id),
            SitePageTitles.PlanningStatus,
            ProjectPagesUrl.CheckAnswers(ProjectData.Id),
            (nameof(SiteDetails.PlanningStatus), newPlanningStatus.ToString()));
    }

    [Fact(Skip = FrontDoorConfig.SkipTest)]
    [Order(16)]
    public async Task Order16_CheckAnswersHasValidSummaryAfterChangingPlanningStatus()
    {
        // given
        var checkAnswersPage = await GetCurrentPage(ProjectPagesUrl.CheckAnswers(ProjectData.Id));
        checkAnswersPage
            .UrlWithoutQueryEndsWith(ProjectPagesUrl.CheckAnswers(ProjectData.Id))
            .HasTitle(ProjectPageTitles.CheckAnswers);

        // when
        var summary = checkAnswersPage.GetSummaryListItems();
        summary.Should().ContainKey("Planning status").WithValue(SiteData.NewPlanningStatus.GetDescription());
    }

    [Fact(Skip = FrontDoorConfig.SkipTest)]
    [Order(17)]
    public async Task Order17_CheckAnswersCompleteProject()
    {
        // given
        var currentPage = await TestClient.NavigateTo(ProjectPagesUrl.CheckAnswers(ProjectData.Id));
        var continueButton = currentPage
            .UrlEndWith(ProjectPagesUrl.CheckAnswers(ProjectData.Id))
            .HasTitle(ProjectPageTitles.CheckAnswers)
            .GetSubmitButton("Accept and submit");

        // when
        var nextPage = await TestClient.SubmitButton(continueButton);

        // then
        nextPage
            .UrlWithoutQueryNotEndsWith(ProjectPagesUrl.YouNeedToSpeakToHomesEngland(ProjectData.Id));
    }
}
