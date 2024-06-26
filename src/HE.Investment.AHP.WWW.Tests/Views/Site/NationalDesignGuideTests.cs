using HE.Investment.AHP.WWW.Models.Site;
using HE.Investment.AHP.WWW.Views.Site.Const;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace HE.Investment.AHP.WWW.Tests.Views.Site;

public class NationalDesignGuideTests : AhpViewTestBase
{
    private readonly string _viewPath = "/Views/Site/NationalDesignGuide.cshtml";

    [Fact]
    public async Task ShouldDisplayView()
    {
        // given & when
        var siteName = "Test Site 33";
        var site = new NationalDesignGuidePrioritiesModel()
        {
            SiteId = "8",
            SiteName = siteName,
            DesignPriorities = [],
        };
        var document = await Render(_viewPath, site);

        // then
        document
            .HasTitle(SitePageTitles.NationalDesignGuide)
            .HasPageHeader(siteName, @SitePageTitles.NationalDesignGuide)
            .HasSaveAndContinueButton()
            .HasBackLink(out _, false);
    }

    [Fact]
    public async Task ShouldErrorSummary_ForInvalidName()
    {
        // given
        var errorMessage = "some test error";
        var modelState = new ModelStateDictionary();
        var siteName = "Test Site 33";
        var site = new NationalDesignGuidePrioritiesModel()
        {
            SiteId = "8",
            SiteName = siteName,
            DesignPriorities = [],
        };
        modelState.AddModelError(nameof(NationalDesignGuidePrioritiesModel.DesignPriorities), errorMessage);

        // when
        var document = await Render(_viewPath, site, modelStateDictionary: modelState);

        // then
        document
            .HasTitle(SitePageTitles.NationalDesignGuide)
            .HasPageHeader(siteName, @SitePageTitles.NationalDesignGuide)
            .HasSaveAndContinueButton()
            .HasBackLink(out _, false)
            .HasOneValidationMessages(errorMessage);
    }
}
