using AngleSharp.Html.Dom;
using HE.Investment.AHP.WWW.Models.HomeTypes;
using HE.Investments.Common.WWWTestsFramework.Helpers;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace HE.Investment.AHP.WWW.Tests.Views.HomeTypes;

public class SharedOwnershipTests : HomeTypesTestBase
{
    private const string ViewPath = "/Views/HomeTypes/SharedOwnership.cshtml";

    private static readonly SharedOwnershipModel Model = new("My application", "My homes");

    [Fact]
    public async Task ShouldDisplayView_WhenThereAreNoErrors()
    {
        // given & when
        var document = await RenderHomeTypePage(ViewPath, Model);

        // then
        AssertView(document);
        AssertErrors(document, nameof(SharedOwnershipModel.MarketValue), false);
        AssertErrors(document, nameof(SharedOwnershipModel.InitialSalePercentage), false);
        AssertErrors(document, nameof(SharedOwnershipModel.SharedOwnershipWeeklyRent), false);
    }

    [Fact]
    public async Task ShouldDisplayView_WhenAllFieldsHaveErrors()
    {
        // given
        var modelState = new ModelStateDictionary();
        modelState.AddModelError(nameof(SharedOwnershipModel.MarketValue), ErrorMessage);
        modelState.AddModelError(nameof(SharedOwnershipModel.InitialSalePercentage), ErrorMessage);
        modelState.AddModelError(nameof(SharedOwnershipModel.SharedOwnershipWeeklyRent), ErrorMessage);

        // when
        var document = await RenderHomeTypePage(ViewPath, Model, modelStateDictionary: modelState);

        // then
        AssertView(document);
        AssertErrors(document, nameof(SharedOwnershipModel.MarketValue), true);
        AssertErrors(document, nameof(SharedOwnershipModel.InitialSalePercentage), true);
        AssertErrors(document, nameof(SharedOwnershipModel.SharedOwnershipWeeklyRent), true);
    }

    private static void AssertView(IHtmlDocument document)
    {
        document
            .HasElementWithText("span", "My application - My homes")
            .HasElementWithText("h1", "Shared Ownership details")
            .HasElementWithText("h2", "Enter the market value of each home")
            .HasElementWithText("span", "Enter the market value in pounds only.")
            .HasInput("MarketValue")
            .HasElementWithText("h2", "Enter the average assumed first tranche sale percentage")
            .HasElementWithText("span", "This is the average percentage share that you are assuming your purchasers will buy in their initial purchase (must be between 10% nad 75%).")
            .HasInput("InitialSalePercentage")
            .HasElementWithText("h2", "Assumed first tranche sales receipt")
            .HasElementWithText("h2", "Enter the Shared Ownership rent per week")
            .HasElementWithText("span", "Enter the rent in pounds and pence. This is inclusive of all charges.")
            .HasInput("SharedOwnershipWeeklyRent")
            .HasElementWithText("h2", "Shared Ownership rent as percentage of the unsold share")
            .HasElementWithText("button", "Save and continue")
            .HasElementWithText("button", "Calculate");
    }
}
