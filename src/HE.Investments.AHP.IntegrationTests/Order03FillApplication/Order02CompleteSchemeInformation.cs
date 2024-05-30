using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using FluentAssertions;
using HE.Investment.AHP.WWW.Views.Scheme.Const;
using HE.Investment.AHP.WWW.Views.Shared;
using HE.Investments.AHP.IntegrationTests.Extensions;
using HE.Investments.AHP.IntegrationTests.Framework;
using HE.Investments.AHP.IntegrationTests.Order03FillApplication.Data;
using HE.Investments.AHP.IntegrationTests.Pages;
using HE.Investments.Common.Contract.Enum;
using HE.Investments.Common.WWW.Extensions;
using HE.Investments.IntegrationTestsFramework.Assertions;
using HE.Investments.TestsUtils.Extensions;
using Xunit;
using Xunit.Abstractions;
using Xunit.Extensions.Ordering;

namespace HE.Investments.AHP.IntegrationTests.Order03FillApplication;

[Order(302)]
[SuppressMessage("xUnit", "xUnit1004", Justification = "Waits for DevOps configuration - #76791")]
public class Order02CompleteSchemeInformation : AhpIntegrationTest
{
    public Order02CompleteSchemeInformation(AhpIntegrationTestFixture fixture, ITestOutputHelper output)
        : base(fixture, output)
    {
        var schemaInformationData = GetSharedDataOrNull<SchemeInformationData>(nameof(SchemeInformationData));
        if (schemaInformationData is null)
        {
            schemaInformationData = new SchemeInformationData();
            SetSharedData(nameof(SchemeInformationData), schemaInformationData);
        }

        SchemeInformationData = schemaInformationData;
    }

    public SchemeInformationData SchemeInformationData { get; }

    [Fact(Skip = AhpConfig.SkipTest)]
    [Order(1)]
    public async Task Order01_StartSchemeInformation()
    {
        // given
        var taskListPage = await TestClient.NavigateTo(ApplicationPagesUrl.TaskList(ApplicationData.ApplicationId));
        taskListPage.HasLinkWithTestId("enter-scheme-information", out var enterSchemeInformationLink);

        // when
        var schemaDetailsPage = await TestClient.NavigateTo(enterSchemeInformationLink);

        // then
        var continueButton = schemaDetailsPage
            .UrlEndWith(SchemeInformationPagesUrl.SchemeDetails(ApplicationData.ApplicationId))
            .HasTitle(SchemeInformationPageTitles.SchemeDetails)
            .GetLinkButton();

        await TestClient.NavigateTo(continueButton);
        SaveCurrentPage();
    }

    [Fact(Skip = AhpConfig.SkipTest)]
    [Order(2)]
    public async Task Order02_ProvideFundingDetails()
    {
        // given
        SchemeInformationData.GenerateFundingDetails();

        // when & then
        await TestQuestionPage(
            SchemeInformationPagesUrl.FundingDetails(ApplicationData.ApplicationId),
            SchemeInformationPageTitles.FundingDetails,
            SchemeInformationPagesUrl.PartnerDetails(ApplicationData.ApplicationId),
            ("RequiredFunding", SchemeInformationData.RequiredFunding.ToString(CultureInfo.InvariantCulture)),
            ("HousesToDeliver", SchemeInformationData.HousesToDeliver.ToString(CultureInfo.InvariantCulture)));
    }

    [Fact(Skip = AhpConfig.SkipTest)]
    [Order(3)]
    public async Task Order03_CheckPrefilledPartnerDetails()
    {
        // given
        var partnerDetailsPage = await GetCurrentPage(SchemeInformationPagesUrl.PartnerDetails(ApplicationData.ApplicationId));
        partnerDetailsPage
            .UrlEndWith(SchemeInformationPagesUrl.PartnerDetails(ApplicationData.ApplicationId))
            .HasTitle(SchemeInformationPageTitles.PartnerDetails)
            .HasBackLink(out _)
            .HasSaveAndContinueButton(out _);

        // when
        var summary = partnerDetailsPage.GetSummaryListItems();

        // then
        summary.Should().ContainKey("Developing partner").WithValue(SiteData.DevelopingPartner.Name);
        summary.Should().ContainKey("Owner of the land during development").WithValue(SiteData.OwnerOfTheLand.Name);
        summary.Should().ContainKey("Owner of the homes after completion").WithValue(SiteData.OwnerOfTheHomes.Name);
    }

