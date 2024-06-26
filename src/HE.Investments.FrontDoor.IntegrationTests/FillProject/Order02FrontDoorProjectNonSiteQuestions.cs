using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using FluentAssertions;
using HE.Investments.Common.Extensions;
using HE.Investments.Common.WWW.Extensions;
using HE.Investments.FrontDoor.Contract.Project;
using HE.Investments.FrontDoor.IntegrationTests.Framework;
using HE.Investments.FrontDoor.IntegrationTests.Pages;
using HE.Investments.FrontDoor.WWW.Views.Project.Const;
using HE.Investments.IntegrationTestsFramework.Assertions;
using HE.Investments.TestsUtils.Extensions;
using Xunit;
using Xunit.Abstractions;
using Xunit.Extensions.Ordering;

namespace HE.Investments.FrontDoor.IntegrationTests.FillProject;

[Order(2)]
[SuppressMessage("xUnit", "xUnit1004", Justification = "Waits for DevOps configuration - #76791")]
public class Order02FrontDoorProjectNonSiteQuestions : FrontDoorIntegrationTest
{
    public Order02FrontDoorProjectNonSiteQuestions(FrontDoorIntegrationTestFixture fixture, ITestOutputHelper output)
        : base(fixture, output)
    {
    }

    [Fact(Skip = FrontDoorConfig.SkipTest)]
    [Order(1)]
    public async Task Order01_ProvideIdentifiedSite()
    {
        await TestQuestionPage(
            ProjectPagesUrl.IdentifiedSite(UserOrganisationData.OrganisationId, ProjectData.Id),
            ProjectPageTitles.IdentifiedSite,
            ProjectPagesUrl.GeographicFocus(UserOrganisationData.OrganisationId, ProjectData.Id),
            (nameof(ProjectDetails.IsSiteIdentified), ProjectData.IsSiteIdentified.MapToTrueFalse()));
    }

    [Fact(Skip = FrontDoorConfig.SkipTest)]
    [Order(2)]
    public async Task Order02_ProvideGeographicFocus()
    {
        await TestQuestionPage(
            ProjectPagesUrl.GeographicFocus(UserOrganisationData.OrganisationId, ProjectData.Id),
            ProjectPageTitles.GeographicFocus,
            ProjectPagesUrl.Region(UserOrganisationData.OrganisationId, ProjectData.Id),
            (nameof(ProjectDetails.GeographicFocus), ProjectData.GeographicFocus.ToString()));
    }

    [Fact(Skip = FrontDoorConfig.SkipTest)]
    [Order(3)]
    public async Task Order03_ProvideRegion()
    {
        await TestQuestionPage(
            ProjectPagesUrl.Region(UserOrganisationData.OrganisationId, ProjectData.Id),
            ProjectPageTitles.Region,
            ProjectPagesUrl.HomesNumber(UserOrganisationData.OrganisationId, ProjectData.Id),
            ProjectData.RegionTypes.Select(x => (nameof(ProjectDetails.Regions), x.ToString())).ToArray());
    }

    [Fact(Skip = FrontDoorConfig.SkipTest)]
    [Order(4)]
    public async Task Order04_ProvideHomesNumber()
    {
        await TestQuestionPage(
            ProjectPagesUrl.HomesNumber(UserOrganisationData.OrganisationId, ProjectData.Id),
            ProjectPageTitles.HomesNumber,
            ProjectPagesUrl.Progress(UserOrganisationData.OrganisationId, ProjectData.Id),
            (nameof(ProjectDetails.HomesNumber), ProjectData.HomesNumber.ToString(CultureInfo.InvariantCulture)));
    }

    [Fact(Skip = FrontDoorConfig.SkipTest)]
    [Order(5)]
    public async Task Order05_ProvideProgress()
    {
        await TestQuestionPage(
            ProjectPagesUrl.Progress(UserOrganisationData.OrganisationId, ProjectData.Id),
            ProjectPageTitles.Progress,
            ProjectPagesUrl.RequiresFunding(UserOrganisationData.OrganisationId, ProjectData.Id),
            (nameof(ProjectDetails.IsSupportRequired), ProjectData.IsSupportRequired.MapToTrueFalse()));
    }

