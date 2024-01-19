using AngleSharp.Html.Dom;
using HE.Investment.AHP.Contract.Application;
using HE.Investment.AHP.WWW.Models.Application;
using HE.Investments.Common.Contract;
using HE.Investments.Common.WWWTestsFramework;

using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace HE.Investment.AHP.WWW.Tests.Views.Application;

public class TenureTests : ViewTestBase
{
    private readonly ApplicationBasicModel _model = new("1", "test name", Tenure.Undefined);
    private readonly string _viewPath = "/Views/Application/Tenure.cshtml";

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
        modelState.AddModelError(nameof(ApplicationBasicModel.Tenure), errorMessage);

        // when
        var document = await Render(_viewPath, _model, modelStateDictionary: modelState);

        // then
        AssertView(document, _model, errorMessage);
    }

    private static void AssertView(IHtmlDocument document, ApplicationBasicModel model, string? errorMessage = null)
    {
        document
            .HasPageHeader(model.Name!, "What is the tenure of the homes on this application?")
            .HasSummaryDetails("Tenure refers to the conditions around ownership or rental of the properties")
            .HasGdsSaveAndContinueButton()
            .HasSummaryErrorMessage(nameof(ApplicationBasicModel.Tenure), errorMessage, !string.IsNullOrEmpty(errorMessage))
            .HasErrorMessage(nameof(ApplicationBasicModel.Tenure), errorMessage, !string.IsNullOrEmpty(errorMessage));
    }
}
