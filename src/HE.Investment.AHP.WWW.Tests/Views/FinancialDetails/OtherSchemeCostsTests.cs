using AngleSharp.Html.Dom;
using HE.Investment.AHP.Contract.FinancialDetails.Constants;
using HE.Investment.AHP.Domain.FinancialDetails.Constants;
using HE.Investment.AHP.WWW.Models.FinancialDetails;
using HE.Investments.Common.WWWTestsFramework;
using HE.Investments.Common.WWWTestsFramework.Helpers;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace HE.Investment.AHP.WWW.Tests.Views.FinancialDetails;

public class OtherSchemeCostsTests : ViewTestBase
{
    private readonly string _viewPath = "/Views/FinancialDetails/OtherApplicationCosts.cshtml";

    [Fact]
    public async Task ShouldDisplayView()
    {
        var model = new FinancialDetailsOtherApplicationCostsModel(Guid.NewGuid(), "TestApp", string.Empty, string.Empty);

        // given & when
        var document = await Render(_viewPath, model);

        // then
        AssertView(document);
    }

    [Fact]
    public async Task ShouldDisplayView_ForInvalid()
    {
        // given
        var model = new FinancialDetailsOtherApplicationCostsModel(Guid.NewGuid(), "TestApp", "56000", string.Empty);
        var errorMessage = "some test error";
        var modelState = new ModelStateDictionary();
        modelState.AddModelError(nameof(FinancialDetailsOtherApplicationCostsModel.ExpectedWorksCosts), errorMessage);

        // when
        var document = await Render(_viewPath, model, modelStateDictionary: modelState);

        // then
        AssertView(document, errorMessage);
    }

    private static void AssertView(IHtmlDocument document, string? errorMessage = null)
    {
        document
            .HasElementWithText("h1", "Other application costs")
            .HasElementWithText("h2", "Enter your expected works costs")
            .HasElementWithText("a", "Save and return to application")
            .HasElementWithText("button", "Save and continue")
            .HasSummaryErrorMessage(nameof(FinancialDetailsValidationFieldNames.ExpectedWorksCosts), errorMessage, !string.IsNullOrEmpty(errorMessage))
            .HasErrorMessage(nameof(FinancialDetailsValidationFieldNames.ExpectedWorksCosts), errorMessage, !string.IsNullOrEmpty(errorMessage));
    }
}
