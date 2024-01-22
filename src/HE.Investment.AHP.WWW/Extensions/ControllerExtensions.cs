using HE.Investment.AHP.Contract.Application;
using HE.Investment.AHP.Contract.Delivery;
using HE.Investments.Common.Contract.Exceptions;
using HE.Investments.Common.WWW.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace HE.Investment.AHP.WWW.Extensions;

public static class ControllerExtensions
{
    public static AhpApplicationId GetApplicationIdFromRoute(this Controller controller)
    {
        var applicationId = controller.Request.GetRouteValue("applicationId") ?? throw new NotFoundException("Missing required applicationId path parameter.");
        return AhpApplicationId.From(applicationId);
    }

    public static AhpApplicationId? TryGetApplicationIdFromRoute(this Controller controller)
    {
        var id = controller.Request.GetRouteValue("applicationId");
        var applicationId = !string.IsNullOrEmpty(id) ? AhpApplicationId.From(id) : null;

        return applicationId;
    }

    public static DeliveryPhaseId GetDeliveryPhaseIdFromRoute(this Controller controller)
    {
        var deliveryPhase = controller.Request.GetRouteValue("deliveryPhaseId") ?? throw new NotFoundException("Missing required deliveryPhaseId path parameter.");
        return new DeliveryPhaseId(deliveryPhase);
    }
}
