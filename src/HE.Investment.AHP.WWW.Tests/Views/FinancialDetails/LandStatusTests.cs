using AngleSharp.Html.Dom;
using HE.Investment.AHP.Domain.FinancialDetails.Constants;
using HE.Investment.AHP.WWW.Models.Application;
using HE.Investment.AHP.WWW.Models.FinancialDetails;
using HE.Investments.Common.Tests.WWW;
using HE.Investments.Common.Tests.WWW.Helpers;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace HE.Investment.AHP.WWW.Tests.Views.FinancialDetails;

public class LandStatusTests : ViewTestBase
{
    private readonly string _viewPath = "/Views/FinancialDetails/LandStatus.cshtml";

    [Fact]
    public async Task ShouldDisplayView()
    {
        var model = new FinancialDetailsLandStatusModel(Guid.NewGuid(), "TestApp", string.Empty, true);

        // given & when
        var document = await Render(_viewPath, model);

        // then
        AssertView(document);
    }

    [Fact]
    public async Task ShouldDisplayView_ForInvalid()
    {
        // given
        var model = new FinancialDetailsLandStatusModel(Guid.NewGuid(), "TestApp", "56000", true);
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
            .HasElementWithText("button", "Save and continue")
            .HasSummaryErrorMessage(nameof(FinancialDetailsValidationFieldNames.PurchasePrice), errorMessage, !string.IsNullOrEmpty(errorMessage))
            .HasErrorMessage(nameof(FinancialDetailsValidationFieldNames.PurchasePrice), errorMessage, !string.IsNullOrEmpty(errorMessage));
    }
}
