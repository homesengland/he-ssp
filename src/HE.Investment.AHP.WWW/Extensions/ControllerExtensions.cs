using HE.Investments.Common.Contract.Exceptions;
using HE.Investments.Common.WWW.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace HE.Investment.AHP.WWW.Extensions;

public static class ControllerExtensions
{
    public static string GetApplicationIdFromRoute(this Controller controller)
    {
        return controller.Request.GetRouteValue("applicationId") ?? throw new NotFoundException("Missing required applicationId path parameter.");
    }

    public static string GetDeliveryPhaseIdFromRoute(this Controller controller)
    {
        return controller.Request.GetRouteValue("deliveryPhaseId") ?? throw new NotFoundException("Missing required deliveryPhaseId path parameter.");
    }
}
