using AngleSharp.Html.Dom;
using HE.Investment.AHP.WWW.Models.HomeTypes;

using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace HE.Investment.AHP.WWW.Tests.Views.HomeTypes;

public class ExemptionJustificationTests : HomeTypesTestBase
{
    private const string ViewPath = "/Views/HomeTypes/ExemptionJustification.cshtml";

    private static readonly MoreInformationModel Model = new("My application", "My homes") { MoreInformation = "My new important information" };

    [Fact]
    public async Task ShouldRenderViewWithTextArea()
    {
        // given & when
        var document = await RenderHomeTypePage(ViewPath, Model);

        // then
        document
            .HasElementWithText("span", "My application - My homes")
            .HasElementWithText("label", "Outline the criteria under which you intend to apply for an exemption to the Right to Shared Ownership")
            .HasElementWithText("div", "You can enter up to 1500 characters")
            .HasElementWithText("a", "Read about the Right to Shared Ownership and the types of property that are in scope (opens in a new tab).")
            .HasTextAreaInput("MoreInformation", value: "My new important information")
            .HasSaveAndContinueButton();
    }

    [Fact]
    public async Task ShouldDisplayView_WhenThereIsInputLengthError()
    {
        // given
        var modelState = new ModelStateDictionary();
        modelState.AddModelError("MoreInformation", ErrorMessage);

        // when
        var document = await RenderHomeTypePage(ViewPath, Model, modelStateDictionary: modelState);

        // then
        AssertErrors(document, true);
    }

    private void AssertErrors(IHtmlDocument document, bool exist)
    {
        document.HasSummaryErrorMessage(nameof(MoreInformationModel.MoreInformation), ErrorMessage, exist);
    }
}
