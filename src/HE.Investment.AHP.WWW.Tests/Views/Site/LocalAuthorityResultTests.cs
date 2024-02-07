using HE.Investment.AHP.Contract.Site;
using HE.Investment.AHP.WWW.Views.Site.Const;
using HE.Investments.Common.Contract.Pagination;

namespace HE.Investment.AHP.WWW.Tests.Views.Site;

public class LocalAuthorityResultTests : ViewTestBase
{
    private readonly string _viewPath = "/Views/Site/LocalAuthorityResult.cshtml";

    [Fact]
    public async Task ShouldDisplayView()
    {
        // given & when
        var localAuthorities = new LocalAuthorities()
        {
            Page = new PaginationResult<LocalAuthority>(
                new List<LocalAuthority>() { new() { Id = "1", Name = "Liverpool" } },
                1,
                10,
                1),
        };
        var document = await Render(_viewPath, localAuthorities);

        // then
        document
            .HasTitle(SitePageTitles.LocalAuthorityResult)
            .HasPageHeader(header: SitePageTitles.LocalAuthorityResult)
            .HasSelectListItem("Liverpool", null)
            .HasGdsBackLink(false);
    }
}
