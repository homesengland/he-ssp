using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using FluentAssertions;
using HE.Investment.AHP.WWW.Views.Scheme.Const;
using HE.Investment.AHP.WWW.Views.Shared;
using HE.Investments.AHP.IntegrationTests.AreaTests.O01Application.Order03FillApplication.Data;
using HE.Investments.AHP.IntegrationTests.AreaTests.O01Application.Pages;
using HE.Investments.AHP.IntegrationTests.Framework;
using HE.Investments.AHP.IntegrationTests.Framework.Extensions;
using HE.Investments.Common.Contract.Enum;
using HE.Investments.Common.WWW.Extensions;
using HE.Investments.IntegrationTestsFramework.Assertions;
using HE.Investments.TestsUtils.Extensions;
using Xunit;
using Xunit.Abstractions;
using Xunit.Extensions.Ordering;

namespace HE.Investments.AHP.IntegrationTests.AreaTests.O01Application.Order03FillApplication;

[Order(302)]
[SuppressMessage("xUnit", "xUnit1004", Justification = "Waits for DevOps configuration - #76791")]
public class Order02CompleteSchemeInformation : AhpApplicationIntegrationTest
{
    public Order02CompleteSchemeInformation(AhpIntegrationTestFixture fixture, ITestOutputHelper output)
        : base(fixture, output)
    {
        SchemeInformationData = ReturnSharedData<SchemeInformationData>();
    }

    public SchemeInformationData SchemeInformationData { get; }

    [Fact(Skip = AhpConfig.SkipTest)]
    [Order(1)]
    public async Task Order01_StartSchemeInformation()
    {
        // given
        var taskListPage = await TestClient.NavigateTo(ApplicationPagesUrl.TaskList(UserOrganisationData.OrganisationId, ApplicationData.ApplicationId));
        taskListPage.HasLinkWithTestId("enter-scheme-information", out var enterSchemeInformationLink);

        // when
        var schemaDetailsPage = await TestClient.NavigateTo(enterSchemeInformationLink);

        // then
        var continueButton = schemaDetailsPage
            .UrlEndWith(SchemeInformationPagesUrl.SchemeDetails(UserOrganisationData.OrganisationId, ApplicationData.ApplicationId))
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
        await TestApplicationQuestionPage(
            SchemeInformationPagesUrl.FundingDetails(UserOrganisationData.OrganisationId, ApplicationData.ApplicationId),
            SchemeInformationPageTitles.FundingDetails,
            SchemeInformationPagesUrl.PartnerDetails(UserOrganisationData.OrganisationId, ApplicationData.ApplicationId),
            ("RequiredFunding", SchemeInformationData.RequiredFunding.ToString(CultureInfo.InvariantCulture)),
            ("HousesToDeliver", SchemeInformationData.HousesToDeliver.ToString(CultureInfo.InvariantCulture)));
    }

    [Fact(Skip = AhpConfig.SkipTest)]
    [Order(3)]
    public async Task Order03_CheckPrefilledPartnerDetails()
    {
        // given
        var partnerDetailsPage =
            await GetCurrentPage(SchemeInformationPagesUrl.PartnerDetails(UserOrganisationData.OrganisationId, ApplicationData.ApplicationId));
        partnerDetailsPage
            .UrlEndWith(SchemeInformationPagesUrl.PartnerDetails(UserOrganisationData.OrganisationId, ApplicationData.ApplicationId))
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
        var partnerDetailsPage =
            await GetCurrentPage(SchemeInformationPagesUrl.PartnerDetails(UserOrganisationData.OrganisationId, ApplicationData.ApplicationId));
        var currentPage = await TestClient.NavigateTo(partnerDetailsPage.GetSummaryListItems()["Developing partner"].ChangeAnswerLink!);
        currentPage
            .UrlEndWith(SchemeInformationPagesUrl.DevelopingPartner(UserOrganisationData.OrganisationId, ApplicationData.ApplicationId))
            .HasTitle(SharedPageTitles.DevelopingPartner)
            .HasBackLink(out _)
            .HasPartnerSelectItems(out var partners);

        var (developingPartner, confirmLink) = partners.PickNthItem(1);

        // when
        var nextPage = await TestClient.NavigateTo(confirmLink);

        // then
        nextPage.UrlEndWith(SchemeInformationPagesUrl.DevelopingPartnerConfirmation(
            UserOrganisationData.OrganisationId,
            ApplicationData.ApplicationId,
            developingPartner.Id.Value));
        SchemeInformationData.DevelopingPartner = developingPartner;

        SaveCurrentPage();
    }

