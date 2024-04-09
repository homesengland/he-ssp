using AngleSharp.Html.Dom;
using HE.Investment.AHP.Contract.Application;
using HE.Investment.AHP.WWW.Models.Application;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace HE.Investment.AHP.WWW.Tests.Views.Application;

public class NameTests : AhpViewTestBase
{
    private readonly string _viewPath = "/Views/Application/Name.cshtml";

    private readonly ApplicationBasicModel _model = new(null, null, Tenure.Undefined);

    [Fact]
    public async Task ShouldDisplayView()
    {
        // given & when
        var document = await Render(_viewPath, _model);

        // then
        AssertView(document);
    }

    [Fact]
    public async Task ShouldDisplayView_ForInvalidName()
    {
        // given
        var errorMessage = "some test error";
        var modelState = new ModelStateDictionary();
        modelState.AddModelError(nameof(ApplicationBasicModel.Name), errorMessage);

        // when
        var document = await Render(_viewPath, _model, modelStateDictionary: modelState);

        // then
        AssertView(document, errorMessage);
    }

    private static void AssertView(IHtmlDocument document, string? errorMessage = null)
    {
        document
            .HasTitle("Name your application")
            .HasElementWithText("p", "Each application must be for a single tenure. If you are developing a multi-tenure site, each tenure must be applied for within a separate application.")
            .HasElementWithText("p", "Each application needs a unique name. You will not be able to edit this later.")
            .HasElementWithText("p", "You should include the tenure type within your application name. For example, Village Way – Affordable Rent or Village Way – Shared Ownership.")
            .HasContinueButton()
            .HasSummaryErrorMessage(nameof(ApplicationBasicModel.Name), errorMessage, !string.IsNullOrEmpty(errorMessage))
            .HasErrorMessage(nameof(ApplicationBasicModel.Name), errorMessage, !string.IsNullOrEmpty(errorMessage));
    }
}
