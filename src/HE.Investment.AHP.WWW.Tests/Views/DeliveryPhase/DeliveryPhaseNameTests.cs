using AngleSharp.Html.Dom;
using HE.Investment.AHP.Contract.Delivery;
using HE.Investment.AHP.WWW.Models.Delivery;
using HE.Investments.Common.WWWTestsFramework;
using HE.Investments.Common.WWWTestsFramework.Helpers;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Routing;

namespace HE.Investment.AHP.WWW.Tests.Views.Delivery;

public class DeliveryPhaseNameTests : ViewTestBase
{
    private readonly string _viewPath = "/Views/DeliveryPhase/Name.cshtml";

    private readonly RouteData _routeData = new(new RouteValueDictionary { { "applicationId", "123" }, { "deliveryPhaseId", "321" } });

    [Fact]
    public async Task ShouldDisplayView()
    {
        var model = new DeliveryPhaseNameViewModel(Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), "TestApp", string.Empty, "Name");

        // given & when
        var document = await Render(_viewPath, model, routeData: _routeData);

        // then
        AssertView(document);
    }

    [Fact]
    public async Task ShouldDisplayView_ForInvalid()
    {
        // given
        var model = new DeliveryPhaseNameViewModel(Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), "TestApp", "Test phase", "Name");
        var errorMessage = "some test error";
        var modelState = new ModelStateDictionary();
        modelState.AddModelError(nameof(DeliveryPhaseNameViewModel.DeliveryPhaseName), errorMessage);

        // when
        var document = await Render(_viewPath, model, modelStateDictionary: modelState, routeData: _routeData);

        // then
        AssertView(document, errorMessage);
    }

    private static void AssertView(IHtmlDocument document, string? errorMessage = null)
    {
        document
            .HasElementWithText("label", "Name your delivery phase")
            .HasElementWithText("div", "Each delivery phase needs a unique name.")
            .HasElementWithText("button", "Save and continue")
            .HasSummaryErrorMessage(nameof(DeliveryPhaseNameViewModel.DeliveryPhaseName), errorMessage, !string.IsNullOrEmpty(errorMessage))
            .HasErrorMessage(nameof(DeliveryPhaseNameViewModel.DeliveryPhaseName), errorMessage, !string.IsNullOrEmpty(errorMessage));
    }
}
