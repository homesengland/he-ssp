using System.Diagnostics.CodeAnalysis;
using HE.Investment.AHP.WWW.Models.HomeTypes;
using HE.Investments.Common.WWWTestsFramework.Helpers;

namespace HE.Investment.AHP.WWW.Tests.Views.HomeTypes;

public class ExemptFromTheRightToSharedOwnershipTests : HomeTypesTestBase
{
    private const string ViewPath = "/Views/HomeTypes/ExemptFromTheRightToSharedOwnership.cshtml";

    private static readonly ExemptFromTheRightToSharedOwnershipModel Model = new("My application", "My homes");

    [Fact]
    [SuppressMessage("Blocker Code Smell", "S2699:Tests should include assertions", Justification = "Error in the Sonarlint library when using AngleSharp when using AngleSharp")]
    public async Task ShouldRenderViewWithRadioButtons()
    {
        // given & when
        var document = await RenderHomeTypePage(ViewPath, Model);

        // then
        document
            .HasElementWithText("span", "My application - My homes")
            .HasElementWithText("h1", "For any reason, are these homes considered exempt from the Right to Shared Ownership?")
            .HasElementWithText("span", "Which properties are in scope for the Right to Shared Ownership?")
            .HasElementWithText("a", "Read about the Right to Shared Ownership and the types of property that are in scope (opens in the new tab).")
            .HasRadio(
                "ExemptFromTheRightToSharedOwnership",
                new[]
                {
                    "Yes",
                    "No",
                })
            .HasElementWithText("button", "Save and continue");
    }
}
