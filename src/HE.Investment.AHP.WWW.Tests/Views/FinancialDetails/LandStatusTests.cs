using AngleSharp.Html.Dom;
using HE.Investment.AHP.Contract.FinancialDetails.Constants;
using HE.Investment.AHP.WWW.Models.FinancialDetails;

using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace HE.Investment.AHP.WWW.Tests.Views.FinancialDetails;

public class LandStatusTests : AhpViewTestBase
{
    private readonly string _viewPath = "/Views/FinancialDetails/LandStatus.cshtml";

    [Fact]
    public async Task ShouldDisplayView()
    {
        var model = new FinancialDetailsLandStatusModel(Guid.NewGuid().ToString(), "TestApp", string.Empty, true);

        // given & when
        var document = await Render(_viewPath, model);

        // then
        AssertView(document);
    }

    [Fact]
    public async Task ShouldDisplayView_ForInvalid()
    {
        // given
        var model = new FinancialDetailsLandStatusModel(Guid.NewGuid().ToString(), "TestApp", "56000", true);
        var errorMessage = "some test error";
        var modelState = new ModelStateDictionary();
        modelState.AddModelError(nameof(FinancialDetailsLandStatusModel.PurchasePrice), errorMessage);

        // when
        var document = await Render(_viewPath, model, modelStateDictionary: modelState);

        // then
        AssertView(document, errorMessage);
    }

    private static void AssertView(IHtmlDocument document, string? errorMessage = null)
    {
        document
            .HasElementWithText("h1", "Land status")
            .HasElementWithText("h2", "Enter the purchase price of the land")
            .HasElementWithText("div", "The purchase price must be backed by a valuation report from a qualified independent valuer, valid at the date of exchange of purchase contracts.")
            .HasSaveAndContinueButton()
            .HasSummaryErrorMessage(nameof(FinancialDetailsValidationFieldNames.PurchasePrice), errorMessage, !string.IsNullOrEmpty(errorMessage))
            .HasErrorMessage(nameof(FinancialDetailsValidationFieldNames.PurchasePrice), errorMessage, !string.IsNullOrEmpty(errorMessage));
    }
}
