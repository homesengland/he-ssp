using AngleSharp.Html.Dom;
using HE.InvestmentLoans.Contract.Projects.ViewModels;
using HE.InvestmentLoans.WWW.Views.Project.Consts;
using HE.Investments.WWW.Tests;
using HE.Investments.WWW.Tests.Helpers;
using Xunit;

namespace HE.InvestmentLoans.WWW.Tests.Views.Project;

public class LocalAuthoritySearchTests : ViewTestBase
{
    private readonly string _viewPath = "/Views/Project/LocalAuthoritySearch.cshtml";

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
            .HasElementWithText("h1", ProjectPageTitles.LocalAuthority)
            .HasElementWithText("p", "Search for your local authority. If your site is located in more than one local authority, search for the local authority where you have planning permission.")
            .HasInput("Phrase")
            .HasElementWithText("h1", "What is your local authority?")
            .HasElementWithText(
                "div",
                "Enter all or part of your local authority name.")
            .HasElementWithText("button", "Search");
    }
}
