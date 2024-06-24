using System.Diagnostics.CodeAnalysis;
using HE.Investment.AHP.Contract.Site;
using HE.Investment.AHP.WWW.Views.Site.Const;
using HE.Investments.AHP.IntegrationTests.Extensions;
using HE.Investments.AHP.IntegrationTests.Framework;
using HE.Investments.AHP.IntegrationTests.Pages;
using Xunit;
using Xunit.Abstractions;
using Xunit.Extensions.Ordering;

namespace HE.Investments.AHP.IntegrationTests.Order02FillSite;

[Order(202)]
[SuppressMessage("xUnit", "xUnit1004", Justification = "Waits for DevOps configuration - #76791")]
public class Order02Section106 : AhpSiteIntegrationTest
{
    public Order02Section106(AhpIntegrationTestFixture fixture, ITestOutputHelper output)
        : base(fixture, output)
    {
    }

    [Fact(Skip = AhpConfig.SkipTest)]
    [Order(1)]
    public async Task Order01_ShouldProvideSection106GeneralAgreementAndNavigateToSection106AffordableHousing()
    {
        await TestSiteQuestionPage(
            SitePagesUrl.SiteSection106GeneralAgreement(UserOrganisationData.OrganisationId, SiteData.SiteId),
            SitePageTitles.SiteSection106Agreement,
            SitePagesUrl.SiteSection106AffordableHousing(UserOrganisationData.OrganisationId, SiteData.SiteId),
            (nameof(SiteModel.Section106.GeneralAgreement), SiteData.Section106GeneralAgreement.ToBoolAnswer()));
    }

    [Fact(Skip = AhpConfig.SkipTest)]
    [Order(2)]
    public async Task Order02_ShouldProvideSection106AffordableHousingAndNavigateToSection106OnlyAffordableHousing()
    {
        await TestSiteQuestionPage(
            SitePagesUrl.SiteSection106AffordableHousing(UserOrganisationData.OrganisationId, SiteData.SiteId),
            SitePageTitles.SiteSection106AffordableHousing,
            SitePagesUrl.SiteSection106OnlyAffordableHousing(UserOrganisationData.OrganisationId, SiteData.SiteId),
            (nameof(SiteModel.Section106.AffordableHousing), SiteData.Section106AffordableHousing.ToBoolAnswer()));
    }

    [Fact(Skip = AhpConfig.SkipTest)]
    [Order(3)]
    public async Task Order03_ShouldProvideSection106OnlyAffordableHousingAndNavigateToSection106AdditionalAffordableHousing()
    {
        await TestSiteQuestionPage(
            SitePagesUrl.SiteSection106OnlyAffordableHousing(UserOrganisationData.OrganisationId, SiteData.SiteId),
            SitePageTitles.SiteSection106OnlyAffordableHousing,
            SitePagesUrl.SiteSection106AdditionalAffordableHousing(UserOrganisationData.OrganisationId, SiteData.SiteId),
            (nameof(SiteModel.Section106.OnlyAffordableHousing), SiteData.Section106OnlyAffordableHousing.ToBoolAnswer()));
    }

    [Fact(Skip = AhpConfig.SkipTest)]
    [Order(4)]
    public async Task Order04_ShouldProvideSection106AdditionalAffordableHousingAndNavigateToSection106CapitalFundingEligibility()
    {
        await TestSiteQuestionPage(
            SitePagesUrl.SiteSection106AdditionalAffordableHousing(UserOrganisationData.OrganisationId, SiteData.SiteId),
            SitePageTitles.SiteSection106AdditionalAffordableHousing,
            SitePagesUrl.SiteSection106CapitalFundingEligibility(UserOrganisationData.OrganisationId, SiteData.SiteId),
            (nameof(SiteModel.Section106.AdditionalAffordableHousing), SiteData.Section106AdditionalAffordableHousing.ToBoolAnswer()));
    }

    [Fact(Skip = AhpConfig.SkipTest)]
    [Order(5)]
    public async Task Order05_ShouldProvideSection106CapitalFundingEligibilityAndNavigateToSection106LocalAuthorityConfirmation()
    {
        await TestSiteQuestionPage(
            SitePagesUrl.SiteSection106CapitalFundingEligibility(UserOrganisationData.OrganisationId, SiteData.SiteId),
            SitePageTitles.SiteSection106CapitalFundingEligibility,
            SitePagesUrl.SiteSection106LocalAuthorityConfirmation(UserOrganisationData.OrganisationId, SiteData.SiteId),
            (nameof(SiteModel.Section106.CapitalFundingEligibility), SiteData.Section106CapitalFundingEligibility.ToBoolAnswer()));
    }

    [Fact(Skip = AhpConfig.SkipTest)]
    [Order(6)]
    public async Task Order06_ShouldProvideSection106LocalAuthorityConfirmationAndNavigateToLocalAuthoritySearch()
    {
        await TestSiteQuestionPage(
            SitePagesUrl.SiteSection106LocalAuthorityConfirmation(UserOrganisationData.OrganisationId, SiteData.SiteId),
            SitePageTitles.SiteSection106LocalAuthorityConfirmation,
            SitePagesUrl.SiteLocalAuthoritySearch(UserOrganisationData.OrganisationId, SiteData.SiteId),
            (nameof(SiteModel.Section106.LocalAuthorityConfirmation), SiteData.GenerateLocalAuthorityConfirmation()));
    }
}
