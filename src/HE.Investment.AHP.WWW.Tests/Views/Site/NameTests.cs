using HE.Investment.AHP.Contract.Site;
using HE.Investment.AHP.WWW.Views.Site.Const;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace HE.Investment.AHP.WWW.Tests.Views.Site;

public class NameTests : AhpViewTestBase
{
    private readonly string _viewPath = "/Views/Site/Name.cshtml";

    [Fact]
    public async Task ShouldDisplayView()
    {
        // given & when
        var document = await Render(_viewPath, new SiteModel());

        // then
        document
            .HasTitle(SitePageTitles.SiteName)
            .HasInput(nameof(SiteModel.Name))
            .HasSaveAndContinueButton()
            .HasBackLink(out _, false);
    }

    [Fact]
    public async Task ShouldErrorSummary_ForInvalidName()
    {
        // given
        var errorMessage = "some test error";
        var modelState = new ModelStateDictionary();
        modelState.AddModelError(nameof(SiteModel.Name), errorMessage);

        // when
        var document = await Render(_viewPath, new SiteModel(), modelStateDictionary: modelState);

        // then
        document
            .HasTitle(SitePageTitles.SiteName)
            .HasInput(nameof(SiteModel.Name))
            .HasSaveAndContinueButton()
            .HasBackLink(out _, false)
            .HasOneValidationMessages(errorMessage);
    }
}
