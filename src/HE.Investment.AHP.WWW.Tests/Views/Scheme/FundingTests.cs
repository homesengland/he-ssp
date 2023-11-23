using AngleSharp.Html.Dom;
using HE.Investment.AHP.WWW.Models.Scheme;
using HE.Investments.Common.Tests.WWW;
using HE.Investments.Common.Tests.WWW.Helpers;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Routing;

namespace HE.Investment.AHP.WWW.Tests.Views.Scheme;

public class FundingTests : ViewTestBase
{
    private const string ViewPath = "/Views/Scheme/Funding.cshtml";
    private const string FundingError = "Funding error";
    private const string HousesError = "Houses error";
    private static readonly SchemeViewModel Model = TestSchemeViewModel.Test;
    private readonly RouteData _routeData = new(new RouteValueDictionary { { "applicationId", "123" }, { "homeTypeId", "456" }, });

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
        modelState.AddModelError(nameof(SchemeViewModel.RequiredFunding), FundingError);
        modelState.AddModelError(nameof(SchemeViewModel.HousesToDeliver), HousesError);

        // when
        var document = await Render(ViewPath, Model, modelStateDictionary: modelState, routeData: _routeData);

        // then
        AssertView(document);
        AssertErrors(document, true);
    }

    private static void AssertView(IHtmlDocument document)
    {
        document
            .HasElementWithText("span", Model.ApplicationName)
            .HasElementWithText("h1", "Funding details")
            .HasElementWithText("h2", "Enter how much AHP CME 21-26 funding you require on this scheme")
            .HasInput("RequiredFunding")
            .HasElementWithText("h2", "Enter how many homes you intend to deliver")
            .HasInput("HousesToDeliver")
            .HasElementWithText("button", "Save and continue");
    }

    private void AssertErrors(IHtmlDocument document, bool exist)
    {
        AssertError(document, nameof(SchemeViewModel.RequiredFunding), FundingError, exist);
        AssertError(document, nameof(SchemeViewModel.HousesToDeliver), HousesError, exist);
    }
}
