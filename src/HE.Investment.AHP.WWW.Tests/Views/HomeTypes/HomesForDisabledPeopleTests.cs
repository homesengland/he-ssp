using AngleSharp.Html.Dom;
using HE.Investment.AHP.WWW.Models.HomeTypes;
using HE.Investment.AHP.WWW.Tests.Helpers;

namespace HE.Investment.AHP.WWW.Tests.Views.HomeTypes;

public class HomesForDisabledPeopleTests : HomeTypesTestBase
{
    private const string ViewPath = "/Views/HomeTypes/HomesForDisabledPeople.cshtml";

    private static readonly HomesForDisabledPeopleModel Model = new("My application", "My homes");

    [Fact]
    public async Task ShouldDisplayView()
    {
        // given & when
        var document = await Render(ViewPath, Model);

        // then
        AssertView(document);
    }

    private static void AssertView(IHtmlDocument document)
    {
        document
            .HasElementWithText("div", "My application - My homes")
            .HasElementWithText("h1", "What type of homes will you be providing for disabled and vulnerable people?")
            .HasRadio("HousingType", new[] { "DesignatedHomes", "DesignatedSupportedHomes", "PurposeDesignedHomes", "PurposeDesignedSupportedHomes", })
            .HasElementWithText("button", "Save and continue");
    }
}
