using System.Diagnostics.CodeAnalysis;
using HE.Investment.AHP.Contract.Site;
using HE.Investment.AHP.WWW.Views.Site;
using HE.Investments.Common.WWWTestsFramework;
using HE.Investments.TestsUtils.Extensions;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace HE.Investment.AHP.WWW.Tests.Views.Site;

public class NameTests : ViewTestBase
{
    private readonly string _viewPath = "/Views/Site/Name.cshtml";

    [Fact]
    [SuppressMessage("Blocker Code Smell", "S2699:Tests should include assertions", Justification = "Error in the Sonarlint library when using AngleSharp when using AngleSharp")]
    public async Task ShouldDisplayView()
    {
        // given & when
        var document = await Render<SiteModel>(_viewPath);

        // then
        document
            .HasTitle(SitePageTitles.SiteName)
            .HasGdsInput(nameof(SiteModel.Name))
            .HasGdsSaveAndContinueButton()
            .HasGdsBackButton(false);
    }

    [Fact]
    [SuppressMessage("Blocker Code Smell", "S2699:Tests should include assertions", Justification = "Error in the Sonarlint library when using AngleSharp when using AngleSharp")]
    public async Task ShouldErrorSummary_ForInvalidName()
    {
        // given
        var errorMessage = "some test error";
        var modelState = new ModelStateDictionary();
        modelState.AddModelError(nameof(SiteModel.Name), errorMessage);

        // when
        var document = await Render<SiteModel>(_viewPath, modelStateDictionary: modelState);

        // then
        document
            .HasTitle(SitePageTitles.SiteName)
            .HasGdsInput(nameof(SiteModel.Name))
            .HasGdsSaveAndContinueButton()
            .HasGdsBackButton(false)
            .HasOneValidationMessages(errorMessage);
    }
}
