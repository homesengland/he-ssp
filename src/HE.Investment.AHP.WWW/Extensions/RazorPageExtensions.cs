using Microsoft.AspNetCore.Mvc.Razor;

namespace HE.Investment.AHP.WWW.Extensions;

public static class RazorPageExtensions
{
    public static string GetSchemeId(this IRazorPage razorPage)
    {
        return razorPage.ViewContext.RouteData.Values["schemeId"] as string;
    }
}
