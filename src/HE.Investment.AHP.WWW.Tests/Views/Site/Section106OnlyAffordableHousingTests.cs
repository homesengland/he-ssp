using HE.Investment.AHP.Contract.Site;
using HE.Investment.AHP.WWW.Views.Site.Const;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace HE.Investment.AHP.WWW.Tests.Views.Site;

public class Section106OnlyAffordableHousingTests : AhpViewTestBase
{
    private readonly string _viewPath = "/Views/Site/Section106OnlyAffordableHousing.cshtml";
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
            .HasTitle(SitePageTitles.SiteSection106OnlyAffordableHousing)
            .HasPageHeader(_siteName, @SitePageTitles.SiteSection106OnlyAffordableHousing)
            .HasGdsRadioInputWithValues(nameof(Section106Dto.OnlyAffordableHousing), "True", "False")
            .HasSaveAndContinueButton()
            .HasBackLink(out _, false);
    }

    [Fact]
    public async Task ShouldErrorSummary_ForInvalidName()
    {
        // given
        var errorMessage = "some test error";
        var modelState = new ModelStateDictionary();
        var section106 = new Section106Dto(_siteId, _siteName, null);
        modelState.AddModelError(nameof(Section106Dto.OnlyAffordableHousing), errorMessage);

        // when
        var document = await Render(_viewPath, section106, modelStateDictionary: modelState);

        // then
        document
            .HasTitle(SitePageTitles.SiteSection106OnlyAffordableHousing)
            .HasPageHeader(_siteName, @SitePageTitles.SiteSection106OnlyAffordableHousing)
            .HasGdsRadioInputWithValues(nameof(Section106Dto.OnlyAffordableHousing), "True", "False")
            .HasSaveAndContinueButton()
            .HasBackLink(out _, false)
            .HasOneValidationMessages(errorMessage);
    }
}
