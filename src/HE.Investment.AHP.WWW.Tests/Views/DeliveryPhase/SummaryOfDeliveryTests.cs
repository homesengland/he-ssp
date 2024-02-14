using FluentAssertions;
using HE.Investment.AHP.Contract.Delivery;
using HE.Investment.AHP.Contract.Delivery.MilestonePayments;
using HE.Investment.AHP.WWW.Tests.Workflows.DeliveryPhaseWorkflowTests;
using HE.Investment.AHP.WWW.Views.Delivery.Const;
using Microsoft.AspNetCore.Routing;

namespace HE.Investment.AHP.WWW.Tests.Views.DeliveryPhase;

public class SummaryOfDeliveryTests : ViewTestBase
{
    private readonly string _viewPath = "/Views/DeliveryPhase/SummaryOfDelivery.cshtml";

    private readonly RouteData _routeData = new(new RouteValueDictionary { { "applicationId", "123" }, { "deliveryPhaseId", "321" } });

    [Fact]
    public async Task ShouldDisplayAllValues()
    {
        // given
        var model = DeliveryPhaseDetailsTestData.WithNames with { Tranches = new DeliveryPhaseTranchesDto(false, new SummaryOfDelivery(9000.12m, 200, 0.1m, 300, 0.1m, 4000.12m, 0.1m), null) };

        // when
        var document = await Render(_viewPath, model, routeData: _routeData);

        // then
        document.HasTitle(DeliveryPageTitles.SummaryOfDelivery("Test delivery phase"))
            .HasContinueButton()
            .HasBackLink(false);

        document.GetElementByTestId("grant-apportioned-value").TextContent.Should().Contain("\u00a39,000.12");
        document.GetElementByTestId("acquisition-value").TextContent.Should().Contain("\u00a3200");
        document.GetElementByTestId("start-on-site-value").TextContent.Should().Contain("\u00a3300");
        document.GetElementByTestId("completion-value").TextContent.Should().Contain("\u00a34,000.12");
    }

    [Theory]
    [InlineData(true, false)]
    [InlineData(false, true)]
    [InlineData(true, true)]
    public async Task ShouldDisplayOnlyCompletionMilestoneValue(bool isUnregisteredBody, bool isOnlyCompletionMilestone)
    {
        // given
        var model = DeliveryPhaseDetailsTestData.WithNames with
        {
            Tranches = new DeliveryPhaseTranchesDto(false, new SummaryOfDelivery(9000.12m, 200, 0.25m, 300, 0.25m, 4000.12m, 0.25m), null),
            IsUnregisteredBody = isUnregisteredBody,
            IsOnlyCompletionMilestone = isOnlyCompletionMilestone,
        };

        // when
        var document = await Render(_viewPath, model, routeData: _routeData);

        // then
        document.HasTitle(DeliveryPageTitles.SummaryOfDelivery("Test delivery phase"))
            .HasContinueButton()
            .HasBackLink(false);

        document.GetElementByTestId("grant-apportioned-value").TextContent.Should().Contain("\u00a39,000.12");
        document.GetElementByTestId("completion-value").TextContent.Should().Contain("\u00a34,000.12");
    }
}
