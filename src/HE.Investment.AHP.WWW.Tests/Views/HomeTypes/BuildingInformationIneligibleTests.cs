using System.Diagnostics.CodeAnalysis;
using HE.Investment.AHP.WWW.Models.HomeTypes;

namespace HE.Investment.AHP.WWW.Tests.Views.HomeTypes;

public class BuildingInformationIneligibleTests : HomeTypesTestBase
{
    private const string ViewPath = "/Views/HomeTypes/BuildingInformationIneligible.cshtml";

    private static readonly HomeTypeBasicModel Model = new("My application", "My homes");

    [Fact]
    [SuppressMessage("Blocker Code Smell", "S2699:Tests should include assertions", Justification = "Error in the Sonarlint library when using AngleSharp")]
    public async Task ShouldRenderViewWithRadioButtons()
    {
        // given & when
        var document = await RenderHomeTypePage(ViewPath, Model);

        // then
        document
            .HasPageHeader("My application - My homes", "Contact your Growth Manager")
            .HasElementWithText("a", "Go back and try again")
            .HasElementWithText("a", "Save and return to your account");
    }
}
