using Microsoft.AspNetCore.Mvc;

#pragma warning disable CA1716
namespace HE.Investment.AHP.WWW.Views.Shared.Components.ReturnToProjectLink;
#pragma warning restore CA1716

public class ReturnToProjectLink : ViewComponent
{
    public IViewComponentResult Invoke(string projectId)
    {
        return View("ReturnToProjectLink", projectId);
    }
}
