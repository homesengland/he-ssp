using HE.Investment.AHP.Contract.Site;
using HE.Investment.AHP.WWW.Views.Site.Const;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace HE.Investment.AHP.WWW.Tests.Views.Site;

public class NumberOfGreenLightsTests : ViewTestBase
{
    private readonly string _viewPath = "/Views/Site/NumberOfGreenLights.cshtml";

    [Fact]
    public async Task ShouldDisplayView()
    {
        // given & when
        var siteName = "Test Site 33";
        var site = new SiteModel() { Name = siteName };
        var document = await Render(_viewPath, site);

        // then
        document
            .HasTitle(SitePageTitles.NumberOfGreenLights)
            .HasPageHeader(siteName)
            .HasHint("The score is out of 12. You can also include the wider site in this score.")
            .HasInput(nameof(SiteModel.NumberOfGreenLights))
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
        modelState.AddModelError(nameof(SiteModel.NumberOfGreenLights), errorMessage);

        // when
        var document = await Render(_viewPath, site, modelStateDictionary: modelState);

        // then
        document
            .HasTitle(SitePageTitles.NumberOfGreenLights)
            .HasPageHeader(siteName)
            .HasHint("The score is out of 12. You can also include the wider site in this score.")
            .HasInput(nameof(SiteModel.NumberOfGreenLights))
            .HasGdsSaveAndContinueButton()
            .HasGdsBackLink(false)
            .HasOneValidationMessages(errorMessage);
    }
}
