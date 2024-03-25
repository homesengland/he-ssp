using HE.Investment.AHP.Contract.Site;
using HE.Investment.AHP.WWW.Views.Site.Const;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace HE.Investment.AHP.WWW.Tests.Views.Site;

public class EnvironmentalImpactTests : AhpViewTestBase
{
    private readonly string _viewPath = "/Views/Site/EnvironmentalImpact.cshtml";

    [Fact]
    public async Task ShouldDisplayView()
    {
        // given & when
        var viewBag = new Dictionary<string, object> { { "SiteName", " some site name" } };
        var document = await Render(_viewPath, new SiteModel(), viewBag);

        // then
        document
            .HasPageHeader(viewBag["SiteName"].ToString())
            .HasTitle(SitePageTitles.EnvironmentalImpact)
            .HasHint("For example, improving energy efficiency, reducing environmental impact and working towards net zero carbon.")
            .HasHint("You can enter up to 1500 characters")
            .HasTextAreaInput(nameof(SiteModel.EnvironmentalImpact))
            .HasSaveAndContinueButton()
            .HasBackLink(out _, false);
    }

    [Fact]
    public async Task ShouldErrorSummary_ForInvalidName()
    {
        // given
        var errorMessage = "some test error";
        var modelState = new ModelStateDictionary();
        var viewBag = new Dictionary<string, object> { { "SiteName", " some site name" } };
        modelState.AddModelError(nameof(SiteModel.EnvironmentalImpact), errorMessage);

        // when
        var document = await Render(_viewPath, new SiteModel(), viewBag, modelState);

        // then
        document
            .HasPageHeader(viewBag["SiteName"].ToString())
            .HasTitle(SitePageTitles.EnvironmentalImpact)
            .HasHint("For example, improving energy efficiency, reducing environmental impact and working towards net zero carbon.")
            .HasHint("You can enter up to 1500 characters")
            .HasTextAreaInput(nameof(SiteModel.EnvironmentalImpact))
            .HasSaveAndContinueButton()
            .HasBackLink(out _, false)
            .HasOneValidationMessages(errorMessage);
    }
}
