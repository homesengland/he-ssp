using HE.Investment.AHP.Contract.Site;
using HE.Investment.AHP.WWW.Views.Site.Const;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace HE.Investment.AHP.WWW.Tests.Views.Site;

public class LocalAuthoritySearchTests : ViewTestBase
{
    private readonly string _viewPath = "/Views/Site/LocalAuthoritySearch.cshtml";

    [Fact]
    public async Task ShouldDisplayView()
    {
        // given & when
        var localAuthorities = new LocalAuthorities();
        var document = await Render(_viewPath, localAuthorities);

        // then
        document
            .HasTitle(SitePageTitles.LocalAuthoritySearch)
            .HasPageHeader(header: SitePageTitles.LocalAuthoritySearch)
            .HasParagraph(
                "Search for your local authority. If your site is located in more than one local authority, search for the local authority where you have planning permission.")
            .HasInput(nameof(LocalAuthorities.Phrase))
            .HasBackLink(false);
    }

    [Fact]
    public async Task ShouldErrorSummary_ForInvalidName()
    {
        // given
        var errorMessage = "some test error";
        var modelState = new ModelStateDictionary();
        var localAuthorities = new LocalAuthorities();
        modelState.AddModelError(nameof(LocalAuthorities.Phrase), errorMessage);

        // when
        var document = await Render(_viewPath, localAuthorities, modelStateDictionary: modelState);

        // then
        document
            .HasTitle(SitePageTitles.LocalAuthoritySearch)
            .HasPageHeader(header: SitePageTitles.LocalAuthoritySearch)
            .HasParagraph(
                "Search for your local authority. If your site is located in more than one local authority, search for the local authority where you have planning permission.")
            .HasInput(nameof(LocalAuthorities.Phrase))
            .HasBackLink(false)
            .HasOneValidationMessages(errorMessage);
    }
}
