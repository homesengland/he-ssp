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

[Order(7)]
[SuppressMessage("xUnit", "xUnit1004", Justification = "Waits for DevOps configuration - #76791")]
public class Order07PutOnHold : AhpIntegrationTest
{
    public Order07PutOnHold(IntegrationTestFixture<Program> fixture)
        : base(fixture)
    {
    }

    [Fact(Skip = AhpConfig.SkipTest)]
    [Order(1)]
    public async Task Order01_ShouldNavigateToOnHoldPage_WhenApplicationIsSubmitted()
    {
        // given
        var taskListPage = await TestClient.NavigateTo(ApplicationPagesUrl.TaskList(ApplicationData.ApplicationId));
        taskListPage
            .UrlEndWith(ApplicationPagesUrl.TaskListSuffix)
            .HasTitle(ApplicationData.ApplicationName)
            .HasApplicationStatus(ApplicationStatus.ApplicationSubmitted.GetDescription())
            .HasLinkWithTestId("put-application-on-hold", out var onHoldLink);

        // when
        var onHoldPage = await TestClient.NavigateTo(onHoldLink);

        // then
        onHoldPage
            .UrlEndWith(ApplicationPagesUrl.OnHoldSuffix)
            .HasTitle(ApplicationPageTitles.OnHold)
            .HasTextAreaInput("HoldReason")
            .HasSubmitButton("Hold");

        SaveCurrentPage();
    }

    [Fact(Skip = AhpConfig.SkipTest)]
    [Order(2)]
    public async Task Order02_ShouldPutApplicationOnHold_WhenHoldReasonIsProvided()
    {
        // given
        var holdPage = await GetCurrentPage();
        var holdButton = holdPage
            .GetSubmitButton("Hold");

        // when
        var taskListPage = await TestClient.SubmitButton(holdButton, ("HoldReason", "very important reason"));

        // then
        taskListPage
            .UrlEndWith(ApplicationPagesUrl.TaskListSuffix)
            .HasTitle(ApplicationData.ApplicationName)
            .HasSuccessNotificationBanner("You’ll be able to reactivate and submit this application at any time.")
            .HasApplicationStatus(ApplicationStatus.OnHold.GetDescription());

        SaveCurrentPage();
    }
}
