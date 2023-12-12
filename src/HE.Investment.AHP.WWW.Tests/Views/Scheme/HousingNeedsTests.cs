using AngleSharp.Html.Dom;
using HE.Investment.AHP.WWW.Models.Scheme;
using HE.Investments.Common.WWWTestsFramework;
using HE.Investments.Common.WWWTestsFramework.Helpers;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace HE.Investment.AHP.WWW.Tests.Views.Scheme;

public class HousingNeedsTests : ViewTestBase
{
    private const string ViewPath = "/Views/Scheme/HousingNeeds.cshtml";
    private const string MeetingLocalPrioritiesError = "Test error";
    private const string MeetingLocalHousingNeedError = "Second error";
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
        modelState.AddModelError(nameof(SchemeViewModel.MeetingLocalPriorities), MeetingLocalPrioritiesError);
        modelState.AddModelError(nameof(SchemeViewModel.MeetingLocalHousingNeed), MeetingLocalHousingNeedError);

        // when
        var document = await Render(ViewPath, Model, modelStateDictionary: modelState);

        // then
        AssertView(document);
        AssertErrors(document, true);
    }

    private static void AssertView(IHtmlDocument document)
    {
        document
            .HasPageHeader(Model.ApplicationName, "Local housing needs")
            .HasFormFieldLabel("Tell us how this type and tenure of home meets the identified priorities for the local housing market", "h2")
            .HasInput("MeetingLocalPriorities", value: Model.MeetingLocalPriorities)
            .HasFormFieldLabel("Tell us how this scheme and proposal contributes to a locally identified housing need", "h2")
            .HasInput("MeetingLocalHousingNeed", value: Model.MeetingLocalHousingNeed)
            .HasElementWithText("button", "Save and continue");
    }

    private void AssertErrors(IHtmlDocument document, bool exist)
    {
        document.HasSummaryErrorMessage(nameof(SchemeViewModel.MeetingLocalPriorities), MeetingLocalPrioritiesError, exist);
        document.HasSummaryErrorMessage(nameof(SchemeViewModel.MeetingLocalHousingNeed), MeetingLocalHousingNeedError, exist);
    }
}
