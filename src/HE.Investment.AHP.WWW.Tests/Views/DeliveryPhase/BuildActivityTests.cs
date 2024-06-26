using HE.Investment.AHP.Contract.Delivery;
using HE.Investment.AHP.Contract.Delivery.Enums;
using HE.Investment.AHP.WWW.Tests.TestDataBuilders;
using HE.Investment.AHP.WWW.Views.Delivery.Const;
using HE.Investments.Common.Contract;
using Microsoft.AspNetCore.Routing;

namespace HE.Investment.AHP.WWW.Tests.Views.DeliveryPhase;

public class BuildActivityTests : AhpViewTestBase
{
    private readonly string _viewPath = "/Views/DeliveryPhase/BuildActivityType.cshtml";

    private readonly RouteData _routeData = new(new RouteValueDictionary { { "applicationId", "123" }, { "deliveryPhaseId", "321" } });

    [Fact]
    public async Task ShouldDisplayRadiosForRehab_WhenTypeOfHomesIsRehab()
    {
        var availableTypes = new List<BuildActivityType>() { BuildActivityType.WorksOnly, BuildActivityType.AcquisitionAndWorksRehab };
        var model = new DeliveryPhaseDetails(
            ApplicationTestData.SampleApplication,
            "Id",
            "DeliveryPhaseName",
            SectionStatus.InProgress,
            null,
            null,
            availableTypes);

        // given & when
        var document = await Render(_viewPath, model, routeData: _routeData);

        // then
        document.HasTitle(DeliveryPageTitles.BuildActivityType)
            .HasGdsRadioInputWithValues(
                nameof(DeliveryPhaseDetails.BuildActivityType),
                availableTypes.Select(x => x.ToString()).ToArray())
            .HasSaveAndContinueButton()
            .HasBackLink(out _, false);
    }
}
