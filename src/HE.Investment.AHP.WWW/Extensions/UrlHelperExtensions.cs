using HE.Investment.AHP.WWW.Controllers;
using HE.Investments.Common.WWW.Extensions;
using HE.Investments.Common.WWW.Utils;
using Microsoft.AspNetCore.Mvc;

namespace HE.Investment.AHP.WWW.Extensions;

public static class UrlHelperExtensions
{
    public static IActionResult RedirectToTaskList(this IUrlHelper urlHelper, string applicationId)
    {
        var organisationId = urlHelper.ActionContext.HttpContext.GetOrganisationIdFromRoute();
        return new RedirectToActionResult(
            nameof(ApplicationController.TaskList),
            new ControllerName(nameof(ApplicationController)).WithoutPrefix(),
            new { applicationId, organisationId },
            null)
        {
            UrlHelper = urlHelper,
        };
    }

    public static IActionResult RedirectToSitesList(this IUrlHelper urlHelper, string projectId)
    {
        var organisationId = urlHelper.ActionContext.HttpContext.GetOrganisationIdFromRoute();
        return new RedirectToActionResult(
            nameof(SiteController.Index),
            new ControllerName(nameof(SiteController)).WithoutPrefix(),
            new { projectId, organisationId },
            null)
        {
            UrlHelper = urlHelper,
        };
    }

    public static IActionResult RedirectToProjectDetails(this IUrlHelper urlHelper, string projectId)
    {
        var organisationId = urlHelper.ActionContext.HttpContext.GetOrganisationIdFromRoute();
        return new RedirectToActionResult(
            nameof(ProjectController.Details),
            new ControllerName(nameof(ProjectController)).WithoutPrefix(),
            new { projectId, organisationId },
            null)
        {
            UrlHelper = urlHelper,
        };
    }
}
