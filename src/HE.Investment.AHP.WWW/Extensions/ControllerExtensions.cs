using HE.Investment.AHP.Contract.Application;
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

    public static string GetDeliveryPhaseIdFromRoute(this Controller controller)
    {
        return controller.Request.GetRouteValue("deliveryPhaseId") ?? throw new NotFoundException("Missing required deliveryPhaseId path parameter.");
    }
}
