using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using AngleSharp.Html.Dom;
using FluentAssertions;
using HE.Investments.Common.Extensions;
using HE.Investments.Common.WWW.Extensions;
using HE.Investments.FrontDoor.Contract.Project;
using HE.Investments.FrontDoor.IntegrationTests.Framework;
using HE.Investments.FrontDoor.IntegrationTests.Pages;
using HE.Investments.FrontDoor.Shared.Project.Contract;
using HE.Investments.FrontDoor.WWW;
using HE.Investments.FrontDoor.WWW.Views.Project.Const;
using HE.Investments.IntegrationTestsFramework;
using HE.Investments.TestsUtils.Assertions;
using HE.Investments.TestsUtils.Extensions;
using Xunit;
using Xunit.Extensions.Ordering;

namespace HE.Investments.FrontDoor.IntegrationTests.FillProject;

[Order(2)]
[SuppressMessage("xUnit", "xUnit1004", Justification = "Waits for DevOps configuration - #76791")]
public class Order02FrontDoorProjectNonSiteQuestions : FrontDoorIntegrationTest
{
    public Order02FrontDoorProjectNonSiteQuestions(IntegrationTestFixture<Program> fixture)
        : base(fixture)
    {
    }

    [Fact(Skip = FrontDoorConfig.SkipTest)]
    [Order(1)]
    public async Task Order01_ProvideIdentifiedSite()
    {
        await TestQuestionPage(
            ProjectPagesUrl.IdentifiedSite(ProjectData.Id),
            ProjectPageTitles.IdentifiedSite,
            ProjectPagesUrl.GeographicFocus(ProjectData.Id),
            (nameof(ProjectDetails.IsSiteIdentified), ProjectData.IsSiteIdentified.MapToTrueFalse()));
    }

    [Fact(Skip = FrontDoorConfig.SkipTest)]
    [Order(2)]
    public async Task Order02_ProvideGeographicFocus()
    {
        await TestQuestionPage(
            ProjectPagesUrl.GeographicFocus(ProjectData.Id),
            ProjectPageTitles.GeographicFocus,
            ProjectPagesUrl.Region(ProjectData.Id),
            (nameof(ProjectDetails.GeographicFocus), ProjectData.GeographicFocus.ToString()));
    }

    [Fact(Skip = FrontDoorConfig.SkipTest)]
    [Order(3)]
    public async Task Order03_ProvideRegion()
    {
        await TestQuestionPage(
            ProjectPagesUrl.Region(ProjectData.Id),
            ProjectPageTitles.Region,
            ProjectPagesUrl.HomesNumber(ProjectData.Id),
            ProjectData.RegionTypes.Select(x => (nameof(ProjectDetails.Regions), x.ToString())).ToArray());
    }

    [Fact(Skip = FrontDoorConfig.SkipTest)]
    [Order(4)]
    public async Task Order04_ProvideHomesNumber()
    {
        await TestQuestionPage(
            ProjectPagesUrl.HomesNumber(ProjectData.Id),
            ProjectPageTitles.HomesNumber,
            ProjectPagesUrl.Progress(ProjectData.Id),
            (nameof(ProjectDetails.HomesNumber), ProjectData.HomesNumber.ToString(CultureInfo.InvariantCulture)));
    }

    [Fact(Skip = FrontDoorConfig.SkipTest)]
    [Order(5)]
    public async Task Order05_ProvideProgress()
    {
        await TestQuestionPage(
            ProjectPagesUrl.Progress(ProjectData.Id),
            ProjectPageTitles.Progress,
            ProjectPagesUrl.RequiresFunding(ProjectData.Id),
            (nameof(ProjectDetails.IsSupportRequired), ProjectData.IsSupportRequired.MapToTrueFalse()));
    }

    [Fact(Skip = FrontDoorConfig.SkipTest)]
    [Order(6)]
    public async Task Order06_ProvideRequiresFunding()
    {
        await TestQuestionPage(
            ProjectPagesUrl.RequiresFunding(ProjectData.Id),
            ProjectPageTitles.RequiresFunding,
            ProjectPagesUrl.FundingAmount(ProjectData.Id),
            (nameof(ProjectDetails.IsFundingRequired), ProjectData.IsFundingRequired.MapToTrueFalse()));
    }

    [Fact(Skip = FrontDoorConfig.SkipTest)]
    [Order(7)]
    public async Task Order07_ProvideFundingAmount()
    {
        await TestQuestionPage(
            ProjectPagesUrl.FundingAmount(ProjectData.Id),
            ProjectPageTitles.FundingAmount,
            ProjectPagesUrl.Profit(ProjectData.Id),
            (nameof(ProjectDetails.RequiredFunding), ProjectData.RequiredFunding.ToString()));
    }

    [Fact(Skip = FrontDoorConfig.SkipTest)]
    [Order(8)]
    public async Task Order08_ProvideProfit()
    {
        await TestQuestionPage(
            ProjectPagesUrl.Profit(ProjectData.Id),
            ProjectPageTitles.Profit,
            ProjectPagesUrl.ExpectedStart(ProjectData.Id),
            (nameof(ProjectDetails.IsProfit), ProjectData.IsProfit.MapToTrueFalse()));
    }

    [Fact(Skip = FrontDoorConfig.SkipTest)]
    [Order(9)]
    public async Task Order09_ProvideExpectedStart()
    {
        await TestQuestionPage(
            ProjectPagesUrl.ExpectedStart(ProjectData.Id),
            ProjectPageTitles.ExpectedStart,
            ProjectPagesUrl.CheckAnswers(ProjectData.Id),
            ("ExpectedStartDate.Month", ProjectData.ExpectedStartDate.Month.ToString(CultureInfo.InvariantCulture)),
            ("ExpectedStartDate.Year", ProjectData.ExpectedStartDate.Year.ToString(CultureInfo.InvariantCulture)));
    }
}
