using System.Diagnostics.CodeAnalysis;
using AngleSharp.Html.Dom;
using HE.Investment.AHP.WWW.Models.HomeTypes;

using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace HE.Investment.AHP.WWW.Tests.Views.HomeTypes;

public class MoveOnArrangementsTests : HomeTypesTestBase
{
    private const string ViewPath = "/Views/HomeTypes/MoveOnArrangements.cshtml";

    private static readonly MoreInformationModel Model = new("My application", "My homes") { MoreInformation = "My new important information" };

    [Fact]
    public async Task ShouldRenderViewWithTextArea()
    {
        // given & when
        var document = await RenderHomeTypePage(ViewPath, Model);

        // then
        document
            .HasElementWithText("span", "My application - My homes")
            .HasElementWithText("label", "Tell us the move on arrangements that are in place for when these homes are used as short stay accommodation")
            .HasElementWithText("div", "You can enter up to 1500 characters")
            .HasTextAreaInput("MoreInformation", value: "My new important information")
            .HasGdsSaveAndContinueButton();
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
