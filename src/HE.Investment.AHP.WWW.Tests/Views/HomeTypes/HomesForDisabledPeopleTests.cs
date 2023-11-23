using HE.Investment.AHP.WWW.Models.HomeTypes;
using HE.Investments.Common.Tests.WWW.Helpers;

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
            .HasElementWithText("span", "My application - My homes")
            .HasElementWithText("h1", "What type of homes will you be providing for disabled and vulnerable people?")
            .HasRadio("HousingType", new[] { "DesignatedHomes", "DesignatedSupportedHomes", "PurposeDesignedHomes", "PurposeDesignedSupportedHomes", })
            .HasElementWithText("button", "Save and continue");
    }
}
