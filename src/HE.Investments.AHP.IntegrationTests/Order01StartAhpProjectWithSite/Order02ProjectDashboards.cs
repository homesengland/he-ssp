using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using HE.Investments.AHP.IntegrationTests.Framework;
using HE.Investments.AHP.IntegrationTests.Pages;
using HE.Investments.Common.Contract;
using HE.Investments.TestsUtils.Extensions;
using Xunit;
using Xunit.Abstractions;
using Xunit.Extensions.Ordering;

namespace HE.Investments.AHP.IntegrationTests.Order01StartAhpProjectWithSite;

[Order(102)]
[SuppressMessage("xUnit", "xUnit1004", Justification = "Waits for DevOps configuration - #76791")]
public class Order02ProjectDashboards : AhpIntegrationTest
{
    public Order02ProjectDashboards(AhpIntegrationTestFixture fixture, ITestOutputHelper output)
        : base(fixture, output)
    {
    }

    [Fact(Skip = AhpConfig.SkipTest)]
    [Order(1)]
    public async Task Order01_ShouldDisplayNewProjectOnTheTopOfProjectListPage()
    {
        // given
        var currentPage = await TestClient.NavigateTo(ProjectPagesUrl.ProjectList(UserOrganisationData.OrganisationId));
        currentPage
            .HasTitle("Affordable Homes Programme 2021-2026 Continuous Market Engagement projects")
            .UrlEndWith(ProjectPagesUrl.ProjectList(UserOrganisationData.OrganisationId));

        // when
        var projectListPage = currentPage.GetFirstListCard();

        // then
        projectListPage.Title.Should().Be(ProjectData.ProjectName, "New project should be displayed on the top of the list");
        projectListPage.Action.Should().NotBeNull();
        projectListPage.Action!.Title.Should().Be("View project");
        projectListPage.Action!.Url.Should()
            .EndWith(ProjectPagesUrl.ProjectDetails(UserOrganisationData.OrganisationId, ShortGuid.FromString(ProjectData.ProjectId).Value));

        SaveCurrentPage();
    }
}
