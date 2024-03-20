using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using HE.Investments.Common.Extensions;
using HE.Investments.FrontDoor.Contract.Project;
using HE.Investments.FrontDoor.Contract.Site;
using HE.Investments.FrontDoor.IntegrationTests.Framework;
using HE.Investments.FrontDoor.IntegrationTests.Pages;
using HE.Investments.FrontDoor.WWW;
using HE.Investments.FrontDoor.WWW.Views.Project.Const;
using HE.Investments.FrontDoor.WWW.Views.Site.Const;
using HE.Investments.IntegrationTestsFramework;
using HE.Investments.TestsUtils.Extensions;
using Xunit;
using Xunit.Extensions.Ordering;

namespace HE.Investments.FrontDoor.IntegrationTests.FillProject;

[Order(3)]
[SuppressMessage("xUnit", "xUnit1004", Justification = "Waits for DevOps configuration - #76791")]
public class Order03FrontDoorProjectSiteQuestions : FrontDoorIntegrationTest
{
    public Order03FrontDoorProjectSiteQuestions(IntegrationTestFixture<Program> fixture)
        : base(fixture)
    {
    }

    [Fact(Skip = FrontDoorConfig.SkipTest)]
    [Order(1)]
    public async Task Order01_ProvideIdentifiedSite()
    {
        ProjectData.SwitchIsSiteIdentified();

        await TestQuestionPage(
            ProjectPagesUrl.IdentifiedSite(ProjectData.Id),
            ProjectPageTitles.IdentifiedSite,
            SitePagesUrl.Name(ProjectData.Id),
            (nameof(ProjectDetails.IsSiteIdentified), ProjectData.IsSiteIdentified.MapToTrueFalse()));
    }

    [Fact(Skip = FrontDoorConfig.SkipTest)]
    [Order(2)]
    public async Task Order02_ProvideSiteName()
    {
        // given
        var currentPage = await GetCurrentPage(SitePagesUrl.Name(ProjectData.Id));
        currentPage
            .UrlEndWith(SitePagesUrl.Name(ProjectData.Id))
            .HasTitle(SitePageTitles.Name)
            .HasBackLink()
            .HasSaveAndContinueButton(out var continueButton);

        // when
        var nextPage = await TestClient.SubmitButton(
            continueButton,
            (nameof(SiteDetails.Name), SiteData.GenerateSiteName()));

        // then
        SiteData.SetId(nextPage.Url.GetSiteGuidFromUrl());
        nextPage.UrlEndWith(SitePagesUrl.HomesNumber(ProjectData.Id, SiteData.Id));

        SaveCurrentPage();
    }

    [Fact(Skip = FrontDoorConfig.SkipTest)]
    [Order(3)]
    public async Task Order03_ProvideHomesNumber()
    {
        await TestQuestionPage(
            SitePagesUrl.HomesNumber(ProjectData.Id, SiteData.Id),
            SitePageTitles.HomesNumber,
            SitePagesUrl.LocalAuthority(ProjectData.Id, SiteData.Id),
            (nameof(SiteDetails.HomesNumber), SiteData.HomesNumber.ToString(CultureInfo.InvariantCulture)));
    }
}
