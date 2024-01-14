using System.Diagnostics.CodeAnalysis;
using HE.Investment.AHP.WWW.Models.HomeTypes;
using HE.Investments.Common.WWWTestsFramework.Helpers;

namespace HE.Investment.AHP.WWW.Tests.Views.HomeTypes;

public class SupportedHousingInformationTests : HomeTypesTestBase
{
    private const string ViewPath = "/Views/HomeTypes/SupportedHousingInformation.cshtml";

    private static readonly SupportedHousingInformationModel Model = new("My application", "My homes");

    [Fact]
    [SuppressMessage("Blocker Code Smell", "S2699:Tests should include assertions", Justification = "Error in the Sonarlint library when using AngleSharp when using AngleSharp")]
    public async Task ShouldRenderViewWithRadioButtons()
    {
        // given & when
        var document = await RenderHomeTypePage(ViewPath, Model);

        // then
        document
            .HasElementWithText("span", "My application - My homes")
            .HasElementWithText("h1", "Supported housing information")
            .HasRadio(
                "LocalCommissioningBodiesConsulted",
                new[]
                {
                    "Yes",
                    "No",
                })
            .HasRadio(
                "ShortStayAccommodation",
                new[]
                {
                    "Yes",
                    "No",
                })
            .HasRadio(
                "RevenueFundingType",
                new[]
                {
                    "Yes, revenue funding is needed and the source has been identified",
                    "Revenue funding is needed but the source has not been identified",
                    "No, revenue funding is not needed",
                })
            .HasElementWithText("button", "Save and continue");
    }
}
