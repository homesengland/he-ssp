using System.Diagnostics.CodeAnalysis;
using HE.Investment.AHP.WWW;
using HE.Investment.AHP.WWW.Views.Application;
using HE.Investments.AHP.IntegrationTests.Framework;
using HE.Investments.AHP.IntegrationTests.Pages;
using HE.Investments.Common.Contract;
using HE.Investments.Common.Extensions;
using HE.Investments.IntegrationTestsFramework;
using HE.Investments.TestsUtils.Extensions;
using Xunit;
using Xunit.Extensions.Ordering;

namespace HE.Investments.AHP.IntegrationTests.ChangeApplicationStatus;

[Order(9)]
[SuppressMessage("xUnit", "xUnit1004", Justification = "Waits for DevOps configuration - #76791")]
public class Order09Withdraw : AhpIntegrationTest
{
    public Order09Withdraw(IntegrationTestFixture<Program> fixture)
        : base(fixture)
    {
    }

    [Fact(Skip = AhpConfig.SkipTest)]
    [Order(1)]
    public async Task Order01_ShouldNavigateToWithdrawPage_WhenApplicationIsOnHold()
    {
        // given
        var taskListPage = await TestClient.NavigateTo(ApplicationPagesUrl.TaskList(ApplicationData.ApplicationId));
        taskListPage
            .UrlEndWith(ApplicationPagesUrl.TaskListSuffix)
            .HasTitle(ApplicationData.ApplicationName)
            .HasStatusTagByTestId(ApplicationStatus.OnHold.GetDescription(), "application-status")
            .HasLinkWithTestId("withdraw-application", out var withdrawLink);

        // when
        var withdrawPage = await TestClient.NavigateTo(withdrawLink);

        // then
        withdrawPage
            .UrlEndWith(ApplicationPagesUrl.WithdrawSuffix)
            .HasTitle(ApplicationPageTitles.Withdraw)
            .HasTextAreaInput("WithdrawReason")
            .HasSubmitButton(out _, "Withdraw application");

        SaveCurrentPage();
    }

    [Fact(Skip = AhpConfig.SkipTest)]
    [Order(2)]
    public async Task Order02_ShouldWithdrawApplication_WhenWithdrawReasonIsProvided()
    {
        // given
        var withdrawPage = await GetCurrentPage();
        var withdrawApplicationButton = withdrawPage
            .GetSubmitButton("Withdraw application");

        // when
        var taskListPage = await TestClient.SubmitButton(withdrawApplicationButton, ("WithdrawReason", "even more important reason"));

        // then
        taskListPage
            .UrlEndWith(ApplicationPagesUrl.TaskListSuffix)
            .HasTitle(ApplicationData.ApplicationName)
            .HasSuccessNotificationBanner("You will no longer be able proceed with this application.")
            .HasStatusTagByTestId(ApplicationStatus.Withdrawn.GetDescription(), "application-status");

        SaveCurrentPage();
    }
}
