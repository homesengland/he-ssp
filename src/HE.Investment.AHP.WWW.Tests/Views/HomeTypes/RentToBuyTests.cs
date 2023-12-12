using AngleSharp.Html.Dom;
using HE.Investment.AHP.WWW.Models.HomeTypes;
using HE.Investments.Common.WWWTestsFramework.Helpers;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace HE.Investment.AHP.WWW.Tests.Views.HomeTypes;

public class RentToBuyTests : HomeTypesTestBase
{
    private const string ViewPath = "/Views/HomeTypes/RentToBuy.cshtml";

    private static readonly RentToBuyModel Model = new("My application", "My homes");

    [Fact]
    public async Task ShouldDisplayView_WhenThereAreNoErrors()
    {
        // given & when
        var document = await RenderHomeTypePage(ViewPath, Model);

        // then
        AssertView(document);
        AssertErrors(document, nameof(RentToBuyModel.MarketValue), false);
        AssertErrors(document, nameof(RentToBuyModel.MarketRent), false);
        AssertErrors(document, nameof(RentToBuyModel.ProspectiveRent), false);
        AssertErrors(document, nameof(RentToBuyModel.TargetRentExceedMarketRent), false);
    }

    [Fact]
    public async Task ShouldDisplayView_WhenAllFieldsHaveErrors()
    {
        // given
        var modelState = new ModelStateDictionary();
        modelState.AddModelError(nameof(RentToBuyModel.MarketValue), ErrorMessage);
        modelState.AddModelError(nameof(RentToBuyModel.MarketRent), ErrorMessage);
        modelState.AddModelError(nameof(RentToBuyModel.ProspectiveRent), ErrorMessage);
        modelState.AddModelError(nameof(RentToBuyModel.TargetRentExceedMarketRent), ErrorMessage);

        // when
        var document = await RenderHomeTypePage(ViewPath, Model, modelStateDictionary: modelState);

        // then
        AssertView(document);
        AssertErrors(document, nameof(RentToBuyModel.MarketValue), true);
        AssertErrors(document, nameof(RentToBuyModel.MarketRent), true);
        AssertErrors(document, nameof(RentToBuyModel.ProspectiveRent), true);
        AssertErrors(document, nameof(RentToBuyModel.TargetRentExceedMarketRent), true);
    }

    private static void AssertView(IHtmlDocument document)
    {
        document
            .HasElementWithText("span", "My application - My homes")
            .HasElementWithText("h1", "Rent to Buy details")
            .HasElementWithText("h2", "Enter the market value of each home")
            .HasElementWithText("span", "Enter the market value in pounds only.")
            .HasInput("MarketValue")
            .HasElementWithText("h2", "Enter the market rent per week")
            .HasElementWithText("span", "Enter the market rent in pounds and pence.")
            .HasInput("MarketRent")
            .HasElementWithText("h2", "Enter the rent per week")
            .HasElementWithText("span", "Enter the rent in pounds and pence. This is inclusive of all charges.")
            .HasInput("ProspectiveRent")
            .HasElementWithText("h2", "Rent as percentage of market rent")
            .HasElementWithText("h2", "Would the target rent plus service charge for these homes exceed 80% of market rent?")
            .HasElementWithText("span", "Help with target rent")
            .HasElementWithText("a", "Find out more information on our Rent Policy Statement (opens in the new tab).")
            .HasRadio(
                "TargetRentExceedMarketRent",
                new[]
                {
                    "Yes",
                    "No",
                })
            .HasElementWithText("button", "Save and continue")
            .HasElementWithText("button", "Calculate");
    }
}
