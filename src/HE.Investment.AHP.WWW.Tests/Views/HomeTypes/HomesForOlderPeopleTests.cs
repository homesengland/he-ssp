using System.Diagnostics.CodeAnalysis;
using HE.Investment.AHP.WWW.Models.HomeTypes;
using HE.Investments.Common.WWWTestsFramework.Helpers;

namespace HE.Investment.AHP.WWW.Tests.Views.HomeTypes;

public class HomesForOlderPeopleTests : HomeTypesTestBase
{
    private const string ViewPath = "/Views/HomeTypes/HomesForOlderPeople.cshtml";

    private static readonly HomesForOlderPeopleModel Model = new("My application", "My homes");

    [Fact]
    [SuppressMessage("Blocker Code Smell", "S2699:Tests should include assertions", Justification = "Error in the Sonarlint library when using AngleSharp when using AngleSharp")]
    public async Task ShouldRenderViewWithRadioButtons()
    {
        // given & when
        var document = await RenderHomeTypePage(ViewPath, Model);

        // then
        document
            .HasElementWithText("span", "My application - My homes")
            .HasElementWithText("h1", "What type of homes will you be providing for older people?")
            .HasRadio(
                "HousingType",
                new[] { "DesignatedSupportedHomes", "HomesWithAccessToSupport", "HomesWithSomeSpecialDesignFeatures", "HomesWithAllSpecialDesignFeatures", })
            .HasElementWithText("button", "Save and continue");
    }
}
