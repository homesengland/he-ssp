using AngleSharp.Html.Dom;
using HE.Investment.AHP.Domain.FinancialDetails.Constants;
using HE.Investment.AHP.WWW.Models.FinancialDetails;
using HE.Investments.Common.WWWTestsFramework;
using HE.Investments.Common.WWWTestsFramework.Helpers;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace HE.Investment.AHP.WWW.Tests.Views.FinancialDetails;

public class ContributionsTests : ViewTestBase
{
    private readonly string _viewPath = "/Views/FinancialDetails/Contributions.cshtml";

    [Fact]
    public async Task ShouldDisplayView()
    {
        var model = new FinancialDetailsContributionsModel(
            Guid.NewGuid(),
            "TestApp",
            string.Empty,
            string.Empty,
            string.Empty,
            string.Empty,
            string.Empty,
            string.Empty,
            string.Empty,
            string.Empty,
            true,
            true);

        // given & when
        var document = await Render(_viewPath, model);

        // then
        AssertView(document);
    }

    [Fact]
    public async Task ShouldDisplayView_ForInvalid()
    {
        // given
        var model = new FinancialDetailsContributionsModel(
            Guid.NewGuid(),
            "TestApp",
            string.Empty,
            string.Empty,
            string.Empty,
            string.Empty,
            string.Empty,
            string.Empty,
            string.Empty,
            string.Empty,
            true,
            true);
        var errorMessage = "some test error";
        var modelState = new ModelStateDictionary();
        modelState.AddModelError(FinancialDetailsValidationFieldNames.RentalIncomeBorrowing, errorMessage);

        // when
        var document = await Render(_viewPath, model, modelStateDictionary: modelState);

        // then
        AssertView(document, errorMessage);
    }

    private static void AssertView(IHtmlDocument document, string? errorMessage = null)
    {
        document
            .HasElementWithText("h1", "Your expected contributions to the scheme")
            .HasElementWithText("h2", "Enter how much you will contribute from borrowing against rental income for this scheme")
            .HasElementWithText("span", "Total expected contributions to the scheme")
            .HasElementWithText("button", "Save and continue")
            .HasSummaryErrorMessage(nameof(FinancialDetailsValidationFieldNames.RentalIncomeBorrowing), errorMessage, !string.IsNullOrEmpty(errorMessage))
            .HasErrorMessage(nameof(FinancialDetailsValidationFieldNames.RentalIncomeBorrowing), errorMessage, !string.IsNullOrEmpty(errorMessage));
    }
}
