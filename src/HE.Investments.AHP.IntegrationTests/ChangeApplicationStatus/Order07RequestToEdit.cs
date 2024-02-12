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
public class Order07RequestToEdit : AhpIntegrationTest
{
    public Order07RequestToEdit(IntegrationTestFixture<Program> fixture)
        : base(fixture)
    {
    }

    [Fact(Skip = AhpConfig.SkipTest)]
    [Order(1)]
    public async Task Order01_ShouldNavigateToRequestToEditPage_WhenApplicationIsSubmitted()
    {
        // given
        var taskListPage = await TestClient.NavigateTo(ApplicationPagesUrl.TaskList(ApplicationData.ApplicationId));
        taskListPage
            .UrlEndWith(ApplicationPagesUrl.TaskListSuffix)
            .HasTitle(ApplicationData.ApplicationName)
            .HasStatusTagByTestId(ApplicationStatus.ApplicationSubmitted.GetDescription(), "application-status")
            .HasLinkWithTestId("request-to-edit-application", out var requestToEditLink);

        // when
        var requestToEditPage = await TestClient.NavigateTo(requestToEditLink);

        // then
        requestToEditPage
            .UrlEndWith(ApplicationPagesUrl.RequestToEditSuffix)
            .HasTitle(ApplicationPageTitles.RequestToEdit)
            .HasTextAreaInput("RequestToEditReason")
            .HasSubmitButton(out _, "Request to edit");

        SaveCurrentPage();
    }

    [Fact(Skip = AhpConfig.SkipTest)]
    [Order(2)]
    public async Task Order02_ShouldChangeApplicationStatusToRequestedEditing_WhenRequestToEditReasonIsProvided()
    {
        // given
        var requestToEditPage = await GetCurrentPage();
        var requestToEditButton = requestToEditPage
            .GetSubmitButton("Request to edit");

        // when
        var taskListPage = await TestClient.SubmitButton(requestToEditButton, ("RequestToEditReason", "very important reason"));

        // then
        taskListPage
            .UrlEndWith(ApplicationPagesUrl.TaskListSuffix)
            .HasTitle(ApplicationData.ApplicationName)
            .HasSuccessNotificationBanner("You’ll be notified once your Growth Manager has referred your application back to you.")
            .HasStatusTagByTestId(ApplicationStatus.RequestedEditing.GetDescription(), "application-status");

        SaveCurrentPage();
    }
}
