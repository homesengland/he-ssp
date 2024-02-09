using System.Diagnostics.CodeAnalysis;
using HE.Investment.AHP.WWW.Models.HomeTypes;

namespace HE.Investment.AHP.WWW.Tests.Views.HomeTypes;

public class HomesForOlderPeopleTests : HomeTypesTestBase
{
    private const string ViewPath = "/Views/HomeTypes/HomesForOlderPeople.cshtml";

    private static readonly HomesForOlderPeopleModel Model = new("My application", "My homes");

    [Fact]
    public async Task ShouldRenderViewWithRadioButtons()
    {
        // given & when
        var document = await RenderHomeTypePage(ViewPath, Model);

        // then
        document
            .HasPageHeader("My application - My homes", "What type of homes will you be providing for older people?")
            .HasRadio(
                "HousingType",
                new[] { "DesignatedSupportedHomes", "HomesWithAccessToSupport", "HomesWithSomeSpecialDesignFeatures", "HomesWithAllSpecialDesignFeatures", })
            .HasSaveAndContinueButton();
    }
}