    [Fact(Skip = AhpConfig.SkipTest)]
    [Order(4)]
    public async Task Order04_ChangeDevelopingPartner()
    {
        // given
        var partnerDetailsPage = await GetCurrentPage(SchemeInformationPagesUrl.PartnerDetails(ApplicationData.ApplicationId));
        var currentPage = await TestClient.NavigateTo(partnerDetailsPage.GetSummaryListItems()["Developing partner"].ChangeAnswerLink!);
        currentPage
            .UrlEndWith(SchemeInformationPagesUrl.DevelopingPartner(ApplicationData.ApplicationId))
            .HasTitle(SharedPageTitles.DevelopingPartner)
            .HasBackLink(out _)
            .HasPartnerSelectItems(out var partners);

        var (developingPartner, confirmLink) = partners.PickNthItem(1);

        // when
        var nextPage = await TestClient.NavigateTo(confirmLink);

        // then
        nextPage.UrlEndWith(SchemeInformationPagesUrl.DevelopingPartnerConfirmation(ApplicationData.ApplicationId, developingPartner.Id.Value));
        SchemeInformationData.DevelopingPartner = developingPartner;

        SaveCurrentPage();
    }

    [Fact(Skip = AhpConfig.SkipTest)]
    [Order(5)]
    public async Task Order05_ConfirmDevelopingPartner()
    {
        await TestQuestionPage(
            SchemeInformationPagesUrl.DevelopingPartnerConfirmation(ApplicationData.ApplicationId, SchemeInformationData.DevelopingPartner.Id.Value),
            SharedPageTitles.DevelopingPartnerConfirm,
            SchemeInformationPagesUrl.PartnerDetails(ApplicationData.ApplicationId),
            ("isPartnerConfirmed", YesNoType.Yes.ToBoolAnswer()));
    }

    [Fact(Skip = AhpConfig.SkipTest)]
    [Order(6)]
    public async Task Order06_ChangeOwnerOfTheLand()
    {
        // given
        var partnerDetailsPage = await GetCurrentPage(SchemeInformationPagesUrl.PartnerDetails(ApplicationData.ApplicationId));
        var currentPage = await TestClient.NavigateTo(partnerDetailsPage.GetSummaryListItems()["Owner of the land during development"].ChangeAnswerLink!);
        currentPage
            .UrlEndWith(SchemeInformationPagesUrl.OwnerOfTheLand(ApplicationData.ApplicationId))
            .HasTitle(SharedPageTitles.OwnerOfTheLand)
            .HasBackLink(out _)
            .HasPartnerSelectItems(out var partners);

        var (ownerOfTheLand, confirmLink) = partners.PickNthItem(2);

        // when
        var nextPage = await TestClient.NavigateTo(confirmLink);

        // then
        nextPage.UrlEndWith(SchemeInformationPagesUrl.OwnerOfTheLandConfirmation(ApplicationData.ApplicationId, ownerOfTheLand.Id.Value));
        SchemeInformationData.OwnerOfTheLand = ownerOfTheLand;

        SaveCurrentPage();
    }

    [Fact(Skip = AhpConfig.SkipTest)]
    [Order(7)]
    public async Task Order07_ConfirmOwnerOfTheLand()
    {
        await TestQuestionPage(
            SchemeInformationPagesUrl.OwnerOfTheLandConfirmation(ApplicationData.ApplicationId, SchemeInformationData.OwnerOfTheLand.Id.Value),
            SharedPageTitles.OwnerOfTheLandConfirm,
            SchemeInformationPagesUrl.PartnerDetails(ApplicationData.ApplicationId),
            ("isPartnerConfirmed", YesNoType.Yes.ToBoolAnswer()));
    }

