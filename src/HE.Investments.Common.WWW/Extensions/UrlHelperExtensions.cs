using Microsoft.AspNetCore.Mvc;

namespace HE.Investments.Common.WWW.Extensions;

public static class UrlHelperExtensions
{
    public static string? OrganisationAction(this IUrlHelper urlHelper, string? action, string? controller, object? routeValues = null)
    {
        var dynamicRouteData = routeValues.ExpandRouteValues(new { organisationId = urlHelper.ActionContext.HttpContext.GetOrganisationIdFromRoute() });

        return urlHelper.Action(action, controller, dynamicRouteData);
    }
}
