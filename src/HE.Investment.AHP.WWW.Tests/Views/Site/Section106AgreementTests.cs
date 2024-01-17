using System.Diagnostics.CodeAnalysis;
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
    [SuppressMessage("Blocker Code Smell", "S2699:Tests should include assertions", Justification = "Assertion present")]
    public async Task ShouldDisplayView()
    {
        // given & when
        var siteName = "Test Site 33";
        var site = new SiteModel() { Name = siteName };
        var document = await Render(_viewPath, site);

        // then
        document
            .HasTitle(SitePageTitles.SiteSection106Agreement)
            .HasPageHeader(siteName, @SitePageTitles.SiteSection106Agreement)
            .HasGdsRadioInputWithValues(nameof(SiteModel.Section106GeneralAgreement), "True", "False")
            .HasGdsSaveAndContinueButton()
            .HasGdsBackButton(false);
    }

    [Fact]
    [SuppressMessage("Blocker Code Smell", "S2699:Tests should include assertions", Justification = "Assertion present")]
    public async Task ShouldErrorSummary_ForInvalidName()
    {
        // given
        var errorMessage = "some test error";
        var modelState = new ModelStateDictionary();
        var siteName = "Test Site 33";
        var site = new SiteModel() { Name = siteName };
        modelState.AddModelError(nameof(SiteModel.Section106GeneralAgreement), errorMessage);

        // when
        var document = await Render(_viewPath, site, modelStateDictionary: modelState);

        // then
        document
            .HasTitle(SitePageTitles.SiteSection106Agreement)
            .HasPageHeader(siteName, @SitePageTitles.SiteSection106Agreement)
            .HasGdsRadioInputWithValues(nameof(SiteModel.Section106GeneralAgreement), "True", "False")
            .HasGdsSaveAndContinueButton()
            .HasGdsBackButton(false)
            .HasOneValidationMessages(errorMessage);
    }
}
