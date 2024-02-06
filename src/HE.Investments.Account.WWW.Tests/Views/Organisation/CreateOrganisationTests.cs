using AngleSharp.Html.Dom;
using HE.Investments.Account.Contract.Organisation;

namespace HE.Investments.Account.WWW.Tests.Views.Organisation;

public class CreateOrganisationTests : ViewTestBase
{
    private readonly string _viewPath = "/Views/Organisation/CreateOrganisation.cshtml";

    [Fact]
    public async Task ShouldDisplayView()
    {
        // given & when
        var document = await Render<OrganisationDetailsViewModel>(_viewPath);

        // then
        AssertView(document);
    }

    private static void AssertView(IHtmlDocument document)
    {
        document
            .HasElementWithText("h1", "Organisation Details")
            .HasElementWithText("p", "Enter your organisation’s name manually")
            .HasInput("Name")
            .HasElementWithText("h1", "Registered address")
            .HasElementWithText(
                "p",
                "If you have not established your organisation, enter the intended registered address.")
            .HasInput("AddressLine1", "Address line 1")
            .HasInput("AddressLine2", "Address line 2 (optional)")
            .HasInput("TownOrCity", "Town or city")
            .HasInput("County", "County (optional)")
            .HasInput("Postcode", "Postcode")
            .HasElementWithText("button", "Continue");
    }
}