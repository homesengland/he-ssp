using HE.Investment.AHP.Contract.Delivery;
using HE.Investment.AHP.Contract.Delivery.Enums;
using HE.Investment.AHP.WWW.Views.Delivery.Const;
using HE.Investments.Common.WWWTestsFramework;
using HE.Investments.TestsUtils.Extensions;
using Microsoft.AspNetCore.Routing;
using EnumExtensions = HE.Investments.Common.Extensions.EnumExtensions;

namespace HE.Investment.AHP.WWW.Tests.Views.DeliveryPhase;

public class BuildActivityTypeTests : ViewTestBase
{
    private readonly string _viewPath = "/Views/DeliveryPhase/BuildActivityType.cshtml";

    private readonly RouteData _routeData = new(new RouteValueDictionary { { "applicationId", "123" }, { "deliveryPhaseId", "321" } });

    [Theory]
    [InlineData(null)]
    [InlineData(TypeOfHomes.NewBuild)]
    public async Task ShouldDisplayRadiosForNewBuild_WhenTypeOfHomesIsNotProvided(TypeOfHomes? typeOfHomes)
    {
        var model = new DeliveryPhaseDetails(
            "AppName",
            "Id",
            "DeliveryPhaseName",
            typeOfHomes,
            null,
            null,
            null,
            false,
            null,
            null,
            null,
            null,
            null,
            null,
            null);

        // given & when
        var document = await Render(_viewPath, model, routeData: _routeData);

        // then
        document.HasTitle(DeliveryPageTitles.BuildActivityType)
            .HasGdsRadioInputWithValues(
                nameof(DeliveryPhaseDetails.BuildActivityTypeForNewBuild),
                EnumExtensions.GetDefinedValues<BuildActivityTypeForNewBuild>().Select(x => x.ToString()).ToArray())
            .HasGdsSaveAndContinueButton()
            .HasGdsBackButton(false);
    }

    [Fact]
    public async Task ShouldDisplayRadiosForRehab_WhenTypeOfHomesIsRehab()
    {
        var model = new DeliveryPhaseDetails(
            "AppName",
            "Id",
            "DeliveryPhaseName",
            TypeOfHomes.Rehab,
            null,
            null,
            null,
            false,
            null,
            null,
            null,
            null,
            null,
            null,
            null);

        // given & when
        var document = await Render(_viewPath, model, routeData: _routeData);

        // then
        document.HasTitle(DeliveryPageTitles.BuildActivityType)
            .HasGdsRadioInputWithValues(
                nameof(DeliveryPhaseDetails.BuildActivityTypeForRehab),
                EnumExtensions.GetDefinedValues<BuildActivityTypeForRehab>().Select(x => x.ToString()).ToArray())
            .HasGdsSaveAndContinueButton()
            .HasGdsBackButton(false);
    }
}
