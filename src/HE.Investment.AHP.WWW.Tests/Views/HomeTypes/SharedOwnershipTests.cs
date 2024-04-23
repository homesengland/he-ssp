using AngleSharp.Html.Dom;
using HE.Investment.AHP.WWW.Models.HomeTypes;

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
        AssertErrors(document, nameof(SharedOwnershipModel.InitialSale), false);
        AssertErrors(document, nameof(SharedOwnershipModel.RentPerWeek), false);
    }

    [Fact]
    public async Task ShouldDisplayView_WhenAllFieldsHaveErrors()
    {
        // given
        var modelState = new ModelStateDictionary();
        modelState.AddModelError(nameof(SharedOwnershipModel.MarketValue), ErrorMessage);
        modelState.AddModelError(nameof(SharedOwnershipModel.InitialSale), ErrorMessage);
        modelState.AddModelError(nameof(SharedOwnershipModel.RentPerWeek), ErrorMessage);

        // when
        var document = await RenderHomeTypePage(ViewPath, Model, modelStateDictionary: modelState);

        // then
        AssertView(document);
        AssertErrors(document, nameof(SharedOwnershipModel.MarketValue), true);
        AssertErrors(document, nameof(SharedOwnershipModel.InitialSale), true);
        AssertErrors(document, nameof(SharedOwnershipModel.RentPerWeek), true);
    }

    private static void AssertView(IHtmlDocument document)
    {
        document
            .HasPageHeader("My application - My homes", "Shared Ownership details")
            .HasElementWithText("h2", "Enter the market value of each home")
            .HasElementWithText("span", "Enter the market value in pounds only.")
            .HasInput("MarketValue")
            .HasElementWithText("h2", "Enter the assumed average first tranche sale percentage")
            .HasElementWithText("span", "This is the average percentage share that you are assuming your purchasers will buy in their initial purchase (must be between 10% and 75%).")
            .HasInput("InitialSale")
            .HasElementWithText("h2", "Assumed first tranche sales receipt")
            .HasElementWithText("h2", "Enter the Shared Ownership rent per week")
            .HasElementWithText("span", "Enter the rent in pounds and pence. This is inclusive of all charges.")
            .HasInput("RentPerWeek")
            .HasElementWithText("h2", "Shared Ownership rent as percentage of the unsold share")
            .HasSaveAndContinueButton()
            .HasElementWithText("button", "Calculate");
    }
}
