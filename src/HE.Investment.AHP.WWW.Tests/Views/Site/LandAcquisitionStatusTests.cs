using System.Diagnostics.CodeAnalysis;
using HE.Investment.AHP.Contract.Site;
using HE.Investment.AHP.Domain.Site.ValueObjects;
using HE.Investment.AHP.WWW.Models.Site;
using HE.Investment.AHP.WWW.Tests.Views.Site;
using HE.Investment.AHP.WWW.Views.Site.Const;

namespace HE.Investment.AHP.WWW.Tests.Views.Site;

public class LandAcquisitionStatusTests : ViewTestBase
{
    private const string ViewPath = "/Views/Site/LandAcquisitionStatus.cshtml";

    [Fact]
    public async Task ShouldRenderViewWithRadioButtons()
    {
        // given & when
        var site = new SiteModel()
        {
            Id = "8",
            Name = "Test Site 8",
            LandAcquisitionStatus = Contract.Site.Enums.SiteLandAcquisitionStatus.FullOwnership,
        };
        var document = await Render(ViewPath, site);

        // then
        document
            .HasPageHeader(site.Name, SitePageTitles.LandAcquisitionStatus)
            .HasRadio(
                nameof(site.LandAcquisitionStatus),
                SiteFormOptions.LandAcquisitionStatuses.Select(x => x.Text).ToList())
            .HasSaveAndContinueButton();
    }
}
