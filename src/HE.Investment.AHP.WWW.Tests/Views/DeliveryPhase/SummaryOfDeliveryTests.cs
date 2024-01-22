using FluentAssertions;
using HE.Investment.AHP.Contract.Delivery;
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
        var model = DeliveryPhaseDetailsTestData.WithNames with { SummaryOfDelivery = new SummaryOfDelivery(9000.12m, 200, 300, 4000.12m) };

        // when
        var document = await Render(_viewPath, model, routeData: _routeData);

        // then
        document.HasTitle(DeliveryPageTitles.SummaryOfDelivery)
            .HasGdsContinueButton()
            .HasGdsBackButton(false);

        var summary = document.GetSummaryListItems();
        summary.Should().HaveCount(4);
        summary.Keys.ElementAt(0).Should().Contain("\u00a39,000.12");
        summary.Keys.ElementAt(1).Should().Contain("\u00a3200");
        summary.Keys.ElementAt(2).Should().Contain("\u00a3300");
        summary.Keys.ElementAt(3).Should().Contain("\u00a34,000.12");
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
            SummaryOfDelivery = new SummaryOfDelivery(9000.12m, 200, 300, 4000.12m),
            IsUnregisteredBody = isUnregisteredBody,
            IsOnlyCompletionMilestone = isOnlyCompletionMilestone,
        };

        // when
        var document = await Render(_viewPath, model, routeData: _routeData);

        // then
        document.HasTitle(DeliveryPageTitles.SummaryOfDelivery)
            .HasGdsContinueButton()
            .HasGdsBackButton(false);

        var summary = document.GetSummaryListItems();
        summary.Should().HaveCount(2);
        summary.Keys.ElementAt(0).Should().Contain("\u00a39,000.12");
        summary.Keys.ElementAt(1).Should().Contain("\u00a34,000.12");
    }
}
