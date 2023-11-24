using AngleSharp.Html.Dom;
using HE.InvestmentLoans.Contract.Projects.ViewModels;
using HE.InvestmentLoans.WWW.Views.Project.Consts;
using HE.Investments.Common.WWWTestsFramework;
using HE.Investments.Common.WWWTestsFramework.Helpers;
using Xunit;

namespace HE.InvestmentLoans.WWW.Tests.Views.Project;

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
