using HE.Investment.AHP.Contract.Site;
using HE.Investment.AHP.WWW.Views.Site.Const;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace HE.Investment.AHP.WWW.Tests.Views.Site;

public class Section106OnlyAffordableHousingTests : ViewTestBase
{
    private readonly string _viewPath = "/Views/Site/Section106OnlyAffordableHousing.cshtml";

    [Fact]
    public async Task ShouldDisplayView()
    {
        // given & when
        var siteName = "Test Site 33";
        var site = new SiteModel() { Name = siteName };
        var document = await Render(_viewPath, site);

        // then
        document
            .HasTitle(SitePageTitles.SiteSection106OnlyAffordableHousing)
            .HasPageHeader(siteName, @SitePageTitles.SiteSection106OnlyAffordableHousing)
            .HasGdsRadioInputWithValues(nameof(SiteModel.Section106OnlyAffordableHousing), "True", "False")
            .HasGdsSaveAndContinueButton()
            .HasGdsBackLink(false);
    }

    [Fact]
    public async Task ShouldErrorSummary_ForInvalidName()
    {
        // given
        var errorMessage = "some test error";
        var modelState = new ModelStateDictionary();
        var siteName = "Test Site 33";
        var site = new SiteModel() { Name = siteName };
        modelState.AddModelError(nameof(SiteModel.Section106OnlyAffordableHousing), errorMessage);

        // when
        var document = await Render(_viewPath, site, modelStateDictionary: modelState);

        // then
        document
            .HasTitle(SitePageTitles.SiteSection106OnlyAffordableHousing)
            .HasPageHeader(siteName, @SitePageTitles.SiteSection106OnlyAffordableHousing)
            .HasGdsRadioInputWithValues(nameof(SiteModel.Section106OnlyAffordableHousing), "True", "False")
            .HasGdsSaveAndContinueButton()
            .HasGdsBackLink(false)
            .HasOneValidationMessages(errorMessage);
    }
}
