using HE.Investments.Common.Extensions;
using Microsoft.AspNetCore.Mvc.Razor;

namespace HE.Investment.AHP.WWW.Extensions;

public static class RazorPageExtensions
{
    public static string GetSiteIdFromRoute(this IRazorPage razorPage)
    {
        return razorPage.GetOptionalFromRoute("siteId")
               ?? throw new ArgumentException("Site Id is not present in Route data");
    }

    public static string GetApplicationIdFromRoute(this IRazorPage razorPage)
    {
        return razorPage.GetOptionalFromRoute("applicationId")
               ?? throw new ArgumentException("Application Id is not present in Route data");
    }

    public static string GetHomeTypeIdFromRoute(this IRazorPage razorPage)
    {
        return razorPage.GetOptionalFromRoute("homeTypeId")
               ?? throw new ArgumentException("Home Type Id is not present in Route data");
    }

    public static string? GetOptionalHomeTypeIdFromRoute(this IRazorPage razorPage)
    {
        return razorPage.GetOptionalFromRoute("homeTypeId");
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
