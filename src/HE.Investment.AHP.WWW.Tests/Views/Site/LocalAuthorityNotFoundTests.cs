using HE.Investment.AHP.Contract.Site;
using HE.Investment.AHP.WWW.Views.Site.Const;

namespace HE.Investment.AHP.WWW.Tests.Views.Site;

public class LocalAuthorityNotFoundTests : ViewTestBase
{
    private readonly string _viewPath = "/Views/Site/LocalAuthorityNotFound.cshtml";

    [Fact]
    public async Task ShouldDisplayView()
    {
        // given & when
        var localAuthorities = new LocalAuthorities();
        var viewBag = new Dictionary<string, object> { { "SiteName", " some site name" } };
        var document = await Render(_viewPath, localAuthorities, viewBag);

        // then
        document
            .HasPageHeader(viewBag["SiteName"].ToString(), SitePageTitles.LocalAuthorityNoMatch)
            .HasParagraph("We could not find the details you entered in our records.")
            .HasLinkWithTestId("local-authority-search-link", out _)
            .HasLinkWithTestId("assign-empty-local-authority-link", out _)
            .HasBackLink(false);
    }
}
