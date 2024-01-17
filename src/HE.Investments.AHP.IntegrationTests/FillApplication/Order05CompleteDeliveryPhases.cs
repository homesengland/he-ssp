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

    private NewBuildAndWorksOnlyDeliveryPhase NewBuildAndWorksOnlyDeliveryPhase => _deliveryPhasesData.NewBuildAndWorksOnlyDeliveryPhase;

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

        var deliveryPhase = NewBuildAndWorksOnlyDeliveryPhase.GenerateDeliveryPhase();
        var deliveryPhaseNamePage = await TestClient.SubmitButton(continueButton, ("DeliveryPhaseName", deliveryPhase.Name.ToString()!));

        NewBuildAndWorksOnlyDeliveryPhase.SetDeliveryPhaseId(deliveryPhaseNamePage.Url.GetNestedGuidFromUrl());
        deliveryPhaseNamePage.UrlEndWith(BuildDeliveryPhasesPage(DeliveryPhasePagesUrl.Details, NewBuildAndWorksOnlyDeliveryPhase));

        SaveCurrentPage();
    }

    [Fact(Skip = AhpConfig.SkipTest)]
    [Order(3)]
    public async Task Order03_ProvideDetails()
    {
        // given
        var deliveryPhase = NewBuildAndWorksOnlyDeliveryPhase.GenerateDetails();

        // when & then
        await TestQuestionPage(
            BuildDeliveryPhasesPage(DeliveryPhasePagesUrl.Details, NewBuildAndWorksOnlyDeliveryPhase),
            DeliveryPageTitles.Details,
            BuildDeliveryPhasesPage(DeliveryPhasePagesUrl.BuildActivityType, NewBuildAndWorksOnlyDeliveryPhase),
            ("TypeOfHomes", deliveryPhase.TypeOfHomes.ToString()));
    }

    [Fact(Skip = AhpConfig.SkipTest)]
    [Order(4)]
    public async Task Order04_ProvideBuildActivityType()
    {
        // given
        var deliveryPhase = NewBuildAndWorksOnlyDeliveryPhase.GenerateBuildActivityType();

        // when & then
        await TestQuestionPage(
            BuildDeliveryPhasesPage(DeliveryPhasePagesUrl.BuildActivityType, NewBuildAndWorksOnlyDeliveryPhase),
            DeliveryPageTitles.BuildActivityType,
            BuildDeliveryPhasesPage(DeliveryPhasePagesUrl.ReconfiguringExisting, NewBuildAndWorksOnlyDeliveryPhase),
            ("BuildActivityType", deliveryPhase.BuildActivityType.ToString()));
    }

    [Fact(Skip = AhpConfig.SkipTest)]
    [Order(5)]
    public async Task Order05_ProvideReconfiguringExisting()
    {
        // given
        var deliveryPhase = NewBuildAndWorksOnlyDeliveryPhase.GenerateReconfiguringExisting();

        // when & then
        await TestQuestionPage(
            BuildDeliveryPhasesPage(DeliveryPhasePagesUrl.ReconfiguringExisting, NewBuildAndWorksOnlyDeliveryPhase),
            DeliveryPageTitles.ReconfiguringExisting,
            BuildDeliveryPhasesPage(DeliveryPhasePagesUrl.AddHomes, NewBuildAndWorksOnlyDeliveryPhase),
            ("ReconfiguringExisting", deliveryPhase.ReconfiguringExisting.ToString()));
    }

    [Fact(Skip = AhpConfig.SkipTest)]
    [Order(6)]
    public async Task Order06_ProvideHomes()
    {
        // given
        var startPageUrl = BuildDeliveryPhasesPage(DeliveryPhasePagesUrl.AddHomes, NewBuildAndWorksOnlyDeliveryPhase);
        var deliveryPhase = NewBuildAndWorksOnlyDeliveryPhase.GenerateHomes(await GetHomeTypes(startPageUrl));
        var inputs = deliveryPhase.DeliveryPhaseHomes.Select(x => ($"HomesToDeliver[{x.Key}]", x.Value.ToString(CultureInfo.InvariantCulture))).ToArray();

        // when & then
        await TestQuestionPage(
            BuildDeliveryPhasesPage(DeliveryPhasePagesUrl.AddHomes, NewBuildAndWorksOnlyDeliveryPhase),
            DeliveryPageTitles.AddHomes,
            BuildDeliveryPhasesPage(DeliveryPhasePagesUrl.SummaryOfDelivery, NewBuildAndWorksOnlyDeliveryPhase),
            inputs);
    }

    [Fact(Skip = AhpConfig.SkipTest)]
    [Order(7)]
    public async Task Order07_ContinueOnSummaryOfDelivery()
    {
        // given & when & then
        await TestQuestionPage(
            BuildDeliveryPhasesPage(DeliveryPhasePagesUrl.SummaryOfDelivery, NewBuildAndWorksOnlyDeliveryPhase),
            DeliveryPageTitles.SummaryOfDelivery,
            BuildDeliveryPhasesPage(DeliveryPhasePagesUrl.AcquisitionMilestone, NewBuildAndWorksOnlyDeliveryPhase));
    }

    [Fact(Skip = AhpConfig.SkipTest)]
    [Order(8)]
    public async Task Order08_ProvideAcquisitionMilestone()
    {
        // given
        var deliveryPhase = NewBuildAndWorksOnlyDeliveryPhase.GenerateAcquisitionMilestone();

        // when & then
        await TestQuestionPage(
            BuildDeliveryPhasesPage(DeliveryPhasePagesUrl.AcquisitionMilestone, NewBuildAndWorksOnlyDeliveryPhase),
            DeliveryPageTitles.AcquisitionMilestone,
            BuildDeliveryPhasesPage(DeliveryPhasePagesUrl.StartOnSiteMilestone, NewBuildAndWorksOnlyDeliveryPhase),
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
        var deliveryPhase = NewBuildAndWorksOnlyDeliveryPhase.GenerateStartOnSiteMilestone();

        // when & then
        await TestQuestionPage(
            BuildDeliveryPhasesPage(DeliveryPhasePagesUrl.StartOnSiteMilestone, NewBuildAndWorksOnlyDeliveryPhase),
            DeliveryPageTitles.StartOnSiteMilestone,
            BuildDeliveryPhasesPage(DeliveryPhasePagesUrl.PracticalCompletionMilestone, NewBuildAndWorksOnlyDeliveryPhase),
            ("MilestoneStartAt.Day", deliveryPhase.StartOnSiteMilestone.MilestoneDate!.Value.Day.ToString(CultureInfo.InvariantCulture)),
            ("MilestoneStartAt.Month", deliveryPhase.StartOnSiteMilestone.MilestoneDate!.Value.Month.ToString(CultureInfo.InvariantCulture)),
            ("MilestoneStartAt.Year", deliveryPhase.StartOnSiteMilestone.MilestoneDate!.Value.Year.ToString(CultureInfo.InvariantCulture)),
            ("ClaimMilestonePaymentAt.Day", deliveryPhase.StartOnSiteMilestone.PaymentDate!.Value.Day.ToString(CultureInfo.InvariantCulture)),
            ("ClaimMilestonePaymentAt.Month", deliveryPhase.StartOnSiteMilestone.PaymentDate!.Value.Month.ToString(CultureInfo.InvariantCulture)),
            ("ClaimMilestonePaymentAt.Year", deliveryPhase.StartOnSiteMilestone.PaymentDate!.Value.Year.ToString(CultureInfo.InvariantCulture)));
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
