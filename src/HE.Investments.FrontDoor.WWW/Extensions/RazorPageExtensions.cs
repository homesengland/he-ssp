using HE.Investments.Common.Extensions;
using Microsoft.AspNetCore.Mvc.Razor;

namespace HE.Investments.FrontDoor.WWW.Extensions;

public static class RazorPageExtensions
{
    public static string GetProjectIdFromRoute(this IRazorPage razorPage)
    {
        return razorPage.GetOptionalProjectIdFromRoute() ?? throw new ArgumentException("Project Id is not present in Route data");
    }

    public static string? GetOptionalProjectIdFromRoute(this IRazorPage razorPage)
    {
        return razorPage.GetOptionalFromRoute("projectId");
    }

    public static string? GetOptionalSiteIdFromRoute(this IRazorPage razorPage)
    {
        return razorPage.GetOptionalFromRoute("siteId");
    }

    private static string? GetOptionalFromRoute(this IRazorPage razorPage, string parameterName)
    {
        var parameterValue = razorPage.ViewContext.RouteData.Values[parameterName] as string;
        if (parameterValue.IsNotProvided())
        {
            return null;
        }

        return parameterValue!;
    }
}
