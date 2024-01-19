using AngleSharp.Html.Dom;
using HE.Investment.AHP.WWW.Models.Delivery;
using HE.Investments.Common.WWWTestsFramework;

using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Routing;

namespace HE.Investment.AHP.WWW.Tests.Views.DeliveryPhase;

public class RemoveDeliveryPhaseConfirmationTests : ViewTestBase
{
    private const string ViewPath = "/Views/DeliveryPhase/RemoveDeliveryPhaseConfirmation.cshtml";

    private static readonly RemoveDeliveryPhaseModel Model = new("My application", "Phase one");

    private readonly RouteData _routeData = new(new RouteValueDictionary { { "applicationId", "123" }, { "deliveryPhaseId", "321" } });

    [Fact]
    public async Task ShouldDisplayView_WhenThereAreNoErrors()
    {
        // given & when
        var document = await RenderView(Model);

        // then
        AssertView(document);
        document.HasSummaryErrorMessage("DeliveryPhaseEntity", "Some Error", false);
    }

    [Fact]
    public async Task ShouldDisplayViewWithSummaryError_WhenModelHasError()
    {
        // given
        var modelState = new ModelStateDictionary();
        modelState.AddModelError("DeliveryPhaseEntity", "Some Error");

        // when
        var document = await RenderView(Model, modelStateDictionary: modelState);

        // then
        AssertView(document);
        document.HasSummaryErrorMessage("DeliveryPhaseEntity", "Some Error");
    }

    private static void AssertView(IHtmlDocument document)
    {
        document
            .HasPageHeader("My application - Phase one", "Are you sure you want to remove this delivery phase?")
            .HasHint("By removing this delivery phase, any homes you have assigned to this phase will need to be added to another phase.")
            .HasRadio("RemoveDeliveryPhaseAnswer", new[] { "Yes", "No" })
            .HasGdsSaveAndContinueButton()
            .HasSaveAndReturnToApplicationLinkButton();
    }

    private async Task<IHtmlDocument> RenderView(RemoveDeliveryPhaseModel model, ModelStateDictionary? modelStateDictionary = null)
    {
        return await Render(ViewPath, model, modelStateDictionary: modelStateDictionary, routeData: _routeData);
    }
}
