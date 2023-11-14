using AngleSharp.Html.Dom;
using HE.Investment.AHP.WWW.Models.Scheme;
using HE.Investments.WWW.Tests;
using HE.Investments.WWW.Tests.Helpers;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace HE.Investment.AHP.WWW.Tests.Views.Scheme;

public class SalesRiskTests : ViewTestBase
{
    private const string ViewPath = "/Views/Scheme/SalesRisk.cshtml";
    private const string SalesRiskError = "Test error";
    private static readonly SchemeViewModel Model = TestSchemeViewModel.Test;

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
        modelState.AddModelError(nameof(SchemeViewModel.SalesRisk), SalesRiskError);

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
            .HasElementWithText("h1", "Sales risk of Shared Ownership")
            .HasElementWithText("label", "Tell us your assessment of the sales risk and how you will mitigate this")
            .HasInput("SalesRisk", value: Model.SalesRisk)
            .HasElementWithText("button", "Save and continue");
    }

    private void AssertErrors(IHtmlDocument document, bool exist)
    {
        document.HasSummaryErrorMessage(nameof(SchemeViewModel.SalesRisk), SalesRiskError, exist);
    }
}
