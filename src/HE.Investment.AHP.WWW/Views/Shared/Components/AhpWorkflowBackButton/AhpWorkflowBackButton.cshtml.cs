using Microsoft.AspNetCore.Mvc;

namespace HE.Investment.AHP.WWW.Views.Shared.Components.AhpWorkflowBackButton;

public class AhpWorkflowBackButton : ViewComponent
{
    public IViewComponentResult Invoke(Enum currentPage, string? projectId = null)
    {
        object RedirectParametersFactory(Enum currentPageParam) => new
        {
            applicationId = ViewContext.RouteData.Values["applicationId"] as string,
            homeTypeId = ViewContext.RouteData.Values["homeTypeId"] as string,
            deliveryPhaseId = ViewContext.RouteData.Values["deliveryPhaseId"] as string,
            siteId = ViewContext.RouteData.Values["siteId"] as string,
            consortiumId = ViewContext.RouteData.Values["consortiumId"] as string,
            projectId,
            currentPage = currentPageParam.ToString(),
        };

        var redirectParametersFactory = RedirectParametersFactory;

        return View("AhpWorkflowBackButton", (currentPage, redirectParametersFactory));
    }
}
