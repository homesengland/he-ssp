using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using FluentAssertions;
using HE.Investments.Common.Extensions;
using HE.Investments.Common.WWW.Extensions;
using HE.Investments.FrontDoor.Contract.Project;
using HE.Investments.FrontDoor.Contract.Site;
using HE.Investments.FrontDoor.Contract.Site.Enums;
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
            SitePagesUrl.LocalAuthoritySearch(),
            (nameof(SiteDetails.HomesNumber), SiteData.HomesNumber.ToString(CultureInfo.InvariantCulture)));
    }

    [Fact(Skip = FrontDoorConfig.SkipTest)]
    [Order(4)]
    public async Task Order04_SearchLocalAuthority()
    {
        // given
        var currentPage = await GetCurrentPage(SitePagesUrl.LocalAuthoritySearch());
        currentPage
            .UrlWithoutQueryEndsWith(SitePagesUrl.LocalAuthoritySearch())
            .HasTitle(LocalAuthorityPageTitles.SearchForSite)
            .HasBackLink(out _)
            .HasSubmitButton(out var continueButton, "Search");

        // when
        var nextPage = await TestClient.SubmitButton(
            continueButton,
            ("Phrase", "oxford"));

        // then
        nextPage
            .UrlWithoutQueryEndsWith(SitePagesUrl.LocalAuthorityResult())
            .HasTitle(LocalAuthorityPageTitles.SearchResult)
            .HasBackLink(out _);

        SaveCurrentPage();
    }

    [Fact(Skip = FrontDoorConfig.SkipTest)]
    [Order(5)]
    public async Task Order05_SelectLocalAuthority()
    {
        // given
        var currentPage = await GetCurrentPage(SitePagesUrl.LocalAuthorityResult());
        var confirmLocalAuthorityLink = currentPage
            .UrlWithoutQueryEndsWith(SitePagesUrl.LocalAuthorityResult())
            .HasTitle(LocalAuthorityPageTitles.SearchResult)
            .HasBackLink(out _)
            .GetLinkByTestId(SiteData.LocalAuthorityName.ToIdTag());

        // when
        var nextPage = await TestClient.NavigateTo(confirmLocalAuthorityLink);

        // then
        nextPage
            .UrlEndWith(SitePagesUrl.LocalAuthorityConfirm(ProjectData.Id, SiteData.Id, SiteData.LocalAuthorityCode))
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
        summary.Should()
            .ContainKey("Previous residential building experience")
            .WithValue(ProjectData.OrganisationHomesBuilt.ToString(CultureInfo.InvariantCulture));

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
    public async Task Order17_RedirectToAddAnotherSite()
    {
        // given
        var checkAnswersPage = await GetCurrentPage(ProjectPagesUrl.CheckAnswers(ProjectData.Id));
        var addAnotherSiteLink = checkAnswersPage
            .UrlWithoutQueryEndsWith(ProjectPagesUrl.CheckAnswers(ProjectData.Id))
            .HasTitle(ProjectPageTitles.CheckAnswers)
            .GetLinkByTestId("add-another-site");

        // when
        var nextPage = await TestClient.NavigateTo(addAnotherSiteLink);

        // then
        nextPage
            .UrlWithoutQueryEndsWith(SitePagesUrl.Name(ProjectData.Id))
            .HasTitle(SitePageTitles.Name)
            .HasBackLink(out _)
            .HasSaveAndContinueButton();

        SaveCurrentPage();
    }

    [Fact(Skip = FrontDoorConfig.SkipTest)]
    [Order(18)]
    public async Task Order18_ProvideSecondSiteName()
    {
        // given
        var currentPage = await GetCurrentPage(SitePagesUrl.Name(ProjectData.Id));
        currentPage
            .UrlWithoutQueryEndsWith(SitePagesUrl.Name(ProjectData.Id))
            .HasTitle(SitePageTitles.Name)
            .HasBackLink(out _)
            .HasSaveAndContinueButton(out var continueButton);

        // when
        var nextPage = await TestClient.SubmitButton(
            continueButton,
            (nameof(SiteDetails.Name), SecondSiteData.GenerateSiteName()));

        // then
        SecondSiteData.SetId(nextPage.Url.GetSiteGuidFromUrl());
        nextPage.UrlWithoutQueryEndsWith(SitePagesUrl.HomesNumber(ProjectData.Id, SecondSiteData.Id));

        SaveCurrentPage();
    }

    [Fact(Skip = FrontDoorConfig.SkipTest)]
    [Order(19)]
    public async Task Order19_ProvideSecondSiteHomesNumber()
    {
        await TestQuestionPage(
            SitePagesUrl.HomesNumber(ProjectData.Id, SecondSiteData.Id),
            SitePageTitles.HomesNumber,
            SitePagesUrl.LocalAuthoritySearch(),
            (nameof(SiteDetails.HomesNumber), SecondSiteData.HomesNumber.ToString(CultureInfo.InvariantCulture)));
    }

    [SuppressMessage("Maintainability Rules", "S4144", Justification = "Reviewed")]
    [Fact(Skip = FrontDoorConfig.SkipTest)]
    [Order(20)]
    public async Task Order20_SecondSiteSearchLocalAuthority()
    {
        // given
        var currentPage = await GetCurrentPage(SitePagesUrl.LocalAuthoritySearch());
        currentPage
            .UrlWithoutQueryEndsWith(SitePagesUrl.LocalAuthoritySearch())
            .HasTitle(LocalAuthorityPageTitles.SearchForSite)
            .HasBackLink(out _)
            .HasSubmitButton(out var continueButton, "Search");

        // when
        var nextPage = await TestClient.SubmitButton(
            continueButton,
            ("Phrase", "oxford"));

        // then
        nextPage
            .UrlWithoutQueryEndsWith(SitePagesUrl.LocalAuthorityResult())
            .HasTitle(LocalAuthorityPageTitles.SearchResult)
            .HasBackLink(out _);

        SaveCurrentPage();
    }

    [Fact(Skip = FrontDoorConfig.SkipTest)]
    [Order(21)]
    public async Task Order21_SecondSiteSelectLocalAuthority()
    {
        // given
        var currentPage = await GetCurrentPage(SitePagesUrl.LocalAuthorityResult());
        var confirmLocalAuthorityLink = currentPage
            .UrlWithoutQueryEndsWith(SitePagesUrl.LocalAuthorityResult())
            .HasTitle(LocalAuthorityPageTitles.SearchResult)
            .HasBackLink(out _)
            .GetLinkByTestId(SecondSiteData.LocalAuthorityName.ToIdTag());

        // when
        var nextPage = await TestClient.NavigateTo(confirmLocalAuthorityLink);

        // then
        nextPage
            .UrlWithoutQueryEndsWith(SitePagesUrl.LocalAuthorityConfirmSuffix(ProjectData.Id, SecondSiteData.Id))
            .HasTitle(SitePageTitles.LocalAuthorityConfirm)
            .HasBackLink(out _)
            .HasSaveAndContinueButton();

        SaveCurrentPage();
    }

    [Fact(Skip = FrontDoorConfig.SkipTest)]
    [Order(22)]
    public async Task Order22_SecondSiteConfirmLocalAuthority()
    {
        await TestQuestionPage(
            SitePagesUrl.LocalAuthorityConfirmSuffix(ProjectData.Id, SecondSiteData.Id),
            SitePageTitles.LocalAuthorityConfirm,
            SitePagesUrl.PlanningStatus(ProjectData.Id, SecondSiteData.Id),
            ("IsConfirmed", "True"));
    }

    [Fact(Skip = FrontDoorConfig.SkipTest)]
    [Order(23)]
    public async Task Order23_SecondSiteProvidePlanningStatus()
    {
        await TestQuestionPage(
            SitePagesUrl.PlanningStatus(ProjectData.Id, SecondSiteData.Id),
            SitePageTitles.PlanningStatus,
            ProjectPagesUrl.CheckAnswers(ProjectData.Id),
            (nameof(SiteDetails.PlanningStatus), SiteData.PlanningStatus.ToString()));
    }

    [Fact(Skip = FrontDoorConfig.SkipTest)]
    [Order(24)]
    public async Task Order24_CheckAnswersWithSecondSite()
    {
        // given
        var checkAnswersPage = await GetCurrentPage(ProjectPagesUrl.CheckAnswers(ProjectData.Id));
        checkAnswersPage
            .UrlWithoutQueryEndsWith(ProjectPagesUrl.CheckAnswers(ProjectData.Id))
            .HasTitle(ProjectPageTitles.CheckAnswers)
            .HasBackLink(out _);

        // when & then
        var summary = checkAnswersPage.GetSummaryListItems();
        summary.Any(x => x.Value.Value == SecondSiteData.Name).Should().BeTrue();
        summary.Any(x => x.Value.Value == SiteData.Name).Should().BeTrue();
    }

    [Fact(Skip = FrontDoorConfig.SkipTest)]
    [Order(25)]
    public async Task Order25_RedirectToRemoveSite()
    {
        // given
        var checkAnswersPage = await GetCurrentPage(ProjectPagesUrl.CheckAnswers(ProjectData.Id));
        var removeSiteLink = checkAnswersPage
            .UrlWithoutQueryEndsWith(ProjectPagesUrl.CheckAnswers(ProjectData.Id))
            .HasTitle(ProjectPageTitles.CheckAnswers)
            .GetLinkByTestId($"{SecondSiteData.Name}-remove-site");

        // when
        var nextPage = await TestClient.NavigateTo(removeSiteLink);

        // then
        nextPage
            .UrlWithoutQueryEndsWith(SitePagesUrl.Remove(ProjectData.Id, SecondSiteData.Id))
            .HasTitle(SitePageTitles.Remove)
            .HasBackLink(out _)
            .HasSaveAndContinueButton();

        SaveCurrentPage();
    }

    [Fact(Skip = FrontDoorConfig.SkipTest)]
    [Order(26)]
    public async Task Order26_RemoveSite()
    {
        await TestQuestionPage(
            SitePagesUrl.Remove(ProjectData.Id, SecondSiteData.Id),
            SitePageTitles.Remove,
            ProjectPagesUrl.CheckAnswers(ProjectData.Id),
            (nameof(SiteDetails.RemoveSiteAnswer), RemoveSiteAnswer.Yes.ToString()));
    }

    [Fact(Skip = FrontDoorConfig.SkipTest)]
    [Order(27)]
    public async Task Order27_CheckAnswersWithoutSite()
    {
        // given
        var checkAnswersPage = await GetCurrentPage(ProjectPagesUrl.CheckAnswers(ProjectData.Id));
        checkAnswersPage
            .UrlWithoutQueryEndsWith(ProjectPagesUrl.CheckAnswers(ProjectData.Id))
            .HasTitle(ProjectPageTitles.CheckAnswers)
            .HasBackLink(out _);

        // when & then
        var summary = checkAnswersPage.GetSummaryListItems();
        summary.Any(x => x.Value.Value == SiteData.Name).Should().BeTrue();
        summary.Any(x => x.Value.Value == SecondSiteData.Name).Should().BeFalse();
    }

    [Fact(Skip = FrontDoorConfig.SkipTest)]
    [Order(28)]
    public async Task Order28_CheckAnswersCompleteProject()
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
