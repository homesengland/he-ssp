using System.Diagnostics.CodeAnalysis;
using HE.Investment.AHP.WWW.Models.HomeTypes;
using HE.Investments.Common.WWWTestsFramework.Helpers;

namespace HE.Investment.AHP.WWW.Tests.Views.HomeTypes;

public class FloorAreaStandardsTests : HomeTypesTestBase
{
    private const string ViewPath = "/Views/HomeTypes/FloorAreaStandards.cshtml";

    private static readonly FloorAreaModel Model = new("My application", "My homes");

    [Fact]
    [SuppressMessage("Blocker Code Smell", "S2699:Tests should include assertions", Justification = "Error in the Sonarlint library when using AngleSharp when using AngleSharp")]
    public async Task ShouldRenderViewWithCheckboxes()
    {
        // given & when
        var document = await RenderHomeTypePage(ViewPath, Model);

        // then
        document
            .HasElementWithText("span", "My application - My homes")
            .HasElementWithText("h1", "Floor area")
            .HasElementWithText("h2", "Which of these Nationally Described Space Standards do the homes meet?")
            .HasElementWithText("span", "What are the Nationally Described Space Standards?")
            .HasElementWithText("a", "Read more about the Nationally Described Space Standards (opens in a new tab).")
            .HasCheckboxes(
                "NationallyDescribedSpaceStandards",
                new[]
                {
                    "BuiltInStorageSpaceSize",
                    "BedroomAreas",
                    "BedroomWidth",
                })
            .HasCheckboxes("OtherNationallyDescribedSpaceStandards", new[] { "NoneOfThese", })
            .HasElementWithText("button", "Save and continue");
    }
}
