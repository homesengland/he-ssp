using HE.Investment.AHP.Contract.Site;
using HE.Investment.AHP.Contract.Site.Enums;
using HE.Investment.AHP.WWW.Views.Site.Const;

namespace HE.Investment.AHP.WWW.Tests.Views.Site;

public class SiteUseTests : ViewTestBase
{
    private readonly string _viewPath = "/Views/Site/SiteUse.cshtml";

    [Fact]
    public async Task ShouldDisplayView()
    {
        // given
        const string siteName = "Test Site";
        var site = new SiteUseDetails(null, null, TravellerPitchSiteType.Undefined);

        // when
        var document = await Render(_viewPath, site, viewBagOrViewData: new Dictionary<string, object> { { "SiteName", siteName } });

        // then
        document
            .HasTitle(SitePageTitles.SiteUse)
            .HasPageHeader(siteName)
            .HasHeader2("Are the homes part of street front infill?")
            .HasHint("Street front infill sites are commonly known as the development of gaps between existing developments or the replacement of existing properties. This can include commercial and other non-residential.")
            .HasRadio(nameof(SiteUseDetails.IsPartOfStreetFrontInfill), new[] { "True", "False" })
            .HasHeader2("Is this application for a traveller pitch site?")
            .HasRadio(nameof(SiteUseDetails.IsForTravellerPitchSite), new[] { "True", "False" })
            .HasSaveAndContinueButton()
            .HasBackLink(false);
    }
}
