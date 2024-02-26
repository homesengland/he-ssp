using HE.Investment.AHP.Contract.Site;
using HE.Investment.AHP.WWW.Models.Site;
using HE.Investment.AHP.WWW.Views.Site.Const;

namespace HE.Investment.AHP.WWW.Tests.Views.Site;

public class LandAcquisitionStatusTests : AhpViewTestBase
{
    private const string ViewPath = "/Views/Site/LandAcquisitionStatus.cshtml";

    [Fact]
    public async Task ShouldRenderViewWithRadioButtons()
    {
        // given & when
        var site = new SiteModel()
        {
            Id = "8",
            LandAcquisitionStatus = Contract.Site.Enums.SiteLandAcquisitionStatus.FullOwnership,
        };
        var document = await Render(ViewPath, site);

        // then
        document
            .HasPageHeader(null, SitePageTitles.LandAcquisitionStatus)
            .HasRadio(
                nameof(site.LandAcquisitionStatus),
                SiteFormOptions.LandAcquisitionStatuses.Select(x => x.Text).ToList())
            .HasSaveAndContinueButton();
    }
}
