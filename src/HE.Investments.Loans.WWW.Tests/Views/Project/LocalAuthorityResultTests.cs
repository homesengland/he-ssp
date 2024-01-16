using AngleSharp.Html.Dom;
using HE.Investments.Common.WWWTestsFramework;
using HE.Investments.Loans.Contract.Projects.ValueObjects;
using HE.Investments.Loans.Contract.Projects.ViewModels;
using HE.Investments.Loans.WWW.Views.Project.Consts;
using HE.Investments.TestsUtils.Extensions;
using Xunit;

namespace HE.Investments.Loans.WWW.Tests.Views.Project;

public class LocalAuthorityResultTests : ViewTestBase
{
    private readonly string _viewPath = "/Views/Project/LocalAuthorityResult.cshtml";

    [Fact]
    public async Task ShouldDisplayView()
    {
        // given
        var model = new LocalAuthoritiesViewModel { Items = new List<LocalAuthorityViewModel>() };
        var localAuthority = LocalAuthority.New(LocalAuthorityId.From("10"), "Liverpool");
        model.Items.Add(new LocalAuthorityViewModel() { Id = localAuthority.Id.ToString(), Name = localAuthority.Name });

        // when
        var document = await Render(_viewPath, model);

        // then
        AssertView(document, model.Items.First().Name);
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
