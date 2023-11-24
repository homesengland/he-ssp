using HE.Investments.Common.Extensions;
using Microsoft.AspNetCore.Mvc.Razor;

namespace HE.Investment.AHP.WWW.Extensions;

public static class RazorPageExtensions
{
    public static string GetApplicationIdFromRoute(this IRazorPage razorPage)
    {
        var applicationId = razorPage.ViewContext.RouteData.Values["applicationId"] as string;
        if (applicationId.IsNotProvided())
        {
            throw new ArgumentException("Application Id is not present in Route data");
        }

        return applicationId!;
    }

    public static string GetHomeTypeIdFromRoute(this IRazorPage razorPage)
    {
        var homeTypeId = razorPage.ViewContext.RouteData.Values["homeTypeId"] as string;
        if (homeTypeId.IsNotProvided())
        {
            throw new ArgumentException("Application Id is not present in Route data");
        }

        return homeTypeId!;
    }

    public static string? GetOptionalHomeTypeIdFromRoute(this IRazorPage razorPage)
    {
        var homeTypeId = razorPage.ViewContext.RouteData.Values["homeTypeId"] as string;
        if (homeTypeId.IsNotProvided())
        {
            return null;
        }

        return homeTypeId!;
    }
}
