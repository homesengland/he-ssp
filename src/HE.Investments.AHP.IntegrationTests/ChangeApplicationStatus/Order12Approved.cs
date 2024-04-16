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

[Order(12)]
[SuppressMessage("xUnit", "xUnit1004", Justification = "Waits for DevOps configuration - #76791")]
public class Order12Approved : AhpIntegrationTest
{
    public Order12Approved(AhpIntegrationTestFixture fixture, ITestOutputHelper output)
        : base(fixture, output)
    {
    }

    [Fact(Skip = AhpConfig.SkipTest)]
    [Order(1)]
    public async Task Order01_ShouldDisplayReadOnlyApplication_WhenApplicationStatusIsApproved()
    {
        // given
        await ChangeApplicationStatus(ApplicationData.ApplicationId, ApplicationStatus.Approved);

        // when
        var taskListPage = await TestClient.NavigateTo(ApplicationPagesUrl.TaskList(ApplicationData.ApplicationId));

        // then
        taskListPage
            .UrlEndWith(ApplicationPagesUrl.TaskListSuffix)
            .HasTitle(ApplicationData.ApplicationName)
            .HasStatusTagByTestId(ApplicationStatus.Approved.GetDescription(), "application-status")
            .HasNoElementWithTestId("check-and-submit-application")
            .HasNoElementWithTestId("put-application-on-hold")
            .HasNoElementWithTestId("reactivate-application")
            .HasNoElementWithTestId("withdraw-application")
            .HasNoElementWithTestId("request-to-edit-application")
            .HasReturnToApplicationsListLink();
    }
}
