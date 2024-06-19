using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using AngleSharp.Html.Dom;
using FluentAssertions;
using HE.Investments.Common.Extensions;
using HE.Investments.FrontDoor.Contract.Project;
using HE.Investments.FrontDoor.IntegrationTests.Framework;
using HE.Investments.FrontDoor.IntegrationTests.Pages;
using HE.Investments.FrontDoor.WWW.Views.Project.Const;
using HE.Investments.TestsUtils.Assertions;
using HE.Investments.TestsUtils.Extensions;
using Xunit;
using Xunit.Abstractions;
using Xunit.Extensions.Ordering;

namespace HE.Investments.FrontDoor.IntegrationTests.FillProject;

[Order(1)]
[SuppressMessage("xUnit", "xUnit1004", Justification = "Waits for DevOps configuration - #76791")]
public class Order01CreateFrontDoorProject : FrontDoorIntegrationTest
{
    public Order01CreateFrontDoorProject(FrontDoorIntegrationTestFixture fixture, ITestOutputHelper output)
        : base(fixture, output)
    {
    }

    [Fact(Skip = FrontDoorConfig.SkipTest)]
    [Order(1)]
    public async Task Order01_ProjectsListPage()
    {
        // given
        var currentPage = await GetCurrentPage(ProjectsPagesUrl.List(UserOrganisationData.OrganisationId));
        currentPage
            .UrlEndWith(ProjectsPagesUrl.List(UserOrganisationData.OrganisationId))
            .HasTitle("Morelas Ltd. Homes England account")
            .HasLinkButtonForTestId("start-new-project", out var startProjectLink);

        // when
        var nextPage = await TestClient.NavigateTo(startProjectLink);

        // then
        nextPage.UrlEndWith(ProjectPagesUrl.Start(UserOrganisationData.OrganisationId));
        SaveCurrentPage();
    }

    [Fact(Skip = FrontDoorConfig.SkipTest)]
    [Order(2)]
    public async Task Order02_ProjectStartPage()
    {
        // given
        var currentPage = await GetCurrentPage(ProjectPagesUrl.Start(UserOrganisationData.OrganisationId));
        var startNowLink = currentPage
            .UrlEndWith(ProjectPagesUrl.Start(UserOrganisationData.OrganisationId))
            .HasTitle(ProjectPageTitles.Start)
            .GetStartButton("Start now")
            .Parent as IHtmlAnchorElement;

        startNowLink.Should().NotBeNull();
        startNowLink!.Href.Should().NotBeNull();

        // when
        var nextPage = await TestClient.NavigateTo(startNowLink.Href);

        // then
        nextPage.UrlEndWith(ProjectPagesUrl.NewEnglandHousingDelivery(UserOrganisationData.OrganisationId));
        SaveCurrentPage();
    }

    [Fact(Skip = FrontDoorConfig.SkipTest)]
    [Order(3)]
    public async Task Order03_ProvideEnglandHousingDelivery()
    {
        // given
        var currentPage = await GetCurrentPage(ProjectPagesUrl.NewEnglandHousingDelivery(UserOrganisationData.OrganisationId));
        currentPage
            .UrlEndWith(ProjectPagesUrl.NewEnglandHousingDelivery(UserOrganisationData.OrganisationId))
            .HasTitle(ProjectPageTitles.EnglandHousingDelivery)
            .HasBackLink(out _)
            .HasContinueButton(out var continueButton);

        // when
        var nextPage = await TestClient.SubmitButton(
            continueButton,
            (nameof(ProjectDetails.IsEnglandHousingDelivery), ProjectData.IsEnglandHousingDelivery.MapToTrueFalse()));

        // then
        ThenTestQuestionPage(nextPage, ProjectPagesUrl.NewName(UserOrganisationData.OrganisationId));
    }

    [Fact(Skip = FrontDoorConfig.SkipTest)]
    [Order(4)]
    public async Task Order04_ProvideProjectName()
    {
        // given
        var currentPage = await GetCurrentPage(ProjectPagesUrl.NewName(UserOrganisationData.OrganisationId));
        currentPage
            .UrlEndWith(ProjectPagesUrl.NewName(UserOrganisationData.OrganisationId))
            .HasTitle(ProjectPageTitles.Name)
            .HasBackLink(out _)
            .HasSaveAndContinueButton(out var continueButton);

        // when
        var nextPage = await TestClient.SubmitButton(
            continueButton,
            (nameof(ProjectDetails.Name), ProjectData.GenerateProjectName()));

        // then
        ProjectData.SetProjectId(nextPage.Url.GetProjectGuidFromUrl());
        nextPage.UrlEndWith(ProjectPagesUrl.SupportRequiredActivities(UserOrganisationData.OrganisationId, ProjectData.Id));

        SaveCurrentPage();
    }

    [Fact(Skip = FrontDoorConfig.SkipTest)]
    [Order(5)]
    public async Task Order05_ProvideSupportRequiredActivities()
    {
        await TestQuestionPage(
            ProjectPagesUrl.SupportRequiredActivities(UserOrganisationData.OrganisationId, ProjectData.Id),
            ProjectPageTitles.SupportRequiredActivities,
            ProjectPagesUrl.Tenure(UserOrganisationData.OrganisationId, ProjectData.Id),
            (nameof(ProjectDetails.SupportActivityTypes), ProjectData.ActivityType.ToString()));
    }

    [Fact(Skip = FrontDoorConfig.SkipTest)]
    [Order(6)]
    public async Task Order06_ProvideTenure()
    {
        await TestQuestionPage(
            ProjectPagesUrl.Tenure(UserOrganisationData.OrganisationId, ProjectData.Id),
            ProjectPageTitles.Tenure,
            ProjectPagesUrl.OrganisationHomesBuilt(UserOrganisationData.OrganisationId, ProjectData.Id),
            (nameof(ProjectDetails.AffordableHomesAmount), ProjectData.AffordableHomeAmount.ToString()));
    }

    [Fact(Skip = FrontDoorConfig.SkipTest)]
    [Order(7)]
    public async Task Order07_ProvideOrganisationHomesBuilt()
    {
        await TestQuestionPage(
            ProjectPagesUrl.OrganisationHomesBuilt(UserOrganisationData.OrganisationId, ProjectData.Id),
            ProjectPageTitles.OrganisationHomesBuilt,
            ProjectPagesUrl.IdentifiedSite(UserOrganisationData.OrganisationId, ProjectData.Id),
            (nameof(ProjectDetails.OrganisationHomesBuilt), ProjectData.OrganisationHomesBuilt.ToString(CultureInfo.InvariantCulture)));
    }
}
