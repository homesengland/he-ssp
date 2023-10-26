using AngleSharp;
using AngleSharp.Html.Dom;
using HE.Investment.AHP.WWW.Models.Scheme;
using HE.Investment.AHP.WWW.Tests.Helpers;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace HE.Investment.AHP.WWW.Tests.Views.Scheme;

public class NameTests : ViewTestBase
{
    private readonly string _viewPath = "/Views/Scheme/Name.cshtml";

    [Fact]
    public async Task ShouldDisplayView()
    {
        // given & when
        var document = await Render<SchemeModel>(_viewPath);

        // then
        AssertView(document);
    }

    [Fact]
    public async Task ShouldDisplayView_ForInvalidName()
    {
        // given
        var errorMessage = "some test error";
        var modelState = new ModelStateDictionary();
        modelState.AddModelError(nameof(SchemeModel.Name), errorMessage);

        // when
        var document = await Render<SchemeModel>(_viewPath, modelStateDictionary: modelState);

        // then
        AssertView(document, errorMessage);
    }

    private static void AssertView(IHtmlDocument document, string? errorMessage = null)
    {
        document
            .HasElementWithText("h1", "What is the name of the scheme?")
            .HasElementWithText("p", "You should make this something that will help you differentiate")
            .HasElementWithText("button", "Save and continue")
            .HasSummaryErrorMessage(nameof(SchemeModel.Name), errorMessage, !string.IsNullOrEmpty(errorMessage))
            .HasErrorMessage(nameof(SchemeModel.Name), errorMessage, !string.IsNullOrEmpty(errorMessage));
    }
}
