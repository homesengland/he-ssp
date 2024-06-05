using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using AngleSharp.Html.Dom;
using FluentAssertions;
using HE.Investment.AHP.WWW.Views.Delivery.Const;
using HE.Investments.AHP.IntegrationTests.Extensions;
using HE.Investments.AHP.IntegrationTests.Framework;
using HE.Investments.AHP.IntegrationTests.Order03FillApplication.Data;
using HE.Investments.AHP.IntegrationTests.Pages;
using HE.Investments.Common.WWW.Extensions;
using HE.Investments.IntegrationTestsFramework.Assertions;
using HE.Investments.TestsUtils.Extensions;
using Xunit;
using Xunit.Abstractions;
using Xunit.Extensions.Ordering;

namespace HE.Investments.AHP.IntegrationTests.Order03FillApplication;

[Order(305)]
[SuppressMessage("xUnit", "xUnit1004", Justification = "Waits for DevOps configuration - #76791")]
public class Order05CompleteDeliveryPhases : AhpIntegrationTest
{
    private readonly DeliveryPhasesData _deliveryPhasesData;

    public Order05CompleteDeliveryPhases(AhpIntegrationTestFixture fixture, ITestOutputHelper output)
        : base(fixture, output)
    {
        var deliveryPhasesData = GetSharedDataOrNull<DeliveryPhasesData>(nameof(_deliveryPhasesData));
        if (deliveryPhasesData is null)
        {
            deliveryPhasesData = new DeliveryPhasesData();
            SetSharedData(nameof(_deliveryPhasesData), deliveryPhasesData);
        }

        _deliveryPhasesData = deliveryPhasesData;
    }

    private RehabDeliveryPhase RehabDeliveryPhase => _deliveryPhasesData.RehabDeliveryPhase;

    [Fact(Skip = AhpConfig.SkipTest)]
    [Order(0)]
    public async Task Order00_DeliveryPhasesLandingPage()
    {
        // given
        var taskListPage = await TestClient.NavigateTo(ApplicationPagesUrl.TaskList(ApplicationData.ApplicationId));
        taskListPage.HasLinkWithTestId("add-delivery-phases", out var enterDeliveryPhasesSection);

        // when
        var landingPage = await TestClient.NavigateTo(enterDeliveryPhasesSection);

        // then
        landingPage
            .UrlEndWith(BuildDeliveryPhasesPage(DeliveryPhasesPagesUrl.LandingPage))
            .HasTitle(DeliveryPageTitles.LandingPage)
            .HasLinkButtonForTestId("continue-button", out var continueButton);

        (await TestClient.NavigateTo(continueButton)).UrlEndWith(BuildDeliveryPhasesPage(DeliveryPhasesPagesUrl.List));
        SaveCurrentPage();
    }

    [Fact(Skip = AhpConfig.SkipTest)]
    [Order(1)]
    public async Task Order01_ClearAllDeliveryPhases()
    {
        // given
        var deliveryPhasesListPage = await TestClient.NavigateTo(DeliveryPhasesPagesUrl.List(ApplicationData.ApplicationId));
        var deliveryPhaseIds = deliveryPhasesListPage.GetDeliveryPhaseIds();

        // when
        foreach (var deliveryPhaseId in deliveryPhaseIds)
        {
            deliveryPhasesListPage = await RemoveDeliveryPhase(deliveryPhasesListPage, deliveryPhaseId);
        }

        // then
        deliveryPhasesListPage.UrlEndWith(DeliveryPhasesPagesUrl.List(ApplicationData.ApplicationId))
            .HasTitle(DeliveryPageTitles.List);
        SaveCurrentPage();
    }

