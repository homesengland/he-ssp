using HE.Investment.AHP.WWW.Models.HomeTypes;
using HE.Investments.Common.WWWTestsFramework.Helpers;

namespace HE.Investment.AHP.WWW.Tests.Views.HomeTypes;

public class BuildingInformationIneligibleTests : HomeTypesTestBase
{
    private const string ViewPath = "/Views/HomeTypes/BuildingInformationIneligible.cshtml";

    private static readonly HomeTypeBasicModel Model = new("My application", "My homes");

    [Fact]
    public async Task ShouldRenderViewWithRadioButtons()
    {
        // given & when
        var document = await RenderHomeTypePage(ViewPath, Model);

        // then
        document
            .HasElementWithText("span", "My application - My homes")
            .HasElementWithText("h1", "Contact your Growth Manager")
            .HasElementWithText("a", "Go back and try again")
            .HasElementWithText("a", "Save and return to your account");
    }
}