    [Fact(Skip = AhpConfig.SkipTest)]
    [Order(5)]
    public async Task Order05_ConfirmDevelopingPartner()
    {
        await TestApplicationQuestionPage(
            SchemeInformationPagesUrl.DevelopingPartnerConfirmation(
                UserOrganisationData.OrganisationId,
                ApplicationData.ApplicationId,
                SchemeInformationData.DevelopingPartner.Id.Value),
            SharedPageTitles.DevelopingPartnerConfirm,
            SchemeInformationPagesUrl.PartnerDetails(UserOrganisationData.OrganisationId, ApplicationData.ApplicationId),
            ("isPartnerConfirmed", YesNoType.Yes.ToBoolAnswer()));
    }

    [Fact(Skip = AhpConfig.SkipTest)]
    [Order(6)]
    public async Task Order06_ChangeOwnerOfTheLand()
    {
        // given
        var partnerDetailsPage =
            await GetCurrentPage(SchemeInformationPagesUrl.PartnerDetails(UserOrganisationData.OrganisationId, ApplicationData.ApplicationId));
        var currentPage = await TestClient.NavigateTo(partnerDetailsPage.GetSummaryListItems()["Owner of the land during development"].ChangeAnswerLink!);
        currentPage
            .UrlEndWith(SchemeInformationPagesUrl.OwnerOfTheLand(UserOrganisationData.OrganisationId, ApplicationData.ApplicationId))
            .HasTitle(SharedPageTitles.OwnerOfTheLand)
            .HasBackLink(out _)
            .HasPartnerSelectItems(out var partners);

        var (ownerOfTheLand, confirmLink) = partners.PickNthItem(2);

        // when
        var nextPage = await TestClient.NavigateTo(confirmLink);

        // then
        nextPage.UrlEndWith(SchemeInformationPagesUrl.OwnerOfTheLandConfirmation(
            UserOrganisationData.OrganisationId,
            ApplicationData.ApplicationId,
            ownerOfTheLand.Id.Value));
        SchemeInformationData.OwnerOfTheLand = ownerOfTheLand;

        SaveCurrentPage();
    }

    [Fact(Skip = AhpConfig.SkipTest)]
    [Order(7)]
    public async Task Order07_ConfirmOwnerOfTheLand()
    {
        await TestApplicationQuestionPage(
            SchemeInformationPagesUrl.OwnerOfTheLandConfirmation(
                UserOrganisationData.OrganisationId,
                ApplicationData.ApplicationId,
                SchemeInformationData.OwnerOfTheLand.Id.Value),
            SharedPageTitles.OwnerOfTheLandConfirm,
            SchemeInformationPagesUrl.PartnerDetails(UserOrganisationData.OrganisationId, ApplicationData.ApplicationId),
            ("isPartnerConfirmed", YesNoType.Yes.ToBoolAnswer()));
    }

    [Fact(Skip = AhpConfig.SkipTest)]
    [Order(8)]
    public async Task Order08_ChangeOwnerOfTheHomes()
    {
        // given
        var partnerDetailsPage =
            await GetCurrentPage(SchemeInformationPagesUrl.PartnerDetails(UserOrganisationData.OrganisationId, ApplicationData.ApplicationId));
        var currentPage = await TestClient.NavigateTo(partnerDetailsPage.GetSummaryListItems()["Owner of the homes after completion"].ChangeAnswerLink!);
        currentPage
            .UrlEndWith(SchemeInformationPagesUrl.OwnerOfTheHomes(UserOrganisationData.OrganisationId, ApplicationData.ApplicationId))
            .HasTitle(SharedPageTitles.OwnerOfTheHomes)
            .HasBackLink(out _)
            .HasPartnerSelectItems(out var partners);

        var (ownerOfTheHomes, confirmLink) = partners.PickNthItem(3);

        // when
        var nextPage = await TestClient.NavigateTo(confirmLink);

        // then
        nextPage.UrlEndWith(SchemeInformationPagesUrl.OwnerOfTheHomesConfirmation(
            UserOrganisationData.OrganisationId,
            ApplicationData.ApplicationId,
            ownerOfTheHomes.Id.Value));
        SchemeInformationData.OwnerOfTheHomes = ownerOfTheHomes;

        SaveCurrentPage();
    }