    [Fact(Skip = AhpConfig.SkipTest)]
    [Order(2)]
    public async Task Order02_NewBuildAndWorksOnlyDeliveryPhase()
    {
        // given
        var deliveryPhasesListPage = await TestClient.NavigateTo(DeliveryPhasesPagesUrl.List(ApplicationData.ApplicationId));
        deliveryPhasesListPage.UrlEndWith(DeliveryPhasesPagesUrl.List(ApplicationData.ApplicationId))
            .HasTitle(DeliveryPageTitles.List)
            .HasLinkButtonForTestId("add-delivery-phase", out var enterDeliveryPhasePage);

        // when
        var newDeliveryPhasePage = await TestClient.NavigateTo(enterDeliveryPhasePage);

        // then
        newDeliveryPhasePage
            .UrlEndWith(BuildDeliveryPhasesPage(DeliveryPhasesPagesUrl.NewDeliveryPhase))
            .HasTitle(DeliveryPageTitles.Name)
            .HasContinueButton(out var continueButton);

        var deliveryPhase = RehabDeliveryPhase.GenerateDeliveryPhase();
        var deliveryPhaseNamePage = await TestClient.SubmitButton(continueButton, ("DeliveryPhaseName", deliveryPhase.Name));

        RehabDeliveryPhase.SetDeliveryPhaseId(deliveryPhaseNamePage.Url.GetNestedGuidFromUrl());
        deliveryPhaseNamePage.UrlEndWith(BuildDeliveryPhasesPage(DeliveryPhasePagesUrl.Details, RehabDeliveryPhase));

        SaveCurrentPage();
    }

    [Fact(Skip = AhpConfig.SkipTest)]
    [Order(3)]
    public async Task Order03_ProvideDetails()
    {
        // given
        var deliveryPhase = RehabDeliveryPhase.GenerateDetails();

        // when & then
        await TestQuestionPage(
            BuildDeliveryPhasesPage(DeliveryPhasePagesUrl.Details, RehabDeliveryPhase),
            DeliveryPageTitles.Details,
            BuildDeliveryPhasesPage(DeliveryPhasePagesUrl.RehabBuildActivityType, RehabDeliveryPhase),
            ("TypeOfHomes", deliveryPhase.TypeOfHomes.ToString()));
    }

    [Fact(Skip = AhpConfig.SkipTest)]
    [Order(4)]
    public async Task Order04_ProvideRehabBuildActivityType()
    {
        // given
        var deliveryPhase = RehabDeliveryPhase.GenerateBuildActivityType();

        // when & then
        await TestQuestionPage(
            BuildDeliveryPhasesPage(DeliveryPhasePagesUrl.RehabBuildActivityType, RehabDeliveryPhase),
            DeliveryPageTitles.BuildActivityType,
            BuildDeliveryPhasesPage(DeliveryPhasePagesUrl.ReconfiguringExisting, RehabDeliveryPhase),
            ("BuildActivityType", deliveryPhase.BuildActivityType.ToString()));
    }

    [Fact(Skip = AhpConfig.SkipTest)]
    [Order(5)]
    public async Task Order05_ProvideReconfiguringExisting()
    {
        // given
        var deliveryPhase = RehabDeliveryPhase.GenerateReconfiguringExisting();

        // when & then
        await TestQuestionPage(
            BuildDeliveryPhasesPage(DeliveryPhasePagesUrl.ReconfiguringExisting, RehabDeliveryPhase),
            DeliveryPageTitles.ReconfiguringExisting,
            BuildDeliveryPhasesPage(DeliveryPhasePagesUrl.AddHomes, RehabDeliveryPhase),
            ("ReconfiguringExisting", deliveryPhase.ReconfiguringExisting.ToString()));
    }

    [Fact(Skip = AhpConfig.SkipTest)]
    [Order(6)]
    public async Task Order06_ProvideHomes()
    {
        // given
        var startPageUrl = BuildDeliveryPhasesPage(DeliveryPhasePagesUrl.AddHomes, RehabDeliveryPhase);
        var deliveryPhase = RehabDeliveryPhase.GenerateHomes(await GetHomeTypes(startPageUrl));
        var inputs = deliveryPhase.DeliveryPhaseHomes.Select(x => ($"HomesToDeliver[{x.Id}]", x.NumberOfHomes.ToString(CultureInfo.InvariantCulture))).ToArray();

        // when & then
        await TestQuestionPage(
            BuildDeliveryPhasesPage(DeliveryPhasePagesUrl.AddHomes, RehabDeliveryPhase),
            DeliveryPageTitles.AddHomes,
            BuildDeliveryPhasesPage(DeliveryPhasePagesUrl.SummaryOfDelivery, RehabDeliveryPhase),
            inputs);
    }

