using Microsoft.AspNetCore.Mvc;

namespace HE.Investment.AHP.WWW.Views.Shared.Components.AhpWorkflowBackButton;

public class AhpWorkflowBackButton : ViewComponent
{
    public IViewComponentResult Invoke(Enum currentPage)
    {
        var redirectParametersFactory = RedirectParametersFactory;
        return View("AhpWorkflowBackButton", (currentPage, redirectParametersFactory));
    }

    private object RedirectParametersFactory(Enum currentPage)
    {
        return new
        {
            applicationId = ViewContext.RouteData.Values["applicationId"] as string,
            homeTypeId = ViewContext.RouteData.Values["homeTypeId"] as string,
            deliveryPhaseId = ViewContext.RouteData.Values["deliveryPhaseId"] as string,
            siteId = ViewContext.RouteData.Values["siteId"] as string,
            currentPage = currentPage.ToString(),
        };
    }
}
