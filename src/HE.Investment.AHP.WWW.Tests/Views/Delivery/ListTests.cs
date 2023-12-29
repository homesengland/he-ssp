using AngleSharp.Html.Dom;
using HE.Investment.AHP.WWW.Models.Delivery;
using HE.Investments.Common.WWWTestsFramework;
using HE.Investments.Common.WWWTestsFramework.Helpers;
using Microsoft.AspNetCore.Routing;

namespace HE.Investment.AHP.WWW.Tests.Views.Delivery;

public class ListTests : ViewTestBase
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
            IsEditable = true,
            UnusedHomeTypesCount = 0,
            DeliveryPhases = new[]
            {
                new DeliveryPhaseItemModel("1", "Phase 1", 2, null, null, null),
            },
        };

        // when
        var document = await RenderView(model);

        // then
        AssertView(document);
        document.HasElementWithText("a", "Phase 1")
            .HasElementWithText(
                "div",
                "All of your homes have been added to delivery phase. If you want to add another delivery phase, edit the homes in an existing phase or remove a phase.")
            .HasElementWithText("button", "Save and continue");
    }

    [Fact]
    public async Task ShouldDisplayAddButton_WhenThereAreNoDeliveryPhases()
    {
        // given
        var model = new DeliveryListModel(ApplicationName)
        {
            IsEditable = true,
            UnusedHomeTypesCount = 2,
            DeliveryPhases = new List<DeliveryPhaseItemModel>(),
        };

        // when
        var document = await RenderView(model);

        // then
        AssertView(document);
        document.HasElementWithText("p", "Your delivery phases will appear here once added.")
            .HasElementWithText("div", "You have 2 homes that you need to add to your delivery phases.")
            .HasElementWithText("a", "Add a delivery phase");
    }

    [Fact]
    public async Task ShouldDisplayAddAnotherButton_WhenThereAreSomeDeliveryPhases()
    {
        // given
        var model = new DeliveryListModel(ApplicationName)
        {
            IsEditable = true,
            UnusedHomeTypesCount = 1,
            DeliveryPhases = new[]
            {
                new DeliveryPhaseItemModel("1", "Phase 1", 1, null, null, null),
            },
        };

        // when
        var document = await RenderView(model);

        // then
        AssertView(document);
        document.HasElementWithText("a", "Phase 1")
            .HasElementWithText("div", "You have 1 homes that you need to add to your delivery phases.")
            .HasElementWithText("a", "Add another delivery phase");
    }

    private static void AssertView(IHtmlDocument document)
    {
        document
            .HasElementWithText("span", ApplicationName)
            .HasElementWithText("h1", "Delivery")
            .HasElementWithText("p", "View and add the delivery phases for this application and add homes to phases.")
            .HasElementWithText("a", "Return to application");
    }

    private async Task<IHtmlDocument> RenderView(DeliveryListModel model)
    {
        return await Render(ViewPath, model, routeData: _routeData);
    }
}
