using AngleSharp.Html.Dom;
using HE.Investment.AHP.WWW.Models.Scheme;
using HE.Investment.AHP.WWW.Tests.Helpers;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace HE.Investment.AHP.WWW.Tests.Views.Scheme;

public class AffordabilityTests : ViewTestBase
{
    private const string ViewPath = "/Views/Scheme/Affordability.cshtml";
    private const string AffordabilityEvidenceError = "Test error";
    private static readonly SchemeViewModel Model = new("A1", "App Name", "1", null, null, "old evidence");

    [Fact]
    public async Task ShouldDisplayView_WhenThereAreNoErrors()
    {
        // given & when
        var document = await Render(ViewPath, Model);

        // then
        AssertView(document);
        AssertErrors(document, false);
    }

    [Fact]
    public async Task ShouldDisplayView_WhenErrors()
    {
        // given
        var modelState = new ModelStateDictionary();
        modelState.AddModelError(nameof(SchemeViewModel.AffordabilityEvidence), AffordabilityEvidenceError);

        // when
        var document = await Render(ViewPath, Model, modelStateDictionary: modelState);

        // then
        AssertView(document);
        AssertErrors(document, true);
    }

    private static void AssertView(IHtmlDocument document)
    {
        document
            .HasElementWithText("span", Model.ApplicationName)
            .HasElementWithText("h1", "Affordability of Shared Ownership")
            .HasElementWithText("h2", "Tell us about any evidence and analysis you have that the homes will be affordable to the target market")
            .HasInput("AffordabilityEvidence", value: Model.AffordabilityEvidence)
            .HasElementWithText("button", "Save and continue");
    }

    private void AssertErrors(IHtmlDocument document, bool exist)
    {
        document.HasSummaryErrorMessage(nameof(SchemeViewModel.AffordabilityEvidence), AffordabilityEvidenceError, exist);
    }
}
