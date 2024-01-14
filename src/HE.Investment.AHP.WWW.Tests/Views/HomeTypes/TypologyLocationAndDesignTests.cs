using System.Diagnostics.CodeAnalysis;
using AngleSharp.Html.Dom;
using HE.Investment.AHP.WWW.Models.HomeTypes;
using HE.Investments.Common.WWWTestsFramework.Helpers;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace HE.Investment.AHP.WWW.Tests.Views.HomeTypes;

public class TypologyLocationAndDesignTests : HomeTypesTestBase
{
    private const string ViewPath = "/Views/HomeTypes/TypologyLocationAndDesign.cshtml";

    private static readonly MoreInformationModel Model = new("My application", "My homes") { MoreInformation = "My new important information" };

    [Fact]
    [SuppressMessage("Blocker Code Smell", "S2699:Tests should include assertions", Justification = "Error in the Sonarlint library when using AngleSharp when using AngleSharp")]
    public async Task ShouldRenderViewWithTextArea()
    {
        // given & when
        var document = await RenderHomeTypePage(ViewPath, Model);

        // then
        document
            .HasElementWithText("span", "My application - My homes")
            .HasElementWithText("label", "Tell us how the typology, location and design of these homes meet the needs of the intended residents")
            .HasElementWithText("div", "Typology is the classification and characteristics of the homes you are building.")
            .HasElementWithText("div", "You can enter up to 1500 characters")
            .HasTextAreaInput("MoreInformation", value: "My new important information")
            .HasElementWithText("button", "Save and continue");
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
