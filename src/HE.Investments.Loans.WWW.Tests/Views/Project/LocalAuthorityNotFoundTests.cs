using AngleSharp.Html.Dom;
using HE.Investments.Common.WWWTestsFramework;

using HE.Investments.Loans.Contract.Projects.ViewModels;
using HE.Investments.Loans.WWW.Views.Project.Consts;
using HE.Investments.TestsUtils.Extensions;
using Xunit;

namespace HE.Investments.Loans.WWW.Tests.Views.Project;

public class LocalAuthorityNotFoundTests : ViewTestBase
{
    private readonly string _viewPath = "/Views/Project/LocalAuthorityNotFound.cshtml";

    [Fact]
    public async Task ShouldDisplayView()
    {
        // given
        var model = new LocalAuthoritiesViewModel();

        // when
        var document = await Render(_viewPath, model);

        // then
        AssertView(document);
    }

    private static void AssertView(IHtmlDocument document)
    {
        document
            .HasElementWithText("h1", ProjectPageTitles.LocalAuthorityNoMatch)
            .HasElementWithText("p", "We could not find the details you entered in our records.")
            .HasElementWithText("a", "try again using a different name.")
            .HasElementWithText("a", "Transaction Manager can add this for you later.")
            .HasInput("ProjectId")
            .HasInput("ApplicationId");
    }
}
