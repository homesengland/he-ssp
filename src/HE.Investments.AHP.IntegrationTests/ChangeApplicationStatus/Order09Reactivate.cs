using System.Diagnostics.CodeAnalysis;
using HE.Investments.AHP.IntegrationTests.Framework;
using HE.Investments.AHP.IntegrationTests.Pages;
using HE.Investments.Common.Contract;
using HE.Investments.Common.Extensions;
using HE.Investments.TestsUtils.Extensions;
using Xunit;
using Xunit.Abstractions;
using Xunit.Extensions.Ordering;

namespace HE.Investments.AHP.IntegrationTests.ChangeApplicationStatus;

[Order(9)]
[SuppressMessage("xUnit", "xUnit1004", Justification = "Waits for DevOps configuration - #76791")]
public class Order09Reactivate : AhpIntegrationTest
{
    public Order09Reactivate(AhpIntegrationTestFixture fixture, ITestOutputHelper output)
        : base(fixture, output)
    {
    }

    [Fact(Skip = AhpConfig.SkipTest)]
    [Order(1)]
    public async Task Order01_ShouldChangeApplicationStatusToRequestedEditing_WhenApplicationIsReactivated()
    {
        // given
        var taskListPage = await TestClient.NavigateTo(ApplicationPagesUrl.TaskList(ApplicationData.ApplicationId));
        taskListPage
            .UrlEndWith(ApplicationPagesUrl.TaskListSuffix)
            .HasTitle(ApplicationData.ApplicationName)
            .HasStatusTagByTestId(ApplicationStatus.OnHold.GetDescription(), "application-status")
            .HasLinkWithTestId("reactivate-application", out var reactivateLink);

        // when
        var redirectedTaskListPage = await TestClient.NavigateTo(reactivateLink);

        // then
        redirectedTaskListPage
            .UrlEndWith(ApplicationPagesUrl.TaskListSuffix)
            .HasTitle(ApplicationData.ApplicationName)
            .HasLinkWithTestId("withdraw-application", out _)
            .HasLinkWithTestId("put-application-on-hold", out _)
            .HasStatusTagByTestId(ApplicationStatus.RequestedEditing.GetDescription(), "application-status");
    }
}
