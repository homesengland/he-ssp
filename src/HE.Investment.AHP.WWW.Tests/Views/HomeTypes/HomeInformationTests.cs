using AngleSharp.Html.Dom;
using HE.Investment.AHP.WWW.Models.HomeTypes;
using HE.Investments.Common.WWWTestsFramework.Helpers;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace HE.Investment.AHP.WWW.Tests.Views.HomeTypes;

public class HomeInformationTests : HomeTypesTestBase
{
    private const string ViewPath = "/Views/HomeTypes/HomeInformation.cshtml";

    private static readonly HomeInformationBasicModel BasicModel = new("My application", "My homes");

    [Fact]
    public async Task ShouldDisplayView_WhenThereAreNoErrors()
    {
        // given & when
        var document = await RenderHomeTypePage(ViewPath, BasicModel);

        // then
        AssertView(document);
        AssertErrors(document, nameof(HomeInformationBasicModel.NumberOfHomes), false);
        AssertErrors(document, nameof(HomeInformationBasicModel.NumberOfBedrooms), false);
        AssertErrors(document, nameof(HomeInformationBasicModel.MaximumOccupancy), false);
        AssertErrors(document, nameof(HomeInformationBasicModel.NumberOfStoreys), false);
    }

    [Fact]
    public async Task ShouldDisplayView_WhenAllFieldsHaveErrors()
    {
        // given
        var modelState = new ModelStateDictionary();
        modelState.AddModelError(nameof(HomeInformationBasicModel.NumberOfHomes), ErrorMessage);
        modelState.AddModelError(nameof(HomeInformationBasicModel.NumberOfBedrooms), ErrorMessage);
        modelState.AddModelError(nameof(HomeInformationBasicModel.MaximumOccupancy), ErrorMessage);
        modelState.AddModelError(nameof(HomeInformationBasicModel.NumberOfStoreys), ErrorMessage);

        // when
        var document = await RenderHomeTypePage(ViewPath, BasicModel, modelStateDictionary: modelState);

        // then
        AssertView(document);
        AssertErrors(document, nameof(HomeInformationBasicModel.NumberOfHomes), true);
        AssertErrors(document, nameof(HomeInformationBasicModel.NumberOfBedrooms), true);
        AssertErrors(document, nameof(HomeInformationBasicModel.MaximumOccupancy), true);
        AssertErrors(document, nameof(HomeInformationBasicModel.NumberOfStoreys), true);
    }

    private static void AssertView(IHtmlDocument document)
    {
        document
            .HasElementWithText("span", "My application - My homes")
            .HasElementWithText("h1", "Home information")
            .HasElementWithText("h2", "Enter the number of homes you are delivering")
            .HasInput("NumberOfHomes")
            .HasElementWithText("h2", "Enter the number of bedrooms in each home")
            .HasInput("NumberOfBedrooms")
            .HasElementWithText("h2", "Enter the maximum occupancy of each home")
            .HasElementWithText("div", "This is the maximum number of people who can live in each home.")
            .HasInput("MaximumOccupancy")
            .HasElementWithText("h2", "Enter how many storeys each home has")
            .HasElementWithText("div", "If the homes are in a multi storey development, enter how many storeys are in each individual home.")
            .HasInput("NumberOfStoreys")
            .HasElementWithText("button", "Save and continue");
    }
}
