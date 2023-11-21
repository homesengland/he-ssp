using AngleSharp.Html.Dom;
using HE.Investment.AHP.WWW.Models.HomeTypes;
using HE.Investments.Common.Tests.WWW.Helpers;
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
        var document = await Render(ViewPath, Model);

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
        var document = await Render(ViewPath, Model, modelStateDictionary: modelState);

        // then
        AssertView(document);
        document.HasSummaryErrorMessage("HomeTypeEntity", ErrorMessage);
    }

    private static void AssertView(IHtmlDocument document)
    {
        document
            .HasElementWithText("span", "My application - My homes")
            .HasElementWithText("h1", "Remove My homes Home type")
            .HasElementWithText("p", "Are you sure you want to remove this Home Type?")
            .HasElementWithText("button", "Remove Home Type")
            .HasElementWithText("a", "Go back to Home Types");
    }
}
