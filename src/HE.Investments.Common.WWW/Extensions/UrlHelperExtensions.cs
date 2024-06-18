using System.Reflection;
using Microsoft.AspNetCore.Mvc;

namespace HE.Investments.Common.WWW.Extensions;

public static class UrlHelperExtensions
{
    public static string? OrganisationAction(this IUrlHelper urlHelper, string? action, string? controller, object? routeValues = null)
    {
        var dynamicRouteData = new System.Dynamic.ExpandoObject() as IDictionary<string, object>;
        if (routeValues != null)
        {
            foreach (var property in routeValues.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public))
            {
                dynamicRouteData.Add(property.Name, property.GetValue(routeValues)!);
            }
        }

        if (!dynamicRouteData.ContainsKey("organisationId"))
        {
            var organisationId = urlHelper.ActionContext.RouteData.Values["organisationId"] as string;
            dynamicRouteData.Add("organisationId", organisationId!);
        }

        return urlHelper.Action(action, controller, dynamicRouteData);
    }
}
