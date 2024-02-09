using AngleSharp.Html.Dom;
using HE.Investment.AHP.WWW.Models.Scheme;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Routing;

namespace HE.Investment.AHP.WWW.Tests.Views.Scheme;

public class HousingNeedsTests : ViewTestBase
{
    private const string ViewPath = "/Views/Scheme/HousingNeeds.cshtml";
    private const string MeetingLocalPrioritiesError = "Test error";
    private const string MeetingLocalHousingNeedError = "Second error";
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
        modelState.AddModelError(nameof(SchemeViewModel.MeetingLocalPriorities), MeetingLocalPrioritiesError);
        modelState.AddModelError(nameof(SchemeViewModel.MeetingLocalHousingNeed), MeetingLocalHousingNeedError);

        // when
        var document = await Render(ViewPath, Model, modelStateDictionary: modelState, routeData: _routeData);

        // then
        AssertView(document);
        AssertErrors(document, true);
    }

    private static void AssertView(IHtmlDocument document)
    {
        document
            .HasPageHeader(Model.ApplicationName, "Local housing needs")
            .HasTextAreaInput("MeetingLocalPriorities", "Tell us how this type and tenure of home meets the identified priorities for the local housing market", Model.MeetingLocalPriorities)
            .HasTextAreaInput("MeetingLocalHousingNeed", "Tell us how this scheme and proposal contributes to a locally identified housing need", Model.MeetingLocalHousingNeed)
            .HasSaveAndContinueButton();
    }

    private void AssertErrors(IHtmlDocument document, bool exist)
    {
        document.HasSummaryErrorMessage(nameof(SchemeViewModel.MeetingLocalPriorities), MeetingLocalPrioritiesError, exist);
        document.HasSummaryErrorMessage(nameof(SchemeViewModel.MeetingLocalHousingNeed), MeetingLocalHousingNeedError, exist);
    }
}
