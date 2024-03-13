using AngleSharp.Html.Dom;
using HE.Investments.Account.Contract.Organisation;
using HE.Investments.Account.WWW.Views.Organisation;

namespace HE.Investments.Account.WWW.Tests.Views.Organisation;

public class CreateOrganisationTests : ViewTestBase
{
    private readonly string _viewPath = "/Views/Organisation/CreateOrganisation.cshtml";

    [Fact]
    public async Task ShouldDisplayView()
    {
        // given & when
        var document = await Render(_viewPath, new OrganisationDetailsViewModel());

        // then
        AssertView(document);
    }

    private static void AssertView(IHtmlDocument document)
    {
        document
            .HasTitle(OrganisationPageTitles.CreateOrganisation)
            .HasParagraph("This may mean you have not yet established your organisation, or we could not find it in our initial search.")
            .HasInput("Name")
            .HasHeader2(OrganisationPageTitles.OrganisationAddress)
            .HasHint("Enter your organisation name as it would appear on Companies House or any legal document.")
            .HasHint("If you have not yet registered your organisation, enter your intended details below.")
            .HasInput("AddressLine1", "Address line 1")
            .HasInput("AddressLine2", "Address line 2 (optional)")
            .HasInput("TownOrCity", "Town or city")
            .HasInput("County", "County (optional)")
            .HasInput("Postcode", "Postcode")
            .HasSaveAndContinueButton();
    }
}
