using HE.Investment.AHP.WWW.Models.HomeTypes;
using HE.Investments.Common.WWWTestsFramework.Helpers;

namespace HE.Investment.AHP.WWW.Tests.Views.HomeTypes;

public class RentToBuyIneligibleTests : HomeTypesTestBase
{
    private const string ViewPath = "/Views/HomeTypes/RentToBuyIneligible.cshtml";

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
            .HasElementWithText("p", "The rent for Rent to Buy homes must not exceed 80% of market rent, unless the local target rent, including service charge, is higher.")
            .HasElementWithText("p", "Check the figures you have entered are correct. If the are, contact your Growth Manager to discuss this further.")
            .HasElementWithText("a", "Go back and try again")
            .HasElementWithText("a", "Save and return to your account");
    }
}