    [Fact(Skip = AhpConfig.SkipTest)]
    [Order(7)]
    public async Task Order07_ContinueOnSummaryOfDelivery()
    {
        // given
        var summaryOfDeliveryPhase = await GetCurrentPage(BuildDeliveryPhasesPage(DeliveryPhasePagesUrl.SummaryOfDelivery, RehabDeliveryPhase));
        summaryOfDeliveryPhase
            .UrlEndWith(BuildDeliveryPhasesPage(DeliveryPhasePagesUrl.SummaryOfDelivery, RehabDeliveryPhase))
            .HasTitle(DeliveryPageTitles.SummaryOfDelivery(RehabDeliveryPhase.Name))
            .HasBackLink(out _)
            .HasContinueButton(out var continueButton);

        // when
        var acquisitionMilestonePage = await TestClient.SubmitButton(continueButton);

        // then
        acquisitionMilestonePage
            .UrlEndWith(BuildDeliveryPhasesPage(DeliveryPhasePagesUrl.AcquisitionMilestone, RehabDeliveryPhase))
            .HasTitle(DeliveryPageTitles.AcquisitionMilestone);
        SaveCurrentPage();
    }

    [Fact(Skip = AhpConfig.SkipTest)]
    [Order(8)]
    public async Task Order08_ProvideAcquisitionMilestone()
    {
        // given
        var deliveryPhase = RehabDeliveryPhase.GenerateAcquisitionMilestone();

        // when & then
        await TestQuestionPage(
            BuildDeliveryPhasesPage(DeliveryPhasePagesUrl.AcquisitionMilestone, RehabDeliveryPhase),
            DeliveryPageTitles.AcquisitionMilestone,
            BuildDeliveryPhasesPage(DeliveryPhasePagesUrl.StartOnSiteMilestone, RehabDeliveryPhase),
            ("MilestoneStartAt.Day", deliveryPhase.AcquisitionMilestoneDate.Day!),
            ("MilestoneStartAt.Month", deliveryPhase.AcquisitionMilestoneDate.Month!),
            ("MilestoneStartAt.Year", deliveryPhase.AcquisitionMilestoneDate.Year!),
            ("ClaimMilestonePaymentAt.Day", deliveryPhase.AcquisitionMilestonePaymentDate.Day!),
            ("ClaimMilestonePaymentAt.Month", deliveryPhase.AcquisitionMilestonePaymentDate.Month!),
            ("ClaimMilestonePaymentAt.Year", deliveryPhase.AcquisitionMilestonePaymentDate.Year!));
    }

    [Fact(Skip = AhpConfig.SkipTest)]
    [Order(9)]
    public async Task Order09_ProvideStartOnSiteMilestone()
    {
        // given
        var deliveryPhase = RehabDeliveryPhase.GenerateStartOnSiteMilestone();

        // when & then
        await TestQuestionPage(
            BuildDeliveryPhasesPage(DeliveryPhasePagesUrl.StartOnSiteMilestone, RehabDeliveryPhase),
            DeliveryPageTitles.StartOnSiteMilestone,
            BuildDeliveryPhasesPage(DeliveryPhasePagesUrl.PracticalCompletionMilestone, RehabDeliveryPhase),
            ("MilestoneStartAt.Day", deliveryPhase.StartOnSiteMilestoneDate.Day!),
            ("MilestoneStartAt.Month", deliveryPhase.StartOnSiteMilestoneDate.Month!),
            ("MilestoneStartAt.Year", deliveryPhase.StartOnSiteMilestoneDate.Year!),
            ("ClaimMilestonePaymentAt.Day", deliveryPhase.StartOnSiteMilestonePaymentDate.Day!),
            ("ClaimMilestonePaymentAt.Month", deliveryPhase.StartOnSiteMilestonePaymentDate.Month!),
            ("ClaimMilestonePaymentAt.Year", deliveryPhase.StartOnSiteMilestonePaymentDate.Year!));
    }

