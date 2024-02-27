using HE.Investment.AHP.Contract.Site;
using HE.Investment.AHP.WWW.Views.Site.Const;
using HE.Investments.Common.WWW.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace HE.Investment.AHP.WWW.Tests.Views.Site;

public class LocalAuthorityConfirmTests : AhpViewTestBase
{
    private readonly string _viewPath = "/Views/Site/LocalAuthorityConfirm.cshtml";

    [Fact]
    public async Task ShouldDisplayView()
    {
        // given & when
        var confirmModel = GetConfirmModel();
        var viewBag = new Dictionary<string, object> { { "SiteName", " some site name" } };
        var document = await Render(_viewPath, confirmModel, viewBag);

        // then
        document
            .HasTitle(SitePageTitles.LocalAuthorityConfirm)
            .HasPageHeader(viewBag["SiteName"].ToString(), SitePageTitles.LocalAuthorityConfirm)
            .HasBoldText("Liverpool")
            .HasHeader2("Is this the correct local authority?")
            .HasRadio("Response", new[] { "Yes", "No" })
            .HasBackLink(false);
    }

    [Fact]
    public async Task ShouldErrorSummary_ForInvalidName()
    {
        // given
        var errorMessage = "some test error";
        var modelState = new ModelStateDictionary();
        var confirmModel = GetConfirmModel();
        var viewBag = new Dictionary<string, object> { { "SiteName", " some site name" } };
        modelState.AddModelError(nameof(ConfirmModel<LocalAuthorities>.Response), errorMessage);

        // when
        var document = await Render(_viewPath, confirmModel, viewBag, modelState);

        // then
        document
            .HasTitle(SitePageTitles.LocalAuthorityConfirm)
            .HasPageHeader(viewBag["SiteName"].ToString(), SitePageTitles.LocalAuthorityConfirm)
            .HasBoldText("Liverpool")
            .HasHeader2("Is this the correct local authority?")
            .HasRadio("Response", new[] { "Yes", "No" })
            .HasBackLink(false)
            .HasOneValidationMessages(errorMessage);
    }

    private ConfirmModel<LocalAuthorities> GetConfirmModel()
    {
        return new ConfirmModel<LocalAuthorities> { ViewModel = new LocalAuthorities { LocalAuthorityId = "1", LocalAuthorityName = "Liverpool" } };
    }
}
