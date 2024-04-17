using HE.Investment.AHP.Contract.Site;
using HE.Investment.AHP.WWW.Views.Site.Const;
using HE.Investments.Common.Contract;
using HE.Investments.Common.Contract.Pagination;

namespace HE.Investment.AHP.WWW.Tests.Views.Site;

public class LocalAuthorityResultTests : AhpViewTestBase
{
    private readonly string _viewPath = "/Views/Site/LocalAuthorityResult.cshtml";

    [Fact]
    public async Task ShouldDisplayView()
    {
        // given & when
        var localAuthorities = new LocalAuthorities()
        {
            Page = new PaginationResult<LocalAuthority>(
                new List<LocalAuthority> { new("1", "Liverpool") },
                1,
                10,
                1),
        };
        var viewBag = new Dictionary<string, object> { { "SiteName", " some site name" } };
        var document = await Render(_viewPath, localAuthorities, viewBag);

        // then
        document
            .HasTitle(SitePageTitles.LocalAuthorityResult)
            .HasPageHeader(viewBag["SiteName"].ToString(), SitePageTitles.LocalAuthorityResult)
            .HasSelectListItem("Liverpool", null)
            .HasBackLink(out _, false);
    }
}
