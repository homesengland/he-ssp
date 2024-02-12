using HE.Investment.AHP.WWW.Controllers;
using HE.Investments.Common.WWW.Utils;
using Microsoft.AspNetCore.Mvc;

namespace HE.Investment.AHP.WWW.Extensions;

public static class UrlHelperExtensions
{
    public static IActionResult RedirectToTaskList(this IUrlHelper urlHelper, string applicationId)
    {
        return new RedirectToActionResult(
            nameof(ApplicationController.TaskList),
            new ControllerName(nameof(ApplicationController)).WithoutPrefix(),
            new { applicationId },
            null)
        {
            UrlHelper = urlHelper,
        };
    }

    public static IActionResult RedirectToSitesList(this IUrlHelper urlHelper)
    {
        return new RedirectToActionResult(
            nameof(SiteController.Index),
            new ControllerName(nameof(SiteController)).WithoutPrefix(),
            null,
            null)
        {
            UrlHelper = urlHelper,
        };
    }
}
