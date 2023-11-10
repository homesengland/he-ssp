using Microsoft.AspNetCore.Mvc;

#pragma warning disable CA1716
namespace HE.Investment.AHP.WWW.Views.Shared.Components.ReturnToApplicationLink;
#pragma warning restore CA1716

public class ReturnToApplicationLink : ViewComponent
{
    public IViewComponentResult Invoke(string applicationId)
    {
        return View("ReturnToApplicationLink", applicationId);
    }
}
