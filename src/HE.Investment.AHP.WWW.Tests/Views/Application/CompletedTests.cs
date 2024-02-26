using AngleSharp.Html.Dom;
using HE.Investment.AHP.WWW.Models.Application;
using HE.Investment.AHP.WWW.Views.Application;

namespace HE.Investment.AHP.WWW.Tests.Views.Application;

public class CompletedTests : AhpViewTestBase
{
    private readonly string _viewPath = "/Views/Application/Completed.cshtml";

    private readonly ApplicationSubmitModel _model = new(
        "testId",
        "testName",
        "testNumber",
        "SiteName",
        "testTenure",
        "15",
        "250",
        "500000",
        "testWarranties");

    [Fact]
    public async Task ShouldDisplayView()
    {
        // given & when
        var document = await Render(_viewPath, _model);

        // then
        AssertView(document);
    }

    private static void AssertView(IHtmlDocument document)
    {
        document
            .HasPanel(ApplicationPageTitles.Completed, "Your reference number", "testNumber", true)
            .HasHeader2("Application submitted")
            .HasTableRowsHeaders(new()
            {
                "Site name",
                "Scheme name and tenure",
                "Number of homes",
                "Funding requested",
                "Scheme cost",
            })
            .HasParagraph("We have sent you a confirmation email. You can log back in to your account to see the progress of your application at any time.")
            .HasParagraph("We will contact you either to confirm your registration, or to ask for more information.")
            .HasParagraph("If you have not been contacted within 10 working days, you should contact [INSERT CONTACT DETAILS]");
    }
}
