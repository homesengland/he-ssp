using Microsoft.AspNetCore.Mvc;

namespace HE.Investments.FrontDoor.WWW.Views.Shared.Components.FrontDoorWorkflowBackButton;

public class FrontDoorWorkflowBackButton : ViewComponent
{
    public IViewComponentResult Invoke(Enum currentPage)
    {
        var redirectParametersFactory = RedirectParametersFactory;
        return View("FrontDoorWorkflowBackButton", (currentPage, redirectParametersFactory));
    }

    private object RedirectParametersFactory(Enum currentPage)
    {
        return new
        {
            projectId = ViewContext.RouteData.Values["projectId"] as string,
            currentPage = currentPage.ToString(),
        };
    }
}
