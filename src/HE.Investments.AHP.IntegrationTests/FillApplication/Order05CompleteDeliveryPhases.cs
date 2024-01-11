using System.Diagnostics.CodeAnalysis;
using HE.Investment.AHP.WWW;
using HE.Investment.AHP.WWW.Views.Delivery.Const;
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
    [Order(1)]
    public async Task Order01_DeliveryPhasesLandingPage()
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
            .HasGdsLinkButton("continue-button", out var continueButton);

        (await TestClient.NavigateTo(continueButton)).UrlEndWith(BuildDeliveryPhasesPage(DeliveryPhasesPagesUrl.List));
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

        NewBuildAndWorksOnlyDeliveryPhase.SetDeliveryPhaseId(deliveryPhaseNamePage.Url.GetHomeTypeGuidFromUrl());
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
            BuildDeliveryPhasesPage(DeliveryPhasePagesUrl.AcquisitionMilestone, NewBuildAndWorksOnlyDeliveryPhase),
            ("BuildActivityTypeForNewBuild", deliveryPhase.BuildActivityType.NewBuild.ToString()!));
    }

    private string BuildDeliveryPhasesPage(Func<string, string> deliveryPhasesPageUrlFactory)
    {
        return deliveryPhasesPageUrlFactory(ApplicationData.ApplicationId);
    }

    private string BuildDeliveryPhasesPage(Func<string, string, string> deliveryPhasesPageUrlFactory, INestedItemData nestedItemData)
    {
        return deliveryPhasesPageUrlFactory(ApplicationData.ApplicationId, nestedItemData.Id);
    }
}
