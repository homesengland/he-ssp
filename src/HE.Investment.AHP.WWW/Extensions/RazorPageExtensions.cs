using Microsoft.AspNetCore.Mvc.Razor;

namespace HE.Investment.AHP.WWW.Extensions;

public static class RazorPageExtensions
{
    public static string GetApplicationIdFromRoute(this IRazorPage razorPage)
    {
        return razorPage.ViewContext.RouteData.Values["applicationId"] as string;
    }

    public static string GetHomeTypeIdFromRoute(this IRazorPage razorPage)
    {
        return razorPage.ViewContext.RouteData.Values["homeTypeId"] as string;
    }
}
