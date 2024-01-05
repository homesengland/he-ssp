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
}
