using HE.Investment.AHP.Contract.Application;
using HE.Investment.AHP.Contract.Site;
using HE.Investments.Common.Contract;

namespace HE.Investment.AHP.WWW.Tests.Views.Site;

public class DetailsTests : AhpViewTestBase
{
    private readonly string _viewPath = "/Views/Site/Details.cshtml";

    [Fact]
    public async Task ShouldDisplayView_WhenSiteIsEmpty()
    {
        // given
        var model = new SiteDetailsModel(new SiteId("123"), "My new Site", "PwC Organisation", null, new(Array.Empty<ApplicationSiteModel>(), 1, 10, 0));

        // when
        var document = await Render(_viewPath, model);

        // then
        document
            .HasPageHeader(model.OrganisationName, model.SiteName)
            .HasSummaryItem("Local authority", exists: false)
            .HasHeader2("View details of this site")
            .HasParagraph("View or edit this site's details. You cannot edit the details for a site if an application connected to it has already been submitted.")
            .HasLinkButton("View details")
            .HasHeader2("Applications on this site", false)
            .HasParagraph("View or manage existing applications for this site.", false)
            .HasLinkWithTestId("application-list", out _)
            .HasBackLink(out _, false);
    }

    [Fact]
    public async Task ShouldDisplayView_WhenSiteHasSomeApplications()
    {
        // given
        var application = new ApplicationSiteModel(new AhpApplicationId("321"), "My Application", Tenure.AffordableRent, 10, ApplicationStatus.Draft);
        var model = new SiteDetailsModel(
            new SiteId("123"),
            "My new Site",
            "PwC Organisation",
            "My local authority",
            new(new[] { application }, 1, 10, 1));

        // when
        var document = await Render(_viewPath, model);

        // then
        document
            .HasPageHeader(model.OrganisationName, model.SiteName)
            .HasSummaryItem("Local authority", "My local authority")
            .HasHeader2("View details of this site")
            .HasParagraph("View or edit this site's details. You cannot edit the details for a site if an application connected to it has already been submitted.")
            .HasLinkButton("View details")
            .HasHeader2("Applications on this site")
            .HasParagraph("View or manage existing applications for this site.")
            .HasLinkWithHref("/Application/TaskList?applicationId=321", out _)
            .HasLinkWithTestId("application-list", out _)
            .HasBackLink(out _, false);
    }
}
