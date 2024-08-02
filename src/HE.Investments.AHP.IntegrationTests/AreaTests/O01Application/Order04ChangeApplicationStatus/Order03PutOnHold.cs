using System.Diagnostics.CodeAnalysis;
using HE.Investment.AHP.WWW.Views.Application;
using HE.Investments.AHP.IntegrationTests.AreaTests.O01Application.Pages;
using HE.Investments.AHP.IntegrationTests.Framework;
using HE.Investments.Common.Contract;
using HE.Investments.Common.Extensions;
using HE.Investments.TestsUtils.Extensions;
using Xunit;
using Xunit.Abstractions;
using Xunit.Extensions.Ordering;

namespace HE.Investments.AHP.IntegrationTests.AreaTests.O01Application.Order04ChangeApplicationStatus;

[Order(403)]
[SuppressMessage("xUnit", "xUnit1004", Justification = "Waits for DevOps configuration - #76791")]
public class Order03PutOnHold : AhpIntegrationTest
{
    public Order03PutOnHold(AhpIntegrationTestFixture fixture, ITestOutputHelper output)
        : base(fixture, output)
    {
    }

    [Fact(Skip = AhpConfig.SkipTest)]
    [Order(1)]
    public async Task Order01_ShouldNavigateToOnHoldPage_WhenApplicationHasStatusRequestedEditing()
    {
        // given
        var taskListPage = await TestClient.NavigateTo(ApplicationPagesUrl.TaskList(UserOrganisationData.OrganisationId, ApplicationData.ApplicationId));
        taskListPage
            .UrlEndWith(ApplicationPagesUrl.TaskListSuffix)
            .HasTitle(ApplicationData.ApplicationName)
            .HasStatusTagByTestId(ApplicationStatus.RequestedEditing.GetDescription(), "application-status")
            .HasLinkWithTestId("put-application-on-hold", out var onHoldLink);

        // when
        var onHoldPage = await TestClient.NavigateTo(onHoldLink);

        // then
        onHoldPage
            .UrlEndWith(ApplicationPagesUrl.OnHoldSuffix)
            .HasTitle(ApplicationPageTitles.OnHold)
            .HasTextAreaInput("HoldReason")
            .HasSubmitButton(out _, "Put on hold");

        SaveCurrentPage();
    }

    [Fact(Skip = AhpConfig.SkipTest)]
    [Order(2)]
    public async Task Order02_ShouldPutApplicationOnHold_WhenHoldReasonIsProvided()
    {
        // given
        var holdPage = await GetCurrentPage();
        var holdButton = holdPage
            .GetSubmitButton("Put on hold");

        // when
        var taskListPage = await TestClient.SubmitButton(holdButton, ("HoldReason", "very important reason"));

        // then
        taskListPage
            .UrlEndWith(ApplicationPagesUrl.TaskListSuffix)
            .HasTitle(ApplicationData.ApplicationName)
            .HasSuccessNotificationBanner("You’ll be able to reactivate and submit this application at any time.")
            .HasStatusTagByTestId(ApplicationStatus.OnHold.GetDescription(), "application-status");

        SaveCurrentPage();
    }
}
