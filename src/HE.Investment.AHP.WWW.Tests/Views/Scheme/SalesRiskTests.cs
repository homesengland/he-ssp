using AngleSharp.Html.Dom;
using HE.Investment.AHP.WWW.Models.Scheme;
using HE.Investments.Common.WWWTestsFramework;

using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Routing;

namespace HE.Investment.AHP.WWW.Tests.Views.Scheme;

public class SalesRiskTests : ViewTestBase
{
    private const string ViewPath = "/Views/Scheme/SalesRisk.cshtml";
    private const string SalesRiskError = "Test error";
    private static readonly SchemeViewModel Model = TestSchemeViewModel.Test;
    private readonly RouteData _routeData = new(new RouteValueDictionary { { "applicationId", "123" } });

    [Fact]
    public async Task ShouldDisplayView_WhenThereAreNoErrors()
    {
        // given & when
        var document = await Render(ViewPath, Model, routeData: _routeData);

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
        var document = await Render(ViewPath, Model, modelStateDictionary: modelState, routeData: _routeData);

        // then
        AssertView(document);
        AssertErrors(document, true);
    }

    private static void AssertView(IHtmlDocument document)
    {
        document
            .HasPageHeader(Model.ApplicationName, "Sales risk of Shared Ownership")
            .HasTextAreaInput("SalesRisk", "Tell us your assessment of the sales risk and how you will mitigate this", Model.SalesRisk)
            .HasGdsSaveAndContinueButton();
    }

    private void AssertErrors(IHtmlDocument document, bool exist)
    {
        document.HasSummaryErrorMessage(nameof(SchemeViewModel.SalesRisk), SalesRiskError, exist);
    }
}
