using HE.Investment.AHP.WWW.Models.HomeTypes;

namespace HE.Investment.AHP.WWW.Tests.Views.HomeTypes;

public class HomesForDisabledPeopleTests : HomeTypesTestBase
{
    private const string ViewPath = "/Views/HomeTypes/HomesForDisabledPeople.cshtml";

    private static readonly HomesForDisabledPeopleModel Model = new("My application", "My homes");

    [Fact]
    public async Task ShouldRenderViewWithRadioButtons()
    {
        // given & when
        var document = await RenderHomeTypePage(ViewPath, Model);

        // then
        document
            .HasPageHeader("My application - My homes", "What type of homes will you be providing for disabled and vulnerable people?")
            .HasRadio("HousingType", new[] { "DesignatedHomes", "DesignatedSupportedHomes", "PurposeDesignedHomes", "PurposeDesignedSupportedHomes", })
            .HasGdsSaveAndContinueButton();
    }
}