    [Fact(Skip = AhpConfig.SkipTest)]
    [Order(10)]
    public async Task Order10_ProvidePracticalCompletionMilestone()
    {
        // given
        var deliveryPhase = RehabDeliveryPhase.GenerateCompletionMilestone();

        // when & then
        await TestQuestionPage(
            BuildDeliveryPhasesPage(DeliveryPhasePagesUrl.PracticalCompletionMilestone, RehabDeliveryPhase),
            DeliveryPageTitles.PracticalCompletionMilestone,
            BuildDeliveryPhasesPage(DeliveryPhasePagesUrl.CheckAnswers, RehabDeliveryPhase),
            ("MilestoneStartAt.Day", deliveryPhase.CompletionMilestoneDate.Day!),
            ("MilestoneStartAt.Month", deliveryPhase.CompletionMilestoneDate.Month!),
            ("MilestoneStartAt.Year", deliveryPhase.CompletionMilestoneDate.Year!),
            ("ClaimMilestonePaymentAt.Day", deliveryPhase.InvalidCompletionMilestonePaymentDate.Day!),
            ("ClaimMilestonePaymentAt.Month", deliveryPhase.InvalidCompletionMilestonePaymentDate.Month!),
            ("ClaimMilestonePaymentAt.Year", deliveryPhase.InvalidCompletionMilestonePaymentDate.Year!));
    }

    [Fact(Skip = AhpConfig.SkipTest)]
    [Order(11)]
    public async Task Order11_CheckAnswersHasValidSummary()
    {
        // given
        var checkAnswersUrl = BuildDeliveryPhasesPage(DeliveryPhasePagesUrl.CheckAnswers, RehabDeliveryPhase);
        var requiredFunding = await GetRequiredFunding(checkAnswersUrl);
        var checkAnswersPage = await GetCurrentPage(checkAnswersUrl);
        checkAnswersPage
            .UrlEndWith(BuildDeliveryPhasesPage(DeliveryPhasePagesUrl.CheckAnswers, RehabDeliveryPhase))
            .HasTitle(DeliveryPageTitles.CheckAnswers)
            .HasSaveAndContinueButton();

        // when
        var summary = checkAnswersPage.GetSummaryListItems();

        // then
        summary.Should().ContainKey("Phase name").WithValue(RehabDeliveryPhase.Name);
        summary.Should().ContainKey("Type of homes").WithValue(RehabDeliveryPhase.TypeOfHomes);
        summary.Should().ContainKey("Build activity type").WithValue(RehabDeliveryPhase.BuildActivityType);
        summary.Should().ContainKey("Reconfiguring existing residential properties").WithValue(RehabDeliveryPhase.ReconfiguringExisting.MapToCommonResponse());

        foreach (var homeType in RehabDeliveryPhase.DeliveryPhaseHomes)
        {
            summary.Should().ContainKey($"Number of homes {homeType.Name}").WithValue(homeType.NumberOfHomes.ToString(CultureInfo.InvariantCulture));
        }

        summary.Should().ContainKey("Grant apportioned to this phase").WhoseValue.Value.Should().BePoundsPences(requiredFunding);
        summary.Should().ContainKey("Acquisition milestone").WhoseValue.Value.Should().BePoundsPences(requiredFunding * 0.4m);
        summary.Should().ContainKey("Start on site milestone").WhoseValue.Value.Should().BePoundsPences(requiredFunding * 0.35m);
        summary.Should().ContainKey("Completion milestone").WhoseValue.Value.Should().BePoundsPences(requiredFunding * 0.25m);
        summary.Should().ContainKey("Acquisition date").WithValue(RehabDeliveryPhase.AcquisitionMilestoneDate);
        summary.Should().ContainKey("Forecast acquisition claim date").WithValue(RehabDeliveryPhase.AcquisitionMilestonePaymentDate);
        summary.Should().ContainKey("Start on site date").WithValue(RehabDeliveryPhase.StartOnSiteMilestoneDate);
        summary.Should().ContainKey("Forecast start on site claim date").WithValue(RehabDeliveryPhase.StartOnSiteMilestonePaymentDate);
        summary.Should().ContainKey("Completion date").WithValue(RehabDeliveryPhase.CompletionMilestoneDate);
        summary.Should().ContainKey("Forecast completion claim date").WithValue(RehabDeliveryPhase.InvalidCompletionMilestonePaymentDate);
    }

