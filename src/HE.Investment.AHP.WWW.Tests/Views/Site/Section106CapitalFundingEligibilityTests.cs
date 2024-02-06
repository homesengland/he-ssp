using System.Diagnostics.CodeAnalysis;
using HE.Investment.AHP.Contract.Site;
using HE.Investment.AHP.WWW.Views.Site;
using HE.Investments.Common.WWWTestsFramework;
using HE.Investments.TestsUtils.Extensions;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace HE.Investment.AHP.WWW.Tests.Views.Site;

public class Section106CapitalFundingEligibilityTests : ViewTestBase
{
    private readonly string _viewPath = "/Views/Site/Section106CapitalFundingEligibility.cshtml";
    private readonly string _siteId = Guid.NewGuid().ToString();
    private readonly string _siteName = "Test Site 33";

    [Fact]
    public async Task ShouldDisplayView()
    {
        // given & when
        var section106 = new Section106Dto(_siteId, _siteName, null);
        var document = await Render(_viewPath, section106);

        // then
        document
            .HasTitle(SitePageTitles.SiteSection106CapitalFundingEligibility)
            .HasPageHeader(_siteName, @SitePageTitles.SiteSection106CapitalFundingEligibility)
            .HasGdsRadioInputWithValues(nameof(Section106Dto.CapitalFundingEligibility), "True", "False")
            .HasGdsSaveAndContinueButton()
            .HasGdsBackLink(false);
    }

    [Fact]
    public async Task ShouldErrorSummary_ForInvalidName()
    {
        // given
        var errorMessage = "some test error";
        var modelState = new ModelStateDictionary();
        var section106 = new Section106Dto(_siteId, _siteName, null);
        modelState.AddModelError(nameof(Section106Dto.CapitalFundingEligibility), errorMessage);

        // when
        var document = await Render(_viewPath, section106, modelStateDictionary: modelState);

        // then
        document
            .HasTitle(SitePageTitles.SiteSection106CapitalFundingEligibility)
            .HasPageHeader(_siteName, @SitePageTitles.SiteSection106CapitalFundingEligibility)
            .HasGdsRadioInputWithValues(nameof(Section106Dto.CapitalFundingEligibility), "True", "False")
            .HasGdsSaveAndContinueButton()
            .HasGdsBackLink(false)
            .HasOneValidationMessages(errorMessage);
    }
}
