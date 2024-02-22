using HE.Investment.AHP.Contract.Site.Enums;
using HE.Investment.AHP.WWW.Config;
using HE.Investment.AHP.WWW.Models.Site;
using HE.Investment.AHP.WWW.Views.Site.Const;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace HE.Investment.AHP.WWW.Tests.Views.Site;

public class NationalDesignGuideTests : ViewTestBase
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
            DesignPriorities = new List<NationalDesignGuidePriority>(),
        };
        var document = await Render(_viewPath, site, mockDependencies: services =>
        {
            services.AddTransient(_ => new Mock<IExternalLinks>().Object);
        });

        // then
        document
            .HasTitle(SitePageTitles.NationalDesignGuide)
            .HasPageHeader(siteName, @SitePageTitles.NationalDesignGuide)
            .HasSaveAndContinueButton()
            .HasBackLink(false);
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
            DesignPriorities = new List<NationalDesignGuidePriority>(),
        };
        modelState.AddModelError(nameof(NationalDesignGuidePrioritiesModel.DesignPriorities), errorMessage);

        // when
        var document = await Render(_viewPath, site, modelStateDictionary: modelState, mockDependencies: services =>
        {
            services.AddTransient(_ => new Mock<IExternalLinks>().Object);
        });

        // then
        document
            .HasTitle(SitePageTitles.NationalDesignGuide)
            .HasPageHeader(siteName, @SitePageTitles.NationalDesignGuide)
            .HasSaveAndContinueButton()
            .HasBackLink(false)
            .HasOneValidationMessages(errorMessage);
    }
}
