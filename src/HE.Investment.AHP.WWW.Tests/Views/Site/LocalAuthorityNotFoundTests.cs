using HE.Investment.AHP.Contract.Site;
using HE.Investment.AHP.WWW.Views.Site;

namespace HE.Investment.AHP.WWW.Tests.Views.Site;

public class LocalAuthorityNotFoundTests : ViewTestBase
{
    private readonly string _viewPath = "/Views/Site/LocalAuthorityNotFound.cshtml";

    [Fact]
    public async Task ShouldDisplayView()
    {
        // given & when
        var localAuthorities = new LocalAuthorities();
        var document = await Render(_viewPath, localAuthorities);

        // then
        document
            .HasPageHeader(header: SitePageTitles.LocalAuthorityNoMatch)
            .HasParagraph("We could not find the details you entered in our records.")
            .HasElementWithText("a", "try again using a different name.")
            .HasElementWithText("a", "Growth Manager can add this for you later.")
            .HasGdsBackButton(false);
    }
}
