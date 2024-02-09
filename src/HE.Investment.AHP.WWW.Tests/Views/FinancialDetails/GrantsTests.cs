using AngleSharp.Html.Dom;
using HE.Investment.AHP.Contract.FinancialDetails.Constants;
using HE.Investment.AHP.WWW.Models.FinancialDetails;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace HE.Investment.AHP.WWW.Tests.Views.FinancialDetails;

public class GrantsTests : ViewTestBase
{
    private readonly string _viewPath = "/Views/FinancialDetails/Grants.cshtml";

    [Fact]
    public async Task ShouldDisplayView()
    {
        var model = new FinancialDetailsGrantsModel(
            Guid.NewGuid(),
            "TestApp",
            string.Empty,
            string.Empty,
            string.Empty,
            string.Empty,
            string.Empty,
            string.Empty,
            string.Empty,
            "0");

        // given & when
        var document = await Render(_viewPath, model);

        // then
        AssertView(document);
    }

    [Fact]
    public async Task ShouldDisplayView_ForInvalid()
    {
        // given
        var model = new FinancialDetailsGrantsModel(
            Guid.NewGuid(),
            "TestApp",
            string.Empty,
            string.Empty,
            string.Empty,
            string.Empty,
            string.Empty,
            string.Empty,
            string.Empty,
            "0");
        var errorMessage = "some test error";
        var modelState = new ModelStateDictionary();
        modelState.AddModelError(FinancialDetailsValidationFieldNames.DhscExtraCareGrants, errorMessage);

        // when
        var document = await Render(_viewPath, model, modelStateDictionary: modelState);

        // then
        AssertView(document, errorMessage);
    }

    private static void AssertView(IHtmlDocument document, string? errorMessage = null)
    {
        document
            .HasElementWithText("h1", "Grants received from other public bodies")
            .HasElementWithText("h2", "Enter how much you have received from the county council")
            .HasElementWithText("span", "Total grants from other public bodies")
            .HasSaveAndContinueButton()
            .HasSummaryErrorMessage(FinancialDetailsValidationFieldNames.DhscExtraCareGrants, errorMessage, !string.IsNullOrEmpty(errorMessage))
            .HasErrorMessage(FinancialDetailsValidationFieldNames.DhscExtraCareGrants, errorMessage, !string.IsNullOrEmpty(errorMessage));
    }
}
