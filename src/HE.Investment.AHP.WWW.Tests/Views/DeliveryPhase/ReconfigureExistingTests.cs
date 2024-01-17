using HE.Investment.AHP.Contract.Delivery;
using HE.Investment.AHP.Contract.Delivery.Enums;
using HE.Investment.AHP.WWW.Tests.Workflows.DeliveryPhaseWorkflowTests;
using HE.Investment.AHP.WWW.Views.Delivery.Const;
using HE.Investments.Common.WWWTestsFramework;
using HE.Investments.TestsUtils.Extensions;
using Microsoft.AspNetCore.Routing;

namespace HE.Investment.AHP.WWW.Tests.Views.DeliveryPhase;

public class ReconfigureExistingTests : ViewTestBase
{
    private readonly string _viewPath = "/Views/DeliveryPhase/ReconfiguringExisting.cshtml";

    private readonly RouteData _routeData = new(new RouteValueDictionary { { "applicationId", "123" }, { "deliveryPhaseId", "321" } });

    [Fact]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Blocker Code Smell", "S2699:Tests should include assertions", Justification = "Assertion present")]
    public async Task ShouldDisplayView()
    {
        var model = DeliveryPhaseDetailsTestData.WithNames with { IsReconfiguringExistingNeeded = true };

        // given & when
        var document = await Render(_viewPath, model, routeData: _routeData);

        // then
        document.HasTitle(DeliveryPageTitles.ReconfiguringExisting)
            .HasGdsRadioInputWithValues(nameof(DeliveryPhaseDetails.ReconfiguringExisting), "True", "False")
            .HasGdsSaveAndContinueButton()
            .HasGdsBackButton(false);
    }
}
