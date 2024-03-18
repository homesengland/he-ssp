using Microsoft.AspNetCore.Mvc;

namespace HE.Investments.FrontDoor.WWW.Views.Shared.Components.LocalAuthorityWorkflowBackButton;

public class LocalAuthorityWorkflowBackButton : ViewComponent
{
    public IViewComponentResult Invoke(Enum currentPage, string projectId, string siteId)
    {
        return View("LocalAuthorityWorkflowBackButton", (currentPage, projectId, siteId));
    }
}
