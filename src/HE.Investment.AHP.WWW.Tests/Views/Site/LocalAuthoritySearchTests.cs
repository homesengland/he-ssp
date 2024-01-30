using System.Diagnostics.CodeAnalysis;
using HE.Investment.AHP.Contract.Site;
using HE.Investment.AHP.WWW.Views.Site;
using HE.Investments.Common.WWWTestsFramework;
using HE.Investments.TestsUtils.Extensions;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace HE.Investment.AHP.WWW.Tests.Views.Site;

public class LocalAuthoritySearchTests : ViewTestBase
{
    private readonly string _viewPath = "/Views/Site/LocalAuthoritySearch.cshtml";

    [Fact]
    public async Task ShouldDisplayView()
    {
        // given & when
        var siteName = "Test Site 33";
        var site = new LocalAuthorities { SiteId = "1" };
        var document = await Render(_viewPath, site);

        // then
        document
            .HasTitle(SitePageTitles.LocalAuthoritySearch)
            .HasPageHeader(header: SitePageTitles.LocalAuthoritySearch)
            .HasParagraph(
                "Search for your local authority. If your site is located in more than one local authority, search for the local authority where you have planning permission.")
            .HasInput(nameof(LocalAuthorities.Phrase))
            .HasGdsBackButton(false);
    }

    [Fact]
    public async Task ShouldErrorSummary_ForInvalidName()
    {
        // given
        var errorMessage = "some test error";
        var modelState = new ModelStateDictionary();
        var siteName = "Test Site 33";
        var site = new LocalAuthorities() { SiteId = "1" };
        modelState.AddModelError(nameof(LocalAuthorities.Phrase), errorMessage);

        // when
        var document = await Render(_viewPath, site, modelStateDictionary: modelState);

        // then
        document
            .HasTitle(SitePageTitles.LocalAuthoritySearch)
            .HasPageHeader(header: SitePageTitles.LocalAuthoritySearch)
            .HasParagraph(
                "Search for your local authority. If your site is located in more than one local authority, search for the local authority where you have planning permission.")
            .HasInput(nameof(LocalAuthorities.Phrase))
            .HasGdsBackButton(false)
            .HasOneValidationMessages(errorMessage);
    }
}
