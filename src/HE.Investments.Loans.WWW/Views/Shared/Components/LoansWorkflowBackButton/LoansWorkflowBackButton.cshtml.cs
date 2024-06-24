using Microsoft.AspNetCore.Mvc;

namespace HE.Investments.Loans.WWW.Views.Shared.Components.LoansWorkflowBackButton;

public class LoansWorkflowBackButton : ViewComponent
{
    public IViewComponentResult Invoke(Enum currentPage)
    {
        object RedirectParametersFactory(Enum currentPageParam) => new
        {
            id = ViewContext.RouteData.Values["id"] as string,
            applicationId = ViewContext.RouteData.Values["applicationId"] as string,
            projectId = ViewContext.RouteData.Values["projectId"] as string,
            fdProjectId = ViewContext.RouteData.Values["fdProjectId"] as string,
            organisationId = ViewContext.RouteData.Values["organisationId"] as string,
            currentPage = currentPageParam.ToString(),
        };

        var redirectParametersFactory = RedirectParametersFactory;

        return View("LoansWorkflowBackButton", (currentPage, redirectParametersFactory));
    }
}
