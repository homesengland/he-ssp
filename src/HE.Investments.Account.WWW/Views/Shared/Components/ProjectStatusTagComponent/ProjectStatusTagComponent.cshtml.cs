using Microsoft.AspNetCore.Mvc;

namespace HE.Investments.Account.WWW.Views.Shared.Components.ProjectStatusTagComponent;

public class ProjectStatusTagComponent : ViewComponent
{
    public IViewComponentResult Invoke(string status)
    {
        return View("ProjectStatusTagComponent", status);
    }
}