    [Fact(Skip = AhpConfig.SkipTest)]
    [Order(12)]
    public async Task Order12_DisplayDeliveryPhaseDatesValidationError()
    {
        // given
        var continueButton = await GivenTestQuestionPage(
            BuildDeliveryPhasesPage(DeliveryPhasePagesUrl.CheckAnswers, RehabDeliveryPhase),
            DeliveryPageTitles.CheckAnswers);

        // when
        var checkAnswersPage = await TestClient.SubmitButton(
            continueButton,
            ("IsSectionCompleted", "Yes"));

        // then
        checkAnswersPage
            .UrlEndWith(BuildDeliveryPhasesPage(DeliveryPhasePagesUrl.CheckAnswers, RehabDeliveryPhase))
            .HasTitle(DeliveryPageTitles.CheckAnswers)
            .HasSummaryErrorMessage("Milestones", text: "The information you have entered doesn't meet what is required, check and try again")
            .HasSaveAndContinueButton();
        SaveCurrentPage();
    }

    [Fact(Skip = AhpConfig.SkipTest)]
    [Order(13)]
    public async Task Order13_ChangePracticalCompletionDate()
    {
        // given
        var currentPage = await GetCurrentPage(BuildDeliveryPhasesPage(DeliveryPhasePagesUrl.CheckAnswers, RehabDeliveryPhase));
        var summary = currentPage
            .UrlWithoutQueryEndsWith(BuildDeliveryPhasesPage(DeliveryPhasePagesUrl.CheckAnswers, RehabDeliveryPhase))
            .HasSummaryItem("Forecast completion claim date")
            .GetSummaryListItems();
        var practicalCompletionMilestonePage = await TestClient.NavigateTo(summary["Forecast completion claim date"].ChangeAnswerLink!);
        practicalCompletionMilestonePage
            .UrlWithoutQueryEndsWith(BuildDeliveryPhasesPage(DeliveryPhasePagesUrl.PracticalCompletionMilestone, RehabDeliveryPhase))
            .HasTitle(DeliveryPageTitles.PracticalCompletionMilestone)
            .HasSaveAndContinueButton(out var continueButton);

        // when
        var checkAnswersPage = await TestClient.SubmitButton(
            continueButton,
            ("MilestoneStartAt.Day", RehabDeliveryPhase.CompletionMilestoneDate.Day!),
            ("MilestoneStartAt.Month", RehabDeliveryPhase.CompletionMilestoneDate.Month!),
            ("MilestoneStartAt.Year", RehabDeliveryPhase.CompletionMilestoneDate.Year!),
            ("ClaimMilestonePaymentAt.Day", RehabDeliveryPhase.CompletionMilestonePaymentDate.Day!),
            ("ClaimMilestonePaymentAt.Month", RehabDeliveryPhase.CompletionMilestonePaymentDate.Month!),
            ("ClaimMilestonePaymentAt.Year", RehabDeliveryPhase.CompletionMilestonePaymentDate.Year!));

        // then
        var updatedSummary = checkAnswersPage
            .UrlWithoutQueryEndsWith(BuildDeliveryPhasesPage(DeliveryPhasePagesUrl.CheckAnswers, RehabDeliveryPhase))
            .HasTitle(DeliveryPageTitles.CheckAnswers)
            .GetSummaryListItems();
        updatedSummary.Should().ContainKey("Forecast completion claim date").WithValue(RehabDeliveryPhase.CompletionMilestonePaymentDate);
        SaveCurrentPage();
    }

    [Fact(Skip = AhpConfig.SkipTest)]
    [Order(14)]
    public async Task Order14_CompleteDeliveryPhase()
    {
        // given
        var continueButton = await GivenTestQuestionPage(
            BuildDeliveryPhasesPage(DeliveryPhasePagesUrl.CheckAnswers, RehabDeliveryPhase),
            DeliveryPageTitles.CheckAnswers);

        // when
        var deliveryPhasesListPage = await TestClient.SubmitButton(
            continueButton,
            ("IsSectionCompleted", "Yes"));

        // then
        deliveryPhasesListPage
            .UrlWithoutQueryEndsWith(BuildDeliveryPhasesPage(DeliveryPhasesPagesUrl.List))
            .HasTitle(DeliveryPageTitles.List);
    }

