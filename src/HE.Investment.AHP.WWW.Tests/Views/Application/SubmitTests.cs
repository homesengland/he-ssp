using AngleSharp.Html.Dom;
using HE.Investment.AHP.WWW.Models.Application;
using HE.Investment.AHP.WWW.Views.Application;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace HE.Investment.AHP.WWW.Tests.Views.Application;

public class SubmitTests : AhpViewTestBase
{
    private readonly string _viewPath = "/Views/Application/Submit.cshtml";

    private readonly ApplicationSubmitModel _model = new(
        "testId",
        "testName",
        "testNumber",
        "SiteName",
        "testTenure",
        "15",
        "250",
        "500000",
        "testWarranties");

    [Fact]
    public async Task ShouldDisplayView()
    {
        // given & when
        var document = await Render(_viewPath, _model);

        // then
        AssertView(document);
    }

    [Fact]
    public async Task ShouldDisplayView_ForInvalidChangeStatusReason()
    {
        // given
        var errorMessage = "some test error";
        var modelState = new ModelStateDictionary();
        modelState.AddModelError(nameof(ApplicationSubmitModel.RepresentationsAndWarranties), errorMessage);

        // when
        var document = await Render(_viewPath, _model, modelStateDictionary: modelState);

        // then
        AssertView(document, errorMessage);
    }

    private static void AssertView(IHtmlDocument document, string? errorMessage = null)
    {
        document
            .HasPageHeader("SiteName", ApplicationPageTitles.Submit)
            .HasCheckboxes(
                nameof(ApplicationSubmitModel.RepresentationsAndWarranties),
                new List<string> { "By submitting this application for funding, I confirm the statement above." })
            .HasOrderedList(new()
            {
                "This is full and final application and the organisation's board has approved, at least in principle, the schemes submitted and this will apply to all future schemes bid.",
                "That no scheme bid for under AHP 2021-26 will displace delivery under any other Homes England programmes (including strategic partnership pipeline delivery).",
                "All information, all confirmations and certifications in relation to the application are correct in all material respects (and if applicable consortium members have authorised the Lead Partner to make such confirmations and certifications).",
                "We consent that, if applicable, relevant financial information provided to the Regulator of Social Housing may be shared with Homes England for assessment purposes.",
                "We are aware that any subsequent award of grant funding will be subject to the terms of the programme, including that:",
            })
            .HasUnorderedList(new()
            {
                "grant recipient/s must enter a suitable form of grant agreement with Homes England",
                "grant recipient/s must become a Homes England Investment Partner (where not already) before submission of first claim",
                "the intended landlord of sub-market rental homes must be a Register Provider of Social Housing before first let",
            })
            .HasTableRowsHeaders(new()
            {
                "Site name",
                "Scheme name and tenure",
                "Number of homes",
                "Funding requested",
                "Scheme cost",
            })
            .HasSubmitButton(out _, "Accept and submit")
            .HasSummaryErrorMessage(nameof(ApplicationSubmitModel.RepresentationsAndWarranties), errorMessage, !string.IsNullOrEmpty(errorMessage))
            .HasErrorMessage(nameof(ApplicationSubmitModel.RepresentationsAndWarranties), errorMessage, !string.IsNullOrEmpty(errorMessage));
    }
}
