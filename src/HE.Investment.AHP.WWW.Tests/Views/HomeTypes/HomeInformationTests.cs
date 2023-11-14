using AngleSharp.Html.Dom;
using HE.Investment.AHP.WWW.Models.HomeTypes;
using HE.Investments.Common.Tests.WWW.Helpers;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace HE.Investment.AHP.WWW.Tests.Views.HomeTypes;

public class HomeInformationTests : HomeTypesTestBase
{
    private const string ViewPath = "/Views/HomeTypes/HomeInformation.cshtml";

    private static readonly HomeInformationModel Model = new("My application", "My homes");

    [Fact]
    public async Task ShouldDisplayView_WhenThereAreNoErrors()
    {
        // given & when
        var document = await Render(ViewPath, Model);

        // then
        AssertView(document);
        AssertErrors(document, nameof(HomeInformationModel.NumberOfHomes), false);
        AssertErrors(document, nameof(HomeInformationModel.NumberOfBedrooms), false);
        AssertErrors(document, nameof(HomeInformationModel.MaximumOccupancy), false);
        AssertErrors(document, nameof(HomeInformationModel.NumberOfStoreys), false);
    }

    [Fact]
    public async Task ShouldDisplayView_WhenAllFieldsHaveErrors()
    {
        // given
        var modelState = new ModelStateDictionary();
        modelState.AddModelError(nameof(HomeInformationModel.NumberOfHomes), ErrorMessage);
        modelState.AddModelError(nameof(HomeInformationModel.NumberOfBedrooms), ErrorMessage);
        modelState.AddModelError(nameof(HomeInformationModel.MaximumOccupancy), ErrorMessage);
        modelState.AddModelError(nameof(HomeInformationModel.NumberOfStoreys), ErrorMessage);

        // when
        var document = await Render(ViewPath, Model, modelStateDictionary: modelState);

        // then
        AssertView(document);
        AssertErrors(document, nameof(HomeInformationModel.NumberOfHomes), true);
        AssertErrors(document, nameof(HomeInformationModel.NumberOfBedrooms), true);
        AssertErrors(document, nameof(HomeInformationModel.MaximumOccupancy), true);
        AssertErrors(document, nameof(HomeInformationModel.NumberOfStoreys), true);
    }

    private static void AssertView(IHtmlDocument document)
    {
        document
            .HasElementWithText("div", "My application - My homes")
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