    [Fact(Skip = AhpConfig.SkipTest)]
    [Order(8)]
    public async Task Order08_ChangeOwnerOfTheHomes()
    {
        // given
        var partnerDetailsPage = await GetCurrentPage(SchemeInformationPagesUrl.PartnerDetails(ApplicationData.ApplicationId));
        var currentPage = await TestClient.NavigateTo(partnerDetailsPage.GetSummaryListItems()["Owner of the homes after completion"].ChangeAnswerLink!);
        currentPage
            .UrlEndWith(SchemeInformationPagesUrl.OwnerOfTheHomes(ApplicationData.ApplicationId))
            .HasTitle(SharedPageTitles.OwnerOfTheHomes)
            .HasBackLink(out _)
            .HasPartnerSelectItems(out var partners);

        var (ownerOfTheHomes, confirmLink) = partners.PickNthItem(3);

        // when
        var nextPage = await TestClient.NavigateTo(confirmLink);

        // then
        nextPage.UrlEndWith(SchemeInformationPagesUrl.OwnerOfTheHomesConfirmation(ApplicationData.ApplicationId, ownerOfTheHomes.Id.Value));
        SchemeInformationData.OwnerOfTheHomes = ownerOfTheHomes;

        SaveCurrentPage();
    }

    [Fact(Skip = AhpConfig.SkipTest)]
    [Order(9)]
    public async Task Order09_ConfirmOwnerOfTheHomes()
    {
        await TestQuestionPage(
            SchemeInformationPagesUrl.OwnerOfTheHomesConfirmation(ApplicationData.ApplicationId, SchemeInformationData.OwnerOfTheHomes.Id.Value),
            SharedPageTitles.OwnerOfTheHomesConfirm,
            SchemeInformationPagesUrl.PartnerDetails(ApplicationData.ApplicationId),
            ("isPartnerConfirmed", YesNoType.Yes.ToBoolAnswer()));
    }

    [Fact(Skip = AhpConfig.SkipTest)]
    [Order(10)]
    public async Task Order10_CheckChangedPartnerDetails()
    {
        // given
        var partnerDetailsPage = await GetCurrentPage(SchemeInformationPagesUrl.PartnerDetails(ApplicationData.ApplicationId));
        partnerDetailsPage
            .UrlEndWith(SchemeInformationPagesUrl.PartnerDetails(ApplicationData.ApplicationId))
            .HasTitle(SchemeInformationPageTitles.PartnerDetails)
            .HasBackLink(out _)
            .HasSaveAndContinueButton(out _);

        // when
        var summary = partnerDetailsPage.GetSummaryListItems();

        // then
        summary.Should().ContainKey("Developing partner").WithValue(SchemeInformationData.DevelopingPartner.Name);
        summary.Should().ContainKey("Owner of the land during development").WithValue(SchemeInformationData.OwnerOfTheLand.Name);
        summary.Should().ContainKey("Owner of the homes after completion").WithValue(SchemeInformationData.OwnerOfTheHomes.Name);
    }

    [Fact(Skip = AhpConfig.SkipTest)]
    [Order(11)]
    public async Task Order11_ConfirmPartnerDetails()
    {
        await TestQuestionPage(
            SchemeInformationPagesUrl.PartnerDetails(ApplicationData.ApplicationId),
            SchemeInformationPageTitles.PartnerDetails,
            SchemeInformationPagesUrl.Affordability(ApplicationData.ApplicationId),
            ("ArePartnersConfirmed", YesNoType.Yes.ToBoolAnswer()));
    }

    [Fact(Skip = AhpConfig.SkipTest)]
    [Order(12)]
    public async Task Order12_ProvideAffordability()
    {
        // given
        SchemeInformationData.GenerateAffordability();

        // when & then
        await TestQuestionPage(
            SchemeInformationPagesUrl.Affordability(ApplicationData.ApplicationId),
            SchemeInformationPageTitles.Affordability,
            SchemeInformationPagesUrl.SalesRisk(ApplicationData.ApplicationId),
            ("AffordabilityEvidence", SchemeInformationData.Affordability));
    }

    [Fact(Skip = AhpConfig.SkipTest)]
    [Order(13)]
    public async Task Order13_ProvideSaleRisk()
    {
        // given
        SchemeInformationData.GenerateSalesRisk();

        // when & then
        await TestQuestionPage(
            SchemeInformationPagesUrl.SalesRisk(ApplicationData.ApplicationId),
            SchemeInformationPageTitles.SalesRisk,
            SchemeInformationPagesUrl.HousingNeeds(ApplicationData.ApplicationId),
            ("SalesRisk", SchemeInformationData.SalesRisk));
    }