    [Fact(Skip = FrontDoorConfig.SkipTest)]
    [Order(6)]
    public async Task Order06_ProvideRequiresFunding()
    {
        await TestQuestionPage(
            ProjectPagesUrl.RequiresFunding(UserOrganisationData.OrganisationId, ProjectData.Id),
            ProjectPageTitles.RequiresFunding,
            ProjectPagesUrl.ExpectedStart(UserOrganisationData.OrganisationId, ProjectData.Id),
            (nameof(ProjectDetails.IsFundingRequired), ProjectData.IsFundingRequired.MapToTrueFalse()));
    }

    [Fact(Skip = FrontDoorConfig.SkipTest)]
    [Order(7)]
    public async Task Order07_ProvideExpectedStart()
    {
        await TestQuestionPage(
            ProjectPagesUrl.ExpectedStart(UserOrganisationData.OrganisationId, ProjectData.Id),
            ProjectPageTitles.ExpectedStart,
            ProjectPagesUrl.CheckAnswers(UserOrganisationData.OrganisationId, ProjectData.Id),
            ("ExpectedStartDate.Month", ProjectData.ExpectedStartDate.Month.ToString(CultureInfo.InvariantCulture)),
            ("ExpectedStartDate.Year", ProjectData.ExpectedStartDate.Year.ToString(CultureInfo.InvariantCulture)));
    }

    [Fact(Skip = FrontDoorConfig.SkipTest)]
    [Order(8)]
    public async Task Order08_CheckAnswers()
    {
        // given
        var checkAnswersPage = await GetCurrentPage(ProjectPagesUrl.CheckAnswers(UserOrganisationData.OrganisationId, ProjectData.Id));
        checkAnswersPage
            .UrlEndWith(ProjectPagesUrl.CheckAnswers(UserOrganisationData.OrganisationId, ProjectData.Id))
            .HasTitle(ProjectPageTitles.CheckAnswers)
            .HasBackLink(out _);

        // when & then
        var summary = checkAnswersPage.GetSummaryListItems();
        summary.Should().ContainKey("Project in England").WithValue(ProjectData.IsEnglandHousingDelivery.MapToCommonResponse());
        summary.Should().ContainKey("Project name").WithValue(ProjectData.Name);
        summary.Should().ContainKey("Activities you require support for").WithValue(ProjectData.ActivityType.GetDescription());
        summary.Should().ContainKey("Amount of affordable homes").WithValue(ProjectData.AffordableHomeAmount.GetDescription());
        summary.Should().ContainKey("Previous residential building experience").WithValue(ProjectData.OrganisationHomesBuilt.ToString(CultureInfo.InvariantCulture));
        summary.Should().ContainKey("Identified site").WithValue(ProjectData.IsSiteIdentified.MapToCommonResponse());
        summary.Should().ContainKey("Geographic focus").WithValue(ProjectData.GeographicFocus.GetDescription());
        summary.Should().ContainKey("Region").WithValue(string.Join(Environment.NewLine, ProjectData.RegionTypes.Select(x => x.GetDescription())) + Environment.NewLine);
        summary.Should().ContainKey("Homes your project enables").WithValue(ProjectData.HomesNumber.ToString(CultureInfo.InvariantCulture));
        summary.Should().ContainKey("Project progress more slowly or stall").WithValue(ProjectData.IsSupportRequired.MapToCommonResponse());
        summary.Should().ContainKey("Funding required").WithValue(ProjectData.IsFundingRequired.MapToCommonResponse());
        summary.Should().ContainKey("Expected project start date").WithValue($"{CultureInfo.InvariantCulture.DateTimeFormat.GetMonthName(ProjectData.ExpectedStartDate.Month)} {ProjectData.ExpectedStartDate.Year}");
    }
}
