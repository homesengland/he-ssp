using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using AngleSharp.Html.Dom;
using HE.Investment.AHP.WWW;
using HE.Investment.AHP.WWW.Views.Delivery.Const;
using HE.Investments.AHP.IntegrationTests.Extensions;
using HE.Investments.AHP.IntegrationTests.FillApplication.Data;
using HE.Investments.AHP.IntegrationTests.Framework;
using HE.Investments.AHP.IntegrationTests.Pages;
using HE.Investments.IntegrationTestsFramework;
using HE.Investments.TestsUtils.Extensions;
using Xunit;
using Xunit.Extensions.Ordering;

namespace HE.Investments.AHP.IntegrationTests.FillApplication;

[Order(5)]
[SuppressMessage("xUnit", "xUnit1004", Justification = "Waits for DevOps configuration - #76791")]
public class Order05CompleteDeliveryPhases : AhpIntegrationTest
{
    private readonly DeliveryPhasesData _deliveryPhasesData;

    public Order05CompleteDeliveryPhases(IntegrationTestFixture<Program> fixture)
        : base(fixture)
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
        taskListPage.HasLinkWithId("add-delivery-phases", out var enterDeliveryPhasesSection);

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
            .HasGdsSubmitButton("continue-button", out var continueButton);

        var deliveryPhase = RehabDeliveryPhase.GenerateDeliveryPhase();
        var deliveryPhaseNamePage = await TestClient.SubmitButton(continueButton, ("DeliveryPhaseName", deliveryPhase.Name.ToString()));

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
            BuildDeliveryPhasesPage(DeliveryPhasePagesUrl.BuildActivityType, RehabDeliveryPhase),
            ("TypeOfHomes", deliveryPhase.TypeOfHomes.ToString()));
    }

    [Fact(Skip = AhpConfig.SkipTest)]
    [Order(4)]
    public async Task Order04_ProvideBuildActivityType()
    {
        // given
        var deliveryPhase = RehabDeliveryPhase.GenerateBuildActivityType();

        // when & then
        await TestQuestionPage(
            BuildDeliveryPhasesPage(DeliveryPhasePagesUrl.BuildActivityType, RehabDeliveryPhase),
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
        var inputs = deliveryPhase.DeliveryPhaseHomes.Select(x => ($"HomesToDeliver[{x.Key}]", x.Value.ToString(CultureInfo.InvariantCulture))).ToArray();

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
        // given & when & then
        await TestQuestionPage(
            BuildDeliveryPhasesPage(DeliveryPhasePagesUrl.SummaryOfDelivery, RehabDeliveryPhase),
            DeliveryPageTitles.SummaryOfDelivery,
            BuildDeliveryPhasesPage(DeliveryPhasePagesUrl.AcquisitionMilestone, RehabDeliveryPhase));
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
            ("MilestoneStartAt.Day", deliveryPhase.AcquisitionMilestone.MilestoneDate!.Value.Day.ToString(CultureInfo.InvariantCulture)),
            ("MilestoneStartAt.Month", deliveryPhase.AcquisitionMilestone.MilestoneDate!.Value.Month.ToString(CultureInfo.InvariantCulture)),
            ("MilestoneStartAt.Year", deliveryPhase.AcquisitionMilestone.MilestoneDate!.Value.Year.ToString(CultureInfo.InvariantCulture)),
            ("ClaimMilestonePaymentAt.Day", deliveryPhase.AcquisitionMilestone.PaymentDate!.Value.Day.ToString(CultureInfo.InvariantCulture)),
            ("ClaimMilestonePaymentAt.Month", deliveryPhase.AcquisitionMilestone.PaymentDate!.Value.Month.ToString(CultureInfo.InvariantCulture)),
            ("ClaimMilestonePaymentAt.Year", deliveryPhase.AcquisitionMilestone.PaymentDate!.Value.Year.ToString(CultureInfo.InvariantCulture)));
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
            ("MilestoneStartAt.Day", deliveryPhase.StartOnSiteMilestone.MilestoneDate!.Value.Day.ToString(CultureInfo.InvariantCulture)),
            ("MilestoneStartAt.Month", deliveryPhase.StartOnSiteMilestone.MilestoneDate!.Value.Month.ToString(CultureInfo.InvariantCulture)),
            ("MilestoneStartAt.Year", deliveryPhase.StartOnSiteMilestone.MilestoneDate!.Value.Year.ToString(CultureInfo.InvariantCulture)),
            ("ClaimMilestonePaymentAt.Day", deliveryPhase.StartOnSiteMilestone.PaymentDate!.Value.Day.ToString(CultureInfo.InvariantCulture)),
            ("ClaimMilestonePaymentAt.Month", deliveryPhase.StartOnSiteMilestone.PaymentDate!.Value.Month.ToString(CultureInfo.InvariantCulture)),
            ("ClaimMilestonePaymentAt.Year", deliveryPhase.StartOnSiteMilestone.PaymentDate!.Value.Year.ToString(CultureInfo.InvariantCulture)));
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
            ("MilestoneStartAt.Day", deliveryPhase.CompletionMilestone.MilestoneDate!.Value.Day.ToString(CultureInfo.InvariantCulture)),
            ("MilestoneStartAt.Month", deliveryPhase.CompletionMilestone.MilestoneDate!.Value.Month.ToString(CultureInfo.InvariantCulture)),
            ("MilestoneStartAt.Year", deliveryPhase.CompletionMilestone.MilestoneDate!.Value.Year.ToString(CultureInfo.InvariantCulture)),
            ("ClaimMilestonePaymentAt.Day", deliveryPhase.CompletionMilestone.PaymentDate!.Value.Day.ToString(CultureInfo.InvariantCulture)),
            ("ClaimMilestonePaymentAt.Month", deliveryPhase.CompletionMilestone.PaymentDate!.Value.Month.ToString(CultureInfo.InvariantCulture)),
            ("ClaimMilestonePaymentAt.Year", deliveryPhase.CompletionMilestone.PaymentDate!.Value.Year.ToString(CultureInfo.InvariantCulture)));
    }

    [Fact(Skip = AhpConfig.SkipTest)]
    [Order(11)]
    public async Task Order11_CompleteDeliveryPhase()
    {
        // given
        var deliveryPhase = RehabDeliveryPhase.GenerateCompletionMilestone();
        var continueButton =
            await GivenTestQuestionPage(
                BuildDeliveryPhasesPage(DeliveryPhasePagesUrl.CheckAnswers, deliveryPhase),
                DeliveryPageTitles.CheckAnswers);

        // when
        var deliveryPhasesListPage = await TestClient.SubmitButton(
            continueButton,
            ("IsCompleted", "Yes"));

        // then
        deliveryPhasesListPage
            .UrlEndWith(BuildDeliveryPhasesPage(DeliveryPhasesPagesUrl.List))
            .HasTitle(DeliveryPageTitles.List);
    }

    [Fact(Skip = AhpConfig.SkipTest)]
    [Order(12)]
    public async Task Order12_CompleteDeliveryPhasesSection()
    {
        // given
        var deliveryPhaseListPage = await GetCurrentPage(BuildDeliveryPhasesPage(DeliveryPhasesPagesUrl.List));
        deliveryPhaseListPage
            .UrlEndWith(BuildDeliveryPhasesPage(DeliveryPhasesPagesUrl.List))
            .HasTitle(DeliveryPageTitles.List)
            .HasGdsSubmitButton("continue-button", out var continueButton);

        // when
        var completeDeliveryPhasesPage = await TestClient.SubmitButton(continueButton);

        // then
        completeDeliveryPhasesPage.UrlEndWith(BuildDeliveryPhasesPage(DeliveryPhasesPagesUrl.CompleteDeliveryPhases))
            .HasTitle(DeliveryPageTitles.Complete);
        SaveCurrentPage();
    }

    [Fact(Skip = AhpConfig.SkipTest)]
    [Order(13)]
    public async Task Order13_ConfirmCompleteDeliveryPhasesSection()
    {
        // given
        var completeDeliveryPhasesPage = await GetCurrentPage(BuildDeliveryPhasesPage(DeliveryPhasesPagesUrl.CompleteDeliveryPhases));
        completeDeliveryPhasesPage
            .UrlEndWith(BuildDeliveryPhasesPage(DeliveryPhasesPagesUrl.CompleteDeliveryPhases))
            .HasTitle(DeliveryPageTitles.Complete)
            .HasGdsSubmitButton("continue-button", out var continueButton);

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
            .HasGdsSubmitButton("continue-button", out var continueButton);

        return await TestClient.SubmitButton(continueButton, ("RemoveDeliveryPhaseAnswer", "Yes"));
    }

    private async Task<IDictionary<string, int>> GetHomeTypes(string currentPageUrl)
    {
        var homeTypeListPage = await TestClient.NavigateTo(HomeTypesPagesUrl.List(ApplicationData.ApplicationId));
        var result = homeTypeListPage.GetHomeTypeIds()
            .Select(x => new { Id = x, NumberOfHomes = homeTypeListPage.GetHomeTypeNumberOfHomes(x) })
            .ToDictionary(x => x.Id, x => x.NumberOfHomes);

        await TestClient.NavigateTo(currentPageUrl);

        return result;
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
