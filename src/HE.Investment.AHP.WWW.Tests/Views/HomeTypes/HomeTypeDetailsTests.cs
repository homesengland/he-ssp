using AngleSharp.Html.Dom;
using HE.Investment.AHP.WWW.Models.HomeTypes;
using HE.Investments.Common.WWWTestsFramework.Helpers;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace HE.Investment.AHP.WWW.Tests.Views.HomeTypes;

public class HomeTypeDetailsTests : HomeTypesTestBase
{
    private const string ViewPath = "/Views/HomeTypes/HomeTypeDetails.cshtml";

    private static readonly HomeTypeDetailsModel Model = new("My application");

    [Fact]
    public async Task ShouldDisplayView_WhenThereAreNoErrors()
    {
        // given & when
        var document = await RenderHomeTypePage(ViewPath, Model);

        // then
        AssertView(document);
        AssertErrors(document, nameof(HomeTypeDetailsModel.HomeTypeName), false);
    }

    [Fact]
    public async Task ShouldDisplayView_WhenHomeTypeNameHasErrors()
    {
        // given
        var modelState = new ModelStateDictionary();
        modelState.AddModelError(nameof(HomeTypeDetailsModel.HomeTypeName), ErrorMessage);

        // when
        var document = await RenderHomeTypePage(ViewPath, Model, modelStateDictionary: modelState);

        // then
        AssertView(document);
        AssertErrors(document, nameof(HomeTypeDetailsModel.HomeTypeName), true);
    }

    private static void AssertView(IHtmlDocument document)
    {
        document
            .HasElementWithText("span", Model.ApplicationName)
            .HasElementWithText("h1", "Home type details")
            .HasElementWithText("h2", "Home type name")
            .HasInput("HomeTypeName")
            .HasElementWithText("h2", "What type of homes are you delivering?")
            .HasRadio("HousingType", new[] { "General", "HomesForOlderPeople", "HomesForDisabledAndVulnerablePeople" })
            .HasElementWithText("button", "Save and continue");
    }
}
