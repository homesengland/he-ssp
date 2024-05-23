using AngleSharp.Html.Dom;
using HE.Investment.AHP.Contract.FinancialDetails.Constants;
using HE.Investment.AHP.Contract.Site.Enums;
using HE.Investment.AHP.WWW.Models.FinancialDetails;
using HE.Investments.Common.Extensions;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace HE.Investment.AHP.WWW.Tests.Views.FinancialDetails;

public class LandStatusTests : AhpViewTestBase
{
    private readonly string _viewPath = "/Views/FinancialDetails/LandStatus.cshtml";

    [Fact]
    public async Task ShouldDisplayView()
    {
        var model = new FinancialDetailsLandStatusModel(Guid.NewGuid().ToString(), "TestApp", string.Empty, SiteLandAcquisitionStatus.ConditionalAcquisition.GetDescription(), true);

        // given & when
        var document = await Render(_viewPath, model);

        // then
        AssertView(document, SiteLandAcquisitionStatus.ConditionalAcquisition.GetDescription());
    }

    [Fact]
    public async Task ShouldDisplayView_ForInvalid()
    {
        // given
        var model = new FinancialDetailsLandStatusModel(Guid.NewGuid().ToString(), "TestApp", "56000", SiteLandAcquisitionStatus.FullOwnership.GetDescription(), true);
        var errorMessage = "some test error";
        var modelState = new ModelStateDictionary();
        modelState.AddModelError(nameof(FinancialDetailsLandStatusModel.PurchasePrice), errorMessage);

        // when
        var document = await Render(_viewPath, model, modelStateDictionary: modelState);

        // then
        AssertView(document, SiteLandAcquisitionStatus.FullOwnership.GetDescription(), errorMessage);
    }

    private static void AssertView(IHtmlDocument document, string landAcquisitionStatus, string? errorMessage = null)
    {
        document
            .HasTitle("Land status")
            .HasElementWithText("strong", landAcquisitionStatus)
            .HasHint("The purchase price must be backed by a valuation report from a qualified independent valuer, valid at the date of exchange of purchase contracts.")
            .HasElementWithText("div", "If your application concerns existing properties rather than land-led development, this section is still relevant to you. Where we say 'site' or 'land', read 'properties'.")
            .HasSaveAndContinueButton()
            .HasSummaryErrorMessage(nameof(FinancialDetailsValidationFieldNames.PurchasePrice), errorMessage, !string.IsNullOrEmpty(errorMessage))
            .HasErrorMessage(nameof(FinancialDetailsValidationFieldNames.PurchasePrice), errorMessage, !string.IsNullOrEmpty(errorMessage));
    }
}
