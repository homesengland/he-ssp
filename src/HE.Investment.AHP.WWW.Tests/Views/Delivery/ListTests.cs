using AngleSharp.Html.Dom;
using HE.Investment.AHP.Contract.Application;
using HE.Investment.AHP.WWW.Models.Delivery;

using Microsoft.AspNetCore.Routing;

namespace HE.Investment.AHP.WWW.Tests.Views.Delivery;

public class ListTests : AhpViewTestBase
{
    private const string ViewPath = "/Views/Delivery/List.cshtml";

    private const string ApplicationName = "My application";

    private readonly RouteData _routeData = new(new RouteValueDictionary { { "applicationId", "123" } });

    [Fact]
    public async Task ShouldDisplaySaveAndContinueButton_WhenAllDeliveryPhasesAreAdded()
    {
        // given
        var model = new DeliveryListModel(ApplicationName)
        {
            AllowedOperations = [AhpApplicationOperation.Modification],
            UnusedHomeTypesCount = 0,
            DeliveryPhases =
            [
                new DeliveryPhaseItemModel("1", "Phase 1", 2, null, null, null),
            ],
        };

        // when
        var document = await RenderView(model);

        // then
        AssertView(document);
        document.HasElementWithText("a", "Phase 1")
            .ContainsInsetText("All of your homes have been added to delivery phase. If you want to add another delivery phase, edit the homes in an existing phase or remove a phase.")
            .ContainsInsetText("Select save and continue to complete this section.")
            .HasSaveAndContinueButton();
    }

    [Fact]
    public async Task ShouldDisplaySaveAndContinueButtonWithWarning_WhenTooManyDeliveryPhasesAreAdded()
    {
        // given
        var model = new DeliveryListModel(ApplicationName)
        {
            AllowedOperations = [AhpApplicationOperation.Modification],
            UnusedHomeTypesCount = -1,
            DeliveryPhases =
            [
                new DeliveryPhaseItemModel("1", "Phase 1", 2, null, null, null),
            ],
        };

        // when
        var document = await RenderView(model);

        // then
        AssertView(document);
        document.HasElementWithText("a", "Phase 1")
            .ContainsInsetText("You have changed the number of homes you are delivering in this application and have now assigned too many homes to delivery phases.")
            .ContainsInsetText("Remove homes from delivery phases to equal the number of homes you told us you are delivering in scheme information.")
            .HasSaveAndContinueButton();
    }

    [Fact]
    public async Task ShouldDisplayAddButton_WhenThereAreNoDeliveryPhases()
    {
        // given
        var model = new DeliveryListModel(ApplicationName)
        {
            AllowedOperations = [AhpApplicationOperation.Modification],
            UnusedHomeTypesCount = 2,
            DeliveryPhases = [],
        };

        // when
        var document = await RenderView(model);

        // then
        AssertView(document);
        document.HasParagraph("Your delivery phases will appear here once added.")
            .ContainsInsetText("You have 2 homes that you need to add to your delivery phases.")
            .HasLinkButton("Add a delivery phase");
    }

    [Fact]
    public async Task ShouldDisplayAddAnotherButton_WhenThereAreSomeDeliveryPhases()
    {
        // given
        var model = new DeliveryListModel(ApplicationName)
        {
            AllowedOperations = [AhpApplicationOperation.Modification],
            UnusedHomeTypesCount = 1,
            DeliveryPhases =
            [
                new DeliveryPhaseItemModel("1", "Phase 1", 1, null, null, null),
            ],
        };

        // when
        var document = await RenderView(model);

        // then
        AssertView(document);
        document.HasElementWithText("a", "Phase 1")
            .ContainsInsetText("You have 1 homes that you need to add to your delivery phases.")
            .HasLinkButton("Add another delivery phase");
    }

    private static void AssertView(IHtmlDocument document)
    {
        document
            .HasPageHeader(ApplicationName, "Delivery")
            .HasParagraph("View and add the delivery phases for this application and add homes to phases.")
            .HasElementWithText("a", "Return to application");
    }

    private async Task<IHtmlDocument> RenderView(DeliveryListModel model)
    {
        return await Render(ViewPath, model, routeData: _routeData);
    }
}
