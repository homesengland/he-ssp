using AngleSharp.Html.Dom;
using HE.Investments.Loans.Contract.Projects.ViewModels;
using HE.Investments.Loans.WWW.Views.Project.Consts;
using HE.Investments.Organisation.LocalAuthorities.ValueObjects;

namespace HE.Investments.Loans.WWW.Tests.Views.Project;

public class LocalAuthorityResultTests : ViewTestBase
{
    private readonly string _viewPath = "/Views/Project/LocalAuthorityResult.cshtml";

    [Fact]
    public async Task ShouldDisplayView()
    {
        // given
        var model = new LocalAuthoritiesViewModel { Items = [] };
        var localAuthority = LocalAuthority.New(LocalAuthorityCode.From("10"), "Liverpool");
        model.Items.Add(new LocalAuthorityViewModel() { Id = localAuthority.Code.ToString(), Name = localAuthority.Name });

        // when
        var document = await Render(_viewPath, model);

        // then
        AssertView(document, model.Items[0].Name);
    }

    private static void AssertView(IHtmlDocument document, string localAuthorityName)
    {
        document
            .HasElementWithText("h1", ProjectPageTitles.LocalAuthorityResult)
            .HasElementWithText("a", "Transaction Manager can add this for you later.")
            .HasElementWithText("b", localAuthorityName)
            .HasInput("ProjectId")
            .HasInput("ApplicationId");
    }
}
