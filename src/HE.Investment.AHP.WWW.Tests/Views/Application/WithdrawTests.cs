using AngleSharp.Html.Dom;
using HE.Investment.AHP.WWW.Models.Application;
using HE.Investment.AHP.WWW.Views.Application;
using HE.Investments.Common.WWWTestsFramework;
using HE.Investments.Common.WWWTestsFramework.Helpers;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace HE.Investment.AHP.WWW.Tests.Views.Application;

public class WithdrawTests : ViewTestBase
{
    private readonly string _viewPath = "/Views/Application/Withdraw.cshtml";
    private readonly ChangeApplicationStatusModel _model = new(Guid.NewGuid(), "my application");

    [Fact]
    public async Task ShouldDisplayView()
    {
        // given & when
        var document = await Render(_viewPath, _model);

        // then
        AssertView(document);
    }

    [Fact]
    public async Task ShouldDisplayView_ForInvalidChangeStatusReason()
    {
        // given
        var errorMessage = "some test error";
        var modelState = new ModelStateDictionary();
        modelState.AddModelError(nameof(ChangeApplicationStatusModel.ChangeStatusReason), errorMessage);

        // when
        var document = await Render(_viewPath, _model, modelStateDictionary: modelState);

        // then
        AssertView(document, errorMessage);
    }

    private static void AssertView(IHtmlDocument document, string? errorMessage = null)
    {
        document
            .HasElementWithText("h1", ApplicationPageTitles.Withdraw)
            .HasElementWithText("div", "You can enter up to 1500 characters")
            .HasElementWithText("button", "Withdraw application")
            .HasSummaryErrorMessage(nameof(ChangeApplicationStatusModel.ChangeStatusReason), errorMessage, !string.IsNullOrEmpty(errorMessage))
            .HasErrorMessage(nameof(ChangeApplicationStatusModel.ChangeStatusReason), errorMessage, !string.IsNullOrEmpty(errorMessage));
    }
}
