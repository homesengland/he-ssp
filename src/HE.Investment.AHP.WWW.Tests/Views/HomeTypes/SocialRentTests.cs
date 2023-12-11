using AngleSharp.Html.Dom;
using HE.Investment.AHP.WWW.Models.HomeTypes;
using HE.Investments.Common.WWWTestsFramework.Helpers;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace HE.Investment.AHP.WWW.Tests.Views.HomeTypes;

public class SocialRentTests : HomeTypesTestBase
{
    private const string ViewPath = "/Views/HomeTypes/SocialRent.cshtml";

    private static readonly SocialRentModel Model = new("My application", "My homes");

    [Fact]
    public async Task ShouldDisplayView_WhenThereAreNoErrors()
    {
        // given & when
        var document = await RenderHomeTypePage(ViewPath, Model);

        // then
        AssertView(document);
        AssertErrors(document, nameof(SocialRentModel.MarketValue), false);
        AssertErrors(document, nameof(SocialRentModel.MarketRent), false);
    }

    [Fact]
    public async Task ShouldDisplayView_WhenAllFieldsHaveErrors()
    {
        // given
        var modelState = new ModelStateDictionary();
        modelState.AddModelError(nameof(SocialRentModel.MarketValue), ErrorMessage);
        modelState.AddModelError(nameof(SocialRentModel.MarketRent), ErrorMessage);

        // when
        var document = await RenderHomeTypePage(ViewPath, Model, modelStateDictionary: modelState);

        // then
        AssertView(document);
        AssertErrors(document, nameof(SocialRentModel.MarketValue), true);
        AssertErrors(document, nameof(SocialRentModel.MarketRent), true);
    }

    private static void AssertView(IHtmlDocument document)
    {
        document
            .HasElementWithText("span", "My application - My homes")
            .HasElementWithText("h1", "Social Rent details")
            .HasElementWithText("h2", "Enter the market value of each home")
            .HasElementWithText("span", "Enter the market value in pounds only.")
            .HasInput("MarketValue")
            .HasElementWithText("h2", "Enter the market rent per week")
            .HasElementWithText("span", "Enter the rent in pounds and pence. This is inclusive of all charges.")
            .HasInput("MarketRent")
            .HasElementWithText("button", "Save and continue");
    }
}
