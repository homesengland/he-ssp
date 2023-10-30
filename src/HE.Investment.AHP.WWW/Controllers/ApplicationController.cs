using HE.Investment.AHP.WWW.Models.Application;
using HE.Investment.AHP.WWW.Views.Application;
using HE.Investments.Common.WWW.Components;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HE.Investment.AHP.WWW.Controllers;

[Authorize]
[Route("application")]
public class ApplicationController : Controller
{
    [HttpGet("")]
    public IActionResult TaskList()
    {
        var sections = ApplicationSections.CreateSections(
            SectionStatus.NotStarted,
            SectionStatus.Completed,
            SectionStatus.Withdrawn,
            SectionStatus.CannotStartYet);

        var model = new ApplicationModel("Test Site", "Nazwa aplikacji", sections);

        return View("TaskList", model);
    }
}
