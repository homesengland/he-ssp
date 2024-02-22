using AngleSharp.Html.Dom;
using HE.Investment.AHP.WWW.Config;
using HE.Investment.AHP.WWW.Models.HomeTypes;

using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace HE.Investment.AHP.WWW.Tests.Views.HomeTypes;

public class AffordableRentTests : HomeTypesTestBase
{
    private const string ViewPath = "/Views/HomeTypes/AffordableRent.cshtml";

    private static readonly AffordableRentModel Model = new("My application", "My homes");

    [Fact]
    public async Task ShouldDisplayView_WhenThereAreNoErrors()
    {
        // given & when
        var document = await RenderHomeTypePage(ViewPath, Model);

        // then
        AssertView(document);
        AssertErrors(document, nameof(AffordableRentModel.MarketValue), false);
        AssertErrors(document, nameof(AffordableRentModel.MarketRent), false);
        AssertErrors(document, nameof(AffordableRentModel.ProspectiveRent), false);
        AssertErrors(document, nameof(AffordableRentModel.TargetRentExceedMarketRent), false);
    }

    [Fact]
    public async Task ShouldDisplayView_WhenAllFieldsHaveErrors()
    {
        // given
        var modelState = new ModelStateDictionary();
        modelState.AddModelError(nameof(AffordableRentModel.MarketValue), ErrorMessage);
        modelState.AddModelError(nameof(AffordableRentModel.MarketRent), ErrorMessage);
        modelState.AddModelError(nameof(AffordableRentModel.ProspectiveRent), ErrorMessage);
        modelState.AddModelError(nameof(AffordableRentModel.TargetRentExceedMarketRent), ErrorMessage);

        // when
        var document = await RenderHomeTypePage(ViewPath, Model, modelStateDictionary: modelState, mockDependencies: services =>
        {
            services.AddTransient(_ => new Mock<IExternalLinks>().Object);
        });

        // then
        AssertView(document);
        AssertErrors(document, nameof(AffordableRentModel.MarketValue), true);
        AssertErrors(document, nameof(AffordableRentModel.MarketRent), true);
        AssertErrors(document, nameof(AffordableRentModel.ProspectiveRent), true);
        AssertErrors(document, nameof(AffordableRentModel.TargetRentExceedMarketRent), true);
    }

    private static void AssertView(IHtmlDocument document)
    {
        document
            .HasPageHeader("My application - My homes", "Affordable Rent details")
            .HasElementWithText("h2", "Enter the market value of each home")
            .HasElementWithText("span", "Enter the market value in pounds only.")
            .HasInput("MarketValue")
            .HasElementWithText("h2", "Enter the market rent per week")
            .HasElementWithText("span", "Enter the market rent in pounds and pence.")
            .HasInput("MarketRent")
            .HasElementWithText("h2", "Enter the Affordable Rent per week")
            .HasElementWithText("span", "Enter the rent in pounds and pence. This is inclusive of all charges.")
            .HasInput("ProspectiveRent")
            .HasElementWithText("h2", "Affordable Rent as percentage of market rent")
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
            .HasSaveAndContinueButton()
            .HasElementWithText("button", "Calculate");
    }
}