    [Fact(Skip = AhpConfig.SkipTest)]
    [Order(9)]
    public async Task Order09_ConfirmOwnerOfTheHomes()
    {
        await TestApplicationQuestionPage(
            SchemeInformationPagesUrl.OwnerOfTheHomesConfirmation(
                UserOrganisationData.OrganisationId,
                ApplicationData.ApplicationId,
                SchemeInformationData.OwnerOfTheHomes.Id.Value),
            SharedPageTitles.OwnerOfTheHomesConfirm,
            SchemeInformationPagesUrl.PartnerDetails(UserOrganisationData.OrganisationId, ApplicationData.ApplicationId),
            ("isPartnerConfirmed", YesNoType.Yes.ToBoolAnswer()));
    }

    [Fact(Skip = AhpConfig.SkipTest)]
    [Order(10)]
    public async Task Order10_CheckChangedPartnerDetails()
    {
        // given
        var partnerDetailsPage =
            await GetCurrentPage(SchemeInformationPagesUrl.PartnerDetails(UserOrganisationData.OrganisationId, ApplicationData.ApplicationId));
        partnerDetailsPage
            .UrlEndWith(SchemeInformationPagesUrl.PartnerDetails(UserOrganisationData.OrganisationId, ApplicationData.ApplicationId))
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
        await TestApplicationQuestionPage(
            SchemeInformationPagesUrl.PartnerDetails(UserOrganisationData.OrganisationId, ApplicationData.ApplicationId),
            SchemeInformationPageTitles.PartnerDetails,
            SchemeInformationPagesUrl.HousingNeeds(UserOrganisationData.OrganisationId, ApplicationData.ApplicationId),
            ("ArePartnersConfirmed", YesNoType.Yes.ToBoolAnswer()));
    }

    [Fact(Skip = AhpConfig.SkipTest)]
    [Order(12)]
    public async Task Order12_ProvideHousingNeed()
    {
        // given
        SchemeInformationData.GenerateHousingNeeds();

        // when & then
        await TestApplicationQuestionPage(
            SchemeInformationPagesUrl.HousingNeeds(UserOrganisationData.OrganisationId, ApplicationData.ApplicationId),
            SchemeInformationPageTitles.HousingNeeds,
            SchemeInformationPagesUrl.StakeholderDiscussions(UserOrganisationData.OrganisationId, ApplicationData.ApplicationId),
            ("MeetingLocalPriorities", SchemeInformationData.HousingNeedsMeetingLocalPriorities),
            ("MeetingLocalHousingNeed", SchemeInformationData.HousingNeedsMeetingLocalHousingNeed));
    }

    [Fact(Skip = AhpConfig.SkipTest)]
    [Order(13)]
    public async Task Order13_ProvideStakeholderDiscussions()
    {
        // given
        SchemeInformationData.GenerateStakeholderDiscussions();

        // when & then
        await TestApplicationQuestionPage(
            SchemeInformationPagesUrl.StakeholderDiscussions(UserOrganisationData.OrganisationId, ApplicationData.ApplicationId),
            SchemeInformationPageTitles.StakeholderDiscussions,
            SchemeInformationPagesUrl.CheckAnswers(UserOrganisationData.OrganisationId, ApplicationData.ApplicationId),
            ("StakeholderDiscussionsReport", SchemeInformationData.StakeholderDiscussions));
    }

    [Fact(Skip = AhpConfig.SkipTest)]
    [Order(14)]
    public async Task Order14_CheckAnswersAndCompleteSection()
    {
        // given
        var checkAnswersPage = await GetCurrentPage(SchemeInformationPagesUrl.CheckAnswers(UserOrganisationData.OrganisationId, ApplicationData.ApplicationId));
        checkAnswersPage
            .UrlEndWith(SchemeInformationPagesUrl.CheckAnswers(UserOrganisationData.OrganisationId, ApplicationData.ApplicationId))
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
        summary.Should().NotContainKey("Affordability of Shared Ownership");
        summary.Should().NotContainKey("Sales risk of Shared Ownership");
        summary.Should().ContainKey("Type and tenure of homes").WithValue(SchemeInformationData.HousingNeedsMeetingLocalPriorities);
        summary.Should().ContainKey("Locally identified housing need").WithValue(SchemeInformationData.HousingNeedsMeetingLocalHousingNeed);
        summary.Should().ContainKey("Local stakeholder discussions").WithValue(SchemeInformationData.StakeholderDiscussions);

        var taskListPage = await TestClient.SubmitButton(
            continueButton,
            ("IsCompleted", true.MapToCommonResponse()));

        // then
        taskListPage.UrlEndWith(ApplicationPagesUrl.TaskList(UserOrganisationData.OrganisationId, ApplicationData.ApplicationId))
            .HasSectionWithStatus("enter-scheme-information-status", "Completed");
        SaveCurrentPage();
    }
}
