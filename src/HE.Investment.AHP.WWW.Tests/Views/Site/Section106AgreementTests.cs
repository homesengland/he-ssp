using HE.Investment.AHP.Contract.Site;
using HE.Investment.AHP.WWW.Views.Site;
using HE.Investments.Common.WWWTestsFramework;
using HE.Investments.TestsUtils.Extensions;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace HE.Investment.AHP.WWW.Tests.Views.Site;

public class Section106AgreementTests : ViewTestBase
{
    private readonly string _viewPath = "/Views/Site/Section106Agreement.cshtml";

    [Fact]
    public async Task ShouldDisplayView()
    {
        // given & when
        var document = await Render<SiteModel>(_viewPath);

        // then
        document
            .HasTitle(SitePageTitles.SiteSection106Agreement)
            .HasGdsRadioInputWithValues(nameof(SiteModel.Section106GeneralAgreement), "True", "False")
            .HasGdsSaveAndContinueButton()
            .HasGdsBackButton();
    }

    [Fact]
    public async Task ShouldErrorSummary_ForInvalidName()
    {
        // given
        var errorMessage = "some test error";
        var modelState = new ModelStateDictionary();
        modelState.AddModelError(nameof(SiteModel.Section106GeneralAgreement), errorMessage);

        // when
        var document = await Render<SiteModel>(_viewPath, modelStateDictionary: modelState);

        // then
        document
            .HasTitle(SitePageTitles.SiteSection106Agreement)
            .HasGdsRadioInputWithValues(nameof(SiteModel.Section106GeneralAgreement), "True", "False")
            .HasGdsSaveAndContinueButton()
            .HasGdsBackButton()
            .HasOneValidationMessages(errorMessage);
    }
}
