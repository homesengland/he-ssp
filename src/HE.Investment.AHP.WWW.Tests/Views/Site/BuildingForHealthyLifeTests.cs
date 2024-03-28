using HE.Investment.AHP.Contract.Site;
using HE.Investment.AHP.WWW.Views.Site.Const;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace HE.Investment.AHP.WWW.Tests.Views.Site;

public class BuildingForHealthyLifeTests : AhpViewTestBase
{
    private readonly string _viewPath = "/Views/Site/BuildingForHealthyLife.cshtml";

    [Fact]
    public async Task ShouldDisplayView()
    {
        // given & when
        var siteName = "Test Site 33";
        var site = new SiteModel() { Name = siteName };
        var document = await Render(_viewPath, site);

        // then
        document
            .HasTitle(SitePageTitles.BuildingForHealthyLife)
            .HasPageHeader(siteName, SitePageTitles.BuildingForHealthyLife)
            .HasSummaryDetails("Building for a Healthy Life is a design code to help people improve the design of new and growing neighbourhoods. You can read more in the")
            .HasRadio(
                "BuildingForHealthyLife",
                new[]
                {
                    "Yes",
                    "No",
                    "Not applicable, this site does not contain more than 10 homes",
                })
            .HasSaveAndContinueButton()
            .HasBackLink(out _, false);
    }

    [Fact]
    public async Task ShouldErrorSummary_ForInvalidName()
    {
        // given
        var errorMessage = "some test error";
        var modelState = new ModelStateDictionary();
        var siteName = "Test Site 33";
        var site = new SiteModel() { Name = siteName };
        modelState.AddModelError(nameof(SiteModel.BuildingForHealthyLife), errorMessage);

        // when
        var document = await Render(_viewPath, site, modelStateDictionary: modelState);

        // then
        document
            .HasTitle(SitePageTitles.BuildingForHealthyLife)
            .HasPageHeader(siteName, SitePageTitles.BuildingForHealthyLife)
            .HasSummaryDetails("Building for a Healthy Life is a design code to help people improve the design of new and growing neighbourhoods. You can read more in the")
            .HasRadio(
                "BuildingForHealthyLife",
                new[]
                {
                    "Yes",
                    "No",
                    "Not applicable, this site does not contain more than 10 homes",
                })
            .HasSaveAndContinueButton()
            .HasBackLink(out _, false)
            .HasOneValidationMessages(errorMessage);
    }
}
