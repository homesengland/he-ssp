using AngleSharp.Html.Dom;
using HE.Investment.AHP.WWW.Models.Scheme;
using HE.Investments.Common.WWWTestsFramework;
using HE.Investments.Common.WWWTestsFramework.Helpers;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace HE.Investment.AHP.WWW.Tests.Views.Scheme;

public class StakeholderDiscussionsTests : ViewTestBase
{
    private const string ViewPath = "/Views/Scheme/StakeholderDiscussions.cshtml";
    private const string StakeholderDiscussionsReportError = "Test error";
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
        modelState.AddModelError(nameof(SchemeViewModel.StakeholderDiscussionsReport), StakeholderDiscussionsReportError);

        // when
        var document = await Render(ViewPath, Model, modelStateDictionary: modelState);

        // then
        AssertView(document);
        AssertErrors(document, true);
    }

    private static void AssertView(IHtmlDocument document)
    {
        document
            .HasPageHeader(Model.ApplicationName, "Local stakeholder discussions")
            .HasElementWithText("span", "Upload a file (PDF, DOCX, PNG)")
            .HasElementWithText("span", "Maximum file size 25 MB")
            .HasFormFieldLabel("Tell us about discussions you have had with local stakeholders", "h2")
            .HasInput("StakeholderDiscussionsReport", value: Model.StakeholderDiscussionsReport)
            .HasElementWithText("button", "Save and continue");
    }

    private void AssertErrors(IHtmlDocument document, bool exist)
    {
        document.HasSummaryErrorMessage(nameof(SchemeViewModel.StakeholderDiscussionsReport), StakeholderDiscussionsReportError, exist);
    }
}
