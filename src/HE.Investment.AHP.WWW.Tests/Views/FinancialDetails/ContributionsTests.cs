using AngleSharp.Html.Dom;
using HE.Investment.AHP.Domain.FinancialDetails.Constants;
using HE.Investment.AHP.WWW.Models.Application;
using HE.Investment.AHP.WWW.Models.FinancialDetails;
using HE.Investments.Common.Tests.WWW;
using HE.Investments.Common.Tests.WWW.Helpers;
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
            .HasElementWithText("gds-fieldset-heading", "Your expected contributions to the scheme")
            .HasElementWithText("label", "Enter how much you will contribute from cross subsidy from the sale of open market homes on this scheme")
            .HasElementWithText("label", "Enter the transfer value of the homes on this scheme")
            .HasElementWithText("button", "Save and continue")
            .HasSummaryErrorMessage(nameof(FinancialDetailsValidationFieldNames.RentalIncomeBorrowing), errorMessage, !string.IsNullOrEmpty(errorMessage))
            .HasErrorMessage(nameof(FinancialDetailsValidationFieldNames.RentalIncomeBorrowing), errorMessage, !string.IsNullOrEmpty(errorMessage));
    }
}
