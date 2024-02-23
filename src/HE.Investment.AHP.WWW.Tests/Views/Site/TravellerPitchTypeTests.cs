using HE.Investment.AHP.Contract.Site;
using HE.Investment.AHP.Contract.Site.Enums;
using HE.Investment.AHP.WWW.Views.Site.Const;

namespace HE.Investment.AHP.WWW.Tests.Views.Site;

public class TravellerPitchTypeTests : AhpViewTestBase
{
    private readonly string _viewPath = "/Views/Site/TravellerPitchType.cshtml";

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
            .HasTitle(SitePageTitles.TravellerPitchType)
            .HasPageHeader(siteName)
            .HasRadio(nameof(SiteUseDetails.TravellerPitchSiteType), new[] { "Permanent", "Transit", "Temporary" })
            .HasHint("Sites that are intended for permanent use as a traveller pitch site and provide pitches for long-term use by residents.")
            .HasHint("Sites that are intended for the permanent provision of transit pitches, providing temporary accommodation for travellers for up to 3 months.")
            .HasHint("Sites that are only intended for temporary use as a traveller pitch site or which lack planning approval for permanent provision of traveller pitches.")
            .HasWarning("Homes England does not fund temporary sites for traveller pitches. Contact your Growth Manager to discuss your proposal before continuing your application.")
            .HasSaveAndContinueButton()
            .HasBackLink(false);
    }
}
