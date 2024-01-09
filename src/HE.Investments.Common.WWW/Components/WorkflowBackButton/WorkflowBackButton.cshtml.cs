using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace HE.Investments.Common.WWW.Components.WorkflowBackButton;

public class WorkflowBackButton : ViewComponent
{
    public IViewComponentResult Invoke(Enum currentPage, string? paramName = null)
    {
        return View("WorkflowBackButton", (currentPage.ToString(), paramName));
    }
}