    [Fact(Skip = AhpConfig.SkipTest)]
    [Order(14)]
    public async Task Order14_ProvideHousingNeed()
    {
        // given
        SchemeInformationData.GenerateHousingNeeds();

        // when & then
        await TestQuestionPage(
            SchemeInformationPagesUrl.HousingNeeds(ApplicationData.ApplicationId),
            SchemeInformationPageTitles.HousingNeeds,
            SchemeInformationPagesUrl.StakeholderDiscussions(ApplicationData.ApplicationId),
            ("MeetingLocalPriorities", SchemeInformationData.HousingNeedsMeetingLocalPriorities),
            ("MeetingLocalHousingNeed", SchemeInformationData.HousingNeedsMeetingLocalHousingNeed));
    }

    [Fact(Skip = AhpConfig.SkipTest)]
    [Order(15)]
    public async Task Order15_ProvideStakeholderDiscussions()
    {
        // given
        SchemeInformationData.GenerateStakeholderDiscussions();

        // when & then
        await TestQuestionPage(
            SchemeInformationPagesUrl.StakeholderDiscussions(ApplicationData.ApplicationId),
            SchemeInformationPageTitles.StakeholderDiscussions,
            SchemeInformationPagesUrl.CheckAnswers(ApplicationData.ApplicationId),
            ("StakeholderDiscussionsReport", SchemeInformationData.StakeholderDiscussions));
    }

    [Fact(Skip = AhpConfig.SkipTest)]
    [Order(16)]
    public async Task Order16_CheckAnswersAndCompleteSection()
    {
        // given
        var checkAnswersPage = await GetCurrentPage(SchemeInformationPagesUrl.CheckAnswers(ApplicationData.ApplicationId));
        checkAnswersPage
            .UrlEndWith(SchemeInformationPagesUrl.CheckAnswers(ApplicationData.ApplicationId))
            .HasTitle(SchemeInformationPageTitles.CheckAnswers)
            .HasSaveAndContinueButton(out var continueButton);

        // when
        var summary = checkAnswersPage.GetSummaryListItems();
        summary.Should().ContainKey("Application name").WithValue(ApplicationData.ApplicationName);
        summary.Should().ContainKey("Funding required").WhoseValue.Value.Should().BePoundsOnly(SchemeInformationData.RequiredFunding);
        summary.Should().ContainKey("Number of homes").WithValue(SchemeInformationData.HousesToDeliver.ToString(CultureInfo.InvariantCulture));
        summary.Should().ContainKey("Developing partner").WithValue(SchemeInformationData.DevelopingPartner.Name);
        summary.Should().ContainKey("Owner of the land").WithValue(SchemeInformationData.OwnerOfTheLand.Name);
        summary.Should().ContainKey("Owner of the homes").WithValue(SchemeInformationData.OwnerOfTheHomes.Name);
        summary.Should().ContainKey("Are partner details correct?").WithValue(YesNoType.Yes);
        summary.Should().ContainKey("Affordability of Shared Ownership").WithValue(SchemeInformationData.Affordability);
        summary.Should().ContainKey("Sales risk of Shared Ownership").WithValue(SchemeInformationData.SalesRisk);
        summary.Should().ContainKey("Type and tenure of homes").WithValue(SchemeInformationData.HousingNeedsMeetingLocalPriorities);
        summary.Should().ContainKey("Locally identified housing need").WithValue(SchemeInformationData.HousingNeedsMeetingLocalHousingNeed);
        summary.Should().ContainKey("Local stakeholder discussions").WithValue(SchemeInformationData.StakeholderDiscussions);

        var taskListPage = await TestClient.SubmitButton(
            continueButton,
            ("IsCompleted", true.MapToCommonResponse()));

        // then
        taskListPage.UrlEndWith(ApplicationPagesUrl.TaskList(ApplicationData.ApplicationId))
            .HasSectionWithStatus("enter-scheme-information-status", "Completed");
        SaveCurrentPage();
    }
}
