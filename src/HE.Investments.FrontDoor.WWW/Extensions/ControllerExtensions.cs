using HE.Investments.Common.Contract.Exceptions;
using HE.Investments.Common.WWW.Extensions;
using HE.Investments.FrontDoor.Contract.Site;
using HE.Investments.FrontDoor.Shared.Project;
using Microsoft.AspNetCore.Mvc;

namespace HE.Investments.FrontDoor.WWW.Extensions;

public static class ControllerExtensions
{
    public static FrontDoorProjectId GetProjectIdFromRoute(this Controller controller)
    {
        var projectId = controller.Request.GetRouteValue("projectId") ?? throw new NotFoundException("Missing required projectId path parameter.");
        return FrontDoorProjectId.From(projectId);
    }

    public static FrontDoorSiteId GetSiteIdFromRoute(this Controller controller)
    {
        var siteId = controller.Request.GetRouteValue("siteId") ?? throw new NotFoundException("Missing required siteId path parameter.");
        return FrontDoorSiteId.From(siteId);
    }

    public static string? GetOptionalParameterFromRoute(this Controller controller)
    {
        return controller.Request.Query["optional"];
    }
}
