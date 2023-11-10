using Microsoft.AspNetCore.Mvc;

namespace HE.Investments.Common.WWW.Components.WorkflowBackButton;

public class WorkflowBackButton : ViewComponent
{
    public IViewComponentResult Invoke(string action, string? paramName = null)
    {
        return View("WorkflowBackButton", (action, paramName));
    }
}
