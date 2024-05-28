using System.Diagnostics.CodeAnalysis;
using HE.Investment.AHP.WWW.Views.Shared;
using HE.Investments.AHP.IntegrationTests.Extensions;
using HE.Investments.AHP.IntegrationTests.Framework;
using HE.Investments.AHP.IntegrationTests.Pages;
using HE.Investments.Common.Contract.Enum;
using HE.Investments.TestsUtils.Extensions;
using Xunit;
using Xunit.Abstractions;
using Xunit.Extensions.Ordering;

namespace HE.Investments.AHP.IntegrationTests.Order02FillSite;

[Order(206)]
[SuppressMessage("xUnit", "xUnit1004", Justification = "Waits for DevOps configuration - #76791")]
public class Order06ConsortiumSection : AhpIntegrationTest
{
    public Order06ConsortiumSection(AhpIntegrationTestFixture fixture, ITestOutputHelper output)
        : base(fixture, output)
    {
    }

    [Fact(Skip = AhpConfig.SkipTest)]
    [Order(1)]
    public async Task Order01_ShouldSelectDevelopingPartner()
    {
        // given
        var currentPage = await GetCurrentPage(SitePagesUrl.SiteDevelopingPartner(SiteData.SiteId));
        currentPage
            .UrlEndWith(SitePagesUrl.SiteDevelopingPartner(SiteData.SiteId))
            .HasTitle(SharedPageTitles.DevelopingPartner)
            .HasBackLink(out _)
            .HasPartnerSelectItems(out var partners);

        var (developingPartner, confirmLink) = partners.PickNthItem(0);

        // when
        var nextPage = await TestClient.NavigateTo(confirmLink);

        // then
        nextPage.UrlEndWith(SitePagesUrl.SiteDevelopingPartnerConfirmation(SiteData.SiteId, developingPartner.Id.Value));
        SiteData.DevelopingPartner = developingPartner;

        SaveCurrentPage();
    }

    [Fact(Skip = AhpConfig.SkipTest)]
    [Order(2)]
    public async Task Order02_ShouldConfirmDevelopingPartner()
    {
        await TestQuestionPage(
            SitePagesUrl.SiteDevelopingPartnerConfirmation(SiteData.SiteId, SiteData.DevelopingPartner.Id.Value),
            SharedPageTitles.DevelopingPartnerConfirm,
            SitePagesUrl.SiteOwnerOfTheLand(SiteData.SiteId),
            ("isConfirmed", YesNoType.Yes.ToBoolAnswer()));
    }

    [Fact(Skip = AhpConfig.SkipTest)]
    [Order(3)]
    public async Task Order03_ShouldSelectOwnerOfTheLand()
    {
        // given
        var currentPage = await GetCurrentPage(SitePagesUrl.SiteOwnerOfTheLand(SiteData.SiteId));
        currentPage
            .UrlEndWith(SitePagesUrl.SiteOwnerOfTheLand(SiteData.SiteId))
            .HasTitle(SharedPageTitles.OwnerOfTheLand)
            .HasBackLink(out _)
            .HasPartnerSelectItems(out var partners);

        var (ownerOfTheLand, confirmLink) = partners.PickNthItem(1);

        // when
        var nextPage = await TestClient.NavigateTo(confirmLink);

        // then
        nextPage.UrlEndWith(SitePagesUrl.SiteOwnerOfTheLandConfirmation(SiteData.SiteId, ownerOfTheLand.Id.Value));
        SiteData.OwnerOfTheLand = ownerOfTheLand;

        SaveCurrentPage();
    }

    [Fact(Skip = AhpConfig.SkipTest)]
    [Order(4)]
    public async Task Order04_ShouldConfirmOwnerOfTheLand()
    {
        await TestQuestionPage(
            SitePagesUrl.SiteOwnerOfTheLandConfirmation(SiteData.SiteId, SiteData.OwnerOfTheLand.Id.Value),
            SharedPageTitles.OwnerOfTheLandConfirm,
            SitePagesUrl.SiteOwnerOfTheHomes(SiteData.SiteId),
            ("isConfirmed", YesNoType.Yes.ToBoolAnswer()));
    }

    [Fact(Skip = AhpConfig.SkipTest)]
    [Order(5)]
    public async Task Order05_ShouldSelectOwnerOfTheHomes()
    {
        // given
        var currentPage = await GetCurrentPage(SitePagesUrl.SiteOwnerOfTheHomes(SiteData.SiteId));
        currentPage
            .UrlEndWith(SitePagesUrl.SiteOwnerOfTheHomes(SiteData.SiteId))
            .HasTitle(SharedPageTitles.OwnerOfTheHomes)
            .HasBackLink(out _)
            .HasPartnerSelectItems(out var partners);

        var (ownerOfTheHomes, confirmLink) = partners.PickNthItem(2);

        // when
        var nextPage = await TestClient.NavigateTo(confirmLink);

        // then
        nextPage.UrlEndWith(SitePagesUrl.SiteOwnerOfTheHomesConfirmation(SiteData.SiteId, ownerOfTheHomes.Id.Value));
        SiteData.OwnerOfTheHomes = ownerOfTheHomes;

        SaveCurrentPage();
    }

    [Fact(Skip = AhpConfig.SkipTest)]
    [Order(6)]
    public async Task Order06_ShouldConfirmOwnerOfTheHomes()
    {
        await TestQuestionPage(
            SitePagesUrl.SiteOwnerOfTheHomesConfirmation(SiteData.SiteId, SiteData.OwnerOfTheHomes.Id.Value),
            SharedPageTitles.OwnerOfTheHomesConfirm,
            SitePagesUrl.SiteLandAcquisitionStatus(SiteData.SiteId),
            ("isConfirmed", YesNoType.Yes.ToBoolAnswer()));
    }
}
