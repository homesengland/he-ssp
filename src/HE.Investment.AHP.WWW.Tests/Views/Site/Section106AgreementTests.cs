using HE.Investment.AHP.Contract.Site;
using HE.Investment.AHP.WWW.Views.Site.Const;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace HE.Investment.AHP.WWW.Tests.Views.Site;

public class Section106AgreementTests : ViewTestBase
{
    private readonly string _viewPath = "/Views/Site/Section106GeneralAgreement.cshtml";
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
            .HasTitle(SitePageTitles.SiteSection106Agreement)
            .HasPageHeader(_siteName, @SitePageTitles.SiteSection106Agreement)
            .HasGdsRadioInputWithValues(nameof(Section106Dto.GeneralAgreement), "True", "False")
            .HasSaveAndContinueButton()
            .HasBackLink(false);
    }

    [Fact]
    public async Task ShouldErrorSummary_ForInvalidName()
    {
        // given
        var errorMessage = "some test error";
        var modelState = new ModelStateDictionary();
        var section106 = new Section106Dto(_siteId, _siteName, null);
        modelState.AddModelError(nameof(Section106Dto.GeneralAgreement), errorMessage);

        // when
        var document = await Render(_viewPath, section106, modelStateDictionary: modelState);

        // then
        document
            .HasTitle(SitePageTitles.SiteSection106Agreement)
            .HasPageHeader(_siteName, @SitePageTitles.SiteSection106Agreement)
            .HasGdsRadioInputWithValues(nameof(Section106Dto.GeneralAgreement), "True", "False")
            .HasSaveAndContinueButton()
            .HasBackLink(false)
            .HasOneValidationMessages(errorMessage);
    }
}
