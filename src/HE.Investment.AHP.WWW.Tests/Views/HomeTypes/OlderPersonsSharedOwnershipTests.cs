using AngleSharp.Html.Dom;
using HE.Investment.AHP.WWW.Models.HomeTypes;

using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace HE.Investment.AHP.WWW.Tests.Views.HomeTypes;

public class OlderPersonsSharedOwnershipTests : HomeTypesTestBase
{
    private const string ViewPath = "/Views/HomeTypes/OlderPersonsSharedOwnership.cshtml";

    private static readonly OlderPersonsSharedOwnershipModel Model = new("My application", "My homes");

    [Fact]
    public async Task ShouldDisplayView_WhenThereAreNoErrors()
    {
        // given & when
        var document = await RenderHomeTypePage(ViewPath, Model);

        // then
        AssertView(document);
        AssertErrors(document, nameof(OlderPersonsSharedOwnershipModel.MarketValue), false);
        AssertErrors(document, nameof(OlderPersonsSharedOwnershipModel.InitialSale), false);
        AssertErrors(document, nameof(OlderPersonsSharedOwnershipModel.ProspectiveRent), false);
    }

    [Fact]
    public async Task ShouldDisplayView_WhenAllFieldsHaveErrors()
    {
        // given
        var modelState = new ModelStateDictionary();
        modelState.AddModelError(nameof(OlderPersonsSharedOwnershipModel.MarketValue), ErrorMessage);
        modelState.AddModelError(nameof(OlderPersonsSharedOwnershipModel.InitialSale), ErrorMessage);
        modelState.AddModelError(nameof(OlderPersonsSharedOwnershipModel.ProspectiveRent), ErrorMessage);

        // when
        var document = await RenderHomeTypePage(ViewPath, Model, modelStateDictionary: modelState);

        // then
        AssertView(document);
        AssertErrors(document, nameof(OlderPersonsSharedOwnershipModel.MarketValue), true);
        AssertErrors(document, nameof(OlderPersonsSharedOwnershipModel.InitialSale), true);
        AssertErrors(document, nameof(OlderPersonsSharedOwnershipModel.ProspectiveRent), true);
    }

    private static void AssertView(IHtmlDocument document)
    {
        document
            .HasPageHeader("My application - My homes", "Older Persons Shared Ownership (OPSO) details")
            .HasElementWithText("h2", "Enter the market value of each home")
            .HasElementWithText("span", "Enter the market value in pounds only.")
            .HasInput("MarketValue")
            .HasElementWithText("h2", "Enter the average assumed first tranche sale percentage")
            .HasElementWithText("span", "This is the average percentage share that you are assuming your purchasers will buy in their initial purchase (must be between 10% and 75%).")
            .HasInput("InitialSale")
            .HasElementWithText("h2", "Assumed first tranche sales receipt")
            .HasElementWithText("h2", "Enter the rent per week")
            .HasElementWithText("span", "Enter the rent in pounds and pence. This is inclusive of all charges.")
            .HasInput("ProspectiveRent")
            .HasElementWithText("h2", "Rent as percentage of the unsold share")
            .HasSaveAndContinueButton()
            .HasElementWithText("button", "Calculate");
    }
}
