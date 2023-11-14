using AngleSharp.Html.Dom;
using HE.InvestmentLoans.Contract.Projects.ValueObjects;
using HE.InvestmentLoans.Contract.Projects.ViewModels;
using HE.InvestmentLoans.WWW.Tests.Helpers;
using HE.InvestmentLoans.WWW.Views.Project.Consts;
using HE.Investments.Common.WWW.Models;
using Xunit;

namespace HE.InvestmentLoans.WWW.Tests.Views.Project;

public class LocalAuthorityConfirmTests : ViewTestBase
{
    private readonly string _viewPath = "/Views/Project/LocalAuthorityConfirm.cshtml";

    [Fact]
    public async Task ShouldDisplayView()
    {
        // given
        var localAuthoritiesViewModel = new LocalAuthoritiesViewModel { Items = new List<LocalAuthorityViewModel>() };
        var localAuthority = LocalAuthority.New(LocalAuthorityId.From("10"), "Liverpool");
        var model = new ConfirmModel<LocalAuthoritiesViewModel> { ViewModel = localAuthoritiesViewModel };
        model.ViewModel.LocalAuthorityName = localAuthority.Name;

        // when
        var document = await Render(_viewPath, model);

        // then
        AssertView(document, localAuthority.Name);
    }

    private static void AssertView(IHtmlDocument document, string localAuthorityName)
    {
        document
            .HasElementWithText("h1", ProjectPageTitles.LocalAuthorityConfirm)
            .HasElementWithText("legend", "Is this the correct local authority?")
            .HasElementWithText("b", localAuthorityName)
            .HasElementWithText("button", "Continue");
    }
}
