using AngleSharp.Html.Dom;
using HE.Investment.AHP.WWW.Models.Scheme;
using HE.Investment.AHP.WWW.Tests.Helpers;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace HE.Investment.AHP.WWW.Tests.Views.Scheme;

public class TenureTests : ViewTestBase
{
    private readonly SchemeModel _model = new("1", "test name", string.Empty);
    private readonly string _viewPath = "/Views/Scheme/Tenure.cshtml";

    [Fact]
    public async Task ShouldDisplayView()
    {
        // given & when
        var document = await Render(_viewPath, _model);

        // then
        AssertView(document, _model);
    }

    [Fact]
    public async Task ShouldDisplayView_ForMissingTenure()
    {
        // given
        var errorMessage = "some test error";
        var modelState = new ModelStateDictionary();
        modelState.AddModelError(nameof(SchemeModel.Tenure), errorMessage);

        // when
        var document = await Render(_viewPath, _model, modelStateDictionary: modelState);

        // then
        AssertView(document, _model, errorMessage);
    }

    private static void AssertView(IHtmlDocument document, SchemeModel model, string? errorMessage = null)
    {
        document
            .HasElementWithText("span", model.Name)
            .HasElementWithText("h1", "What is the tenure of the homes on this scheme?")
            .HasSummaryDetails("Tenure refers to the conditions around ownership or rental of the properties")
            .HasElementWithText("button", "Save and continue")
            .HasSummaryErrorMessage(nameof(SchemeModel.Tenure), errorMessage, !string.IsNullOrEmpty(errorMessage))
            .HasErrorMessage(nameof(SchemeModel.Tenure), errorMessage, !string.IsNullOrEmpty(errorMessage));
    }
}