    [Fact(Skip = AhpConfig.SkipTest)]
    [Order(15)]
    public async Task Order15_CompleteDeliveryPhasesSection()
    {
        // given
        var deliveryPhaseListPage = await GetCurrentPage(BuildDeliveryPhasesPage(DeliveryPhasesPagesUrl.List));
        deliveryPhaseListPage
            .UrlWithoutQueryEndsWith(BuildDeliveryPhasesPage(DeliveryPhasesPagesUrl.List))
            .HasTitle(DeliveryPageTitles.List)
            .HasSaveAndContinueButton(out var continueButton);

        // when
        var completeDeliveryPhasesPage = await TestClient.SubmitButton(continueButton);

        // then
        completeDeliveryPhasesPage.UrlEndWith(BuildDeliveryPhasesPage(DeliveryPhasesPagesUrl.CompleteDeliveryPhases))
            .HasTitle(DeliveryPageTitles.Complete);
        SaveCurrentPage();
    }

    [Fact(Skip = AhpConfig.SkipTest)]
    [Order(16)]
    public async Task Order16_ConfirmCompleteDeliveryPhasesSection()
    {
        // given
        var completeDeliveryPhasesPage = await GetCurrentPage(BuildDeliveryPhasesPage(DeliveryPhasesPagesUrl.CompleteDeliveryPhases));
        completeDeliveryPhasesPage
            .UrlEndWith(BuildDeliveryPhasesPage(DeliveryPhasesPagesUrl.CompleteDeliveryPhases))
            .HasTitle(DeliveryPageTitles.Complete)
            .HasSaveAndContinueButton(out var continueButton);

        // when
        var taskListPage = await TestClient.SubmitButton(continueButton, ("IsDeliveryCompleted", "Yes"));

        // then
        taskListPage.UrlEndWith(ApplicationPagesUrl.TaskList(ApplicationData.ApplicationId))
            .HasSectionWithStatus("add-delivery-phases-status", "Completed");
        SaveCurrentPage();
    }

    private async Task<IHtmlDocument> RemoveDeliveryPhase(IHtmlDocument deliveryPhasesListPage, string deliveryPhaseId)
    {
        // given
        deliveryPhasesListPage.UrlEndWith(DeliveryPhasesPagesUrl.List(ApplicationData.ApplicationId))
            .HasTitle(DeliveryPageTitles.List)
            .HasRemoveDeliveryPhaseLink(deliveryPhaseId, out var removeDeliveryPhaseLink);

        // when
        var removeDeliveryPhasePage = await TestClient.NavigateTo(removeDeliveryPhaseLink);

        // then
        removeDeliveryPhasePage.UrlEndWith(BuildDeliveryPhasesPage(DeliveryPhasePagesUrl.Remove, deliveryPhaseId))
            .HasTitle(DeliveryPageTitles.Remove)
            .HasSaveAndContinueButton(out var continueButton);

        return await TestClient.SubmitButton(continueButton, ("RemoveDeliveryPhaseAnswer", "Yes"));
    }

    private async Task<IList<HomeTypeDetails>> GetHomeTypes(string currentPageUrl)
    {
        var homeTypeListPage = await TestClient.NavigateTo(HomeTypesPagesUrl.List(ApplicationData.ApplicationId));
        var result = homeTypeListPage.GetHomeTypeIds().Select(x => homeTypeListPage.GetHomeTypeDetails(x)).ToList();

        await TestClient.NavigateTo(currentPageUrl);

        return result;
    }

    private async Task<decimal> GetRequiredFunding(string currentPageUrl)
    {
        var fundingDetailsPage = await TestClient.NavigateTo(SchemeInformationPagesUrl.FundingDetails(ApplicationData.ApplicationId));
        fundingDetailsPage.HasInput("RequiredFunding", out var requiredFunding);

        await TestClient.NavigateTo(currentPageUrl);

        return int.Parse(requiredFunding.Value, CultureInfo.InvariantCulture);
    }

    private string BuildDeliveryPhasesPage(Func<string, string> deliveryPhasesPageUrlFactory)
    {
        return deliveryPhasesPageUrlFactory(ApplicationData.ApplicationId);
    }

    private string BuildDeliveryPhasesPage(Func<string, string, string> deliveryPhasesPageUrlFactory, string deliveryPhaseId)
    {
        return deliveryPhasesPageUrlFactory(ApplicationData.ApplicationId, deliveryPhaseId);
    }

    private string BuildDeliveryPhasesPage(Func<string, string, string> deliveryPhasesPageUrlFactory, INestedItemData nestedItemData)
    {
        return BuildDeliveryPhasesPage(deliveryPhasesPageUrlFactory, nestedItemData.Id);
    }
}
