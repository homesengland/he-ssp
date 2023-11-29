using AngleSharp.Html.Dom;
using HE.Investment.AHP.WWW.Models.HomeTypes;
using HE.Investments.Common.WWWTestsFramework.Helpers;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace HE.Investment.AHP.WWW.Tests.Views.HomeTypes;

public class ExitPlanTests : HomeTypesTestBase
{
    private const string ViewPath = "/Views/HomeTypes/ExitPlan.cshtml";

    private static readonly MoreInformationBasicModel BasicModel = new("My application", "My homes") { MoreInformation = "My new important information" };

    [Fact]
    public async Task ShouldRenderViewWithTextArea()
    {
        // given & when
        var document = await RenderHomeTypePage(ViewPath, BasicModel);

        // then
        document
            .HasElementWithText("span", "My application - My homes")
            .HasElementWithText("h1", "Tell us the exit plan or alternative use for these homes should commissioning priorities change in the future")
            .HasElementWithText("span", "You can enter up to 1500 characters")
            .HasInput("MoreInformation", value: "My new important information")
            .HasElementWithText("button", "Save and continue");
    }

    [Fact]
    public async Task ShouldDisplayView_WhenThereIsInputLengthError()
    {
        // given
        var modelState = new ModelStateDictionary();
        modelState.AddModelError("MoreInformation", ErrorMessage);

        // when
        var document = await RenderHomeTypePage(ViewPath, BasicModel, modelStateDictionary: modelState);

        // then
        AssertErrors(document, true);
    }

    private void AssertErrors(IHtmlDocument document, bool exist)
    {
        document.HasSummaryErrorMessage(nameof(MoreInformationBasicModel.MoreInformation), ErrorMessage, exist);
    }
}
