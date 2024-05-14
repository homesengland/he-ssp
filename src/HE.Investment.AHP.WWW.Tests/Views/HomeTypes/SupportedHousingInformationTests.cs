using HE.Investment.AHP.WWW.Models.HomeTypes;

namespace HE.Investment.AHP.WWW.Tests.Views.HomeTypes;

public class SupportedHousingInformationTests : HomeTypesTestBase
{
    private const string ViewPath = "/Views/HomeTypes/SupportedHousingInformation.cshtml";

    private static readonly SupportedHousingInformationModel Model = new("My application", "My homes");

    [Fact]
    public async Task ShouldRenderViewWithRadioButtons()
    {
        // given & when
        var document = await RenderHomeTypePage(ViewPath, Model);

        // then
        document
            .HasPageHeader("My application - My homes", "Supported housing information")
            .HasRadio(
                "LocalCommissioningBodiesConsulted",
                [
                    "Yes",
                    "No",
                ])
            .HasRadio(
                "ShortStayAccommodation",
                [
                    "Yes",
                    "No",
                ])
            .HasRadio(
                "RevenueFundingType",
                [
                    "Yes, revenue funding is needed and the source has been identified",
                    "Revenue funding is needed but the source has not been identified",
                    "No, revenue funding is not needed",
                ])
            .HasSaveAndContinueButton();
    }
}
