using AngleSharp.Html.Dom;
using HE.Investment.AHP.WWW.Models.HomeTypes;
using HE.Investments.Common.WWWTestsFramework.Helpers;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace HE.Investment.AHP.WWW.Tests.Views.HomeTypes;

public class RemoveHomeTypeConfirmationTests : HomeTypesTestBase
{
    private const string ViewPath = "/Views/HomeTypes/RemoveHomeTypeConfirmation.cshtml";

    private static readonly RemoveHomeTypeModel Model = new("My application", "My homes");

    [Fact]
    public async Task ShouldDisplayView_WhenThereAreNoErrors()
    {
        // given & when
        var document = await RenderHomeTypePage(ViewPath, Model);

        // then
        AssertView(document);
        document.HasSummaryErrorMessage("HomeTypeEntity", ErrorMessage, false);
    }

    [Fact]
    public async Task ShouldDisplayViewWithSummaryError_WhenModelHasError()
    {
        // given
        var modelState = new ModelStateDictionary();
        modelState.AddModelError("HomeTypeEntity", ErrorMessage);

        // when
        var document = await RenderHomeTypePage(ViewPath, Model, modelStateDictionary: modelState);

        // then
        AssertView(document);
        document.HasSummaryErrorMessage("HomeTypeEntity", ErrorMessage);
    }

    private static void AssertView(IHtmlDocument document)
    {
        document
            .HasElementWithText("span", "My application - My homes")
            .HasElementWithText("h1", "Are you sure you want to remove this home type?")
            .HasRadio("RemoveHomeTypeAnswer", new[] { "Yes", "No" })
            .HasElementWithText("button", "Save and continue")
            .HasElementWithText("button", "Save and return to application");
    }
}
