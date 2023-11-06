using Microsoft.AspNetCore.Mvc;

#pragma warning disable CA1716
namespace HE.Investment.AHP.WWW.Views.Shared.Components.WorkflowBackButton;
#pragma warning restore CA1716

public class WorkflowBackButton : ViewComponent
{
    public IViewComponentResult Invoke(string action, string paramName)
    {
        return View("WorkflowBackButton", (action, paramName));
    }
}
