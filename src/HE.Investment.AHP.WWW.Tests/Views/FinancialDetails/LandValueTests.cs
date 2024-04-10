using AngleSharp.Html.Dom;
using HE.Investment.AHP.Contract.FinancialDetails.Constants;
using HE.Investment.AHP.WWW.Models.FinancialDetails;
using HE.Investments.Common.Contract.Enum;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace HE.Investment.AHP.WWW.Tests.Views.FinancialDetails;

public class LandValueTests : AhpViewTestBase
{
    private readonly string _viewPath = "/Views/FinancialDetails/LandValue.cshtml";

    [Fact]
    public async Task ShouldDisplayView()
    {
        var model = new FinancialDetailsLandValueModel(Guid.NewGuid(), "TestApp", string.Empty, null);

        // given & when
        var document = await Render(_viewPath, model);

        // then
        AssertView(document);
    }

    [Fact]
    public async Task ShouldDisplayView_ForInvalid()
    {
        // given
        var model = new FinancialDetailsLandValueModel(Guid.NewGuid(), "TestApp", "56000", null);
        var errorMessage = "some test error";
        var modelState = new ModelStateDictionary();
        modelState.AddModelError(nameof(FinancialDetailsLandValueModel.LandValue), errorMessage);

        // when
        var document = await Render(_viewPath, model, modelStateDictionary: modelState);

        // then
        AssertView(document, errorMessage);
    }

    private static void AssertView(IHtmlDocument document, string? errorMessage = null)
    {
        document
            .HasElementWithText("h1", "Land value")
            .HasElementWithText("h2", "Enter the current value of the land")
            .HasSaveAndContinueButton()
            .HasSummaryErrorMessage(nameof(FinancialDetailsValidationFieldNames.LandValue), errorMessage, !string.IsNullOrEmpty(errorMessage))
            .HasErrorMessage(nameof(FinancialDetailsValidationFieldNames.LandValue), errorMessage, !string.IsNullOrEmpty(errorMessage));
    }
}
