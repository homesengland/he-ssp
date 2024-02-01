using HE.Investment.AHP.Contract.Site;
using HE.Investment.AHP.WWW.Views.Site;
using HE.Investments.Common.WWW.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace HE.Investment.AHP.WWW.Tests.Views.Site;

public class LocalAuthorityConfirmTests : ViewTestBase
{
    private readonly string _viewPath = "/Views/Site/LocalAuthorityConfirm.cshtml";

    [Fact]
    public async Task ShouldDisplayView()
    {
        // given & when
        var confirmModel = GetConfirmModel();
        var document = await Render(_viewPath, confirmModel);

        // then
        document
            .HasTitle(SitePageTitles.LocalAuthorityConfirm)
            .HasPageHeader(header: SitePageTitles.LocalAuthorityConfirm)
            .HasBoldText("Liverpool")
            .HasHeader2("Is this the correct local authority?")
            .HasRadio("Response", new[] { "Yes", "No" })
            .HasGdsBackLink(false);
    }

    [Fact]
    public async Task ShouldErrorSummary_ForInvalidName()
    {
        // given
        var errorMessage = "some test error";
        var modelState = new ModelStateDictionary();
        var confirmModel = GetConfirmModel();
        modelState.AddModelError(nameof(ConfirmModel<LocalAuthorities>.Response), errorMessage);

        // when
        var document = await Render(_viewPath, confirmModel, modelStateDictionary: modelState);

        // then
        document
            .HasTitle(SitePageTitles.LocalAuthorityConfirm)
            .HasPageHeader(header: SitePageTitles.LocalAuthorityConfirm)
            .HasBoldText("Liverpool")
            .HasHeader2("Is this the correct local authority?")
            .HasRadio("Response", new[] { "Yes", "No" })
            .HasGdsBackLink(false)
            .HasOneValidationMessages(errorMessage);
    }

    private ConfirmModel<LocalAuthorities> GetConfirmModel()
    {
        return new ConfirmModel<LocalAuthorities> { ViewModel = new LocalAuthorities { LocalAuthorityId = "1", LocalAuthorityName = "Liverpool" } };
    }
}
