using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace HE.Investments.Common.WWW.Components.WorkflowBackButton;

public class WorkflowBackButton : ViewComponent
{
    public IViewComponentResult Invoke(
        Enum currentPage,
        Func<Enum, object> redirectParametersFactory)
    {
        var redirectParameters = redirectParametersFactory(currentPage);
        return View("WorkflowBackButton", redirectParameters);
    }
}
