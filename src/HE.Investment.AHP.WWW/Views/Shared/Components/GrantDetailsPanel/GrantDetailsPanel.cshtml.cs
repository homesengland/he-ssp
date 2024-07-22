using HE.Investments.AHP.Allocation.Contract.Claims;
using Microsoft.AspNetCore.Mvc;

namespace HE.Investment.AHP.WWW.Views.Shared.Components.GrantDetailsPanel;

public class GrantDetailsPanel : ViewComponent
{
    public IViewComponentResult Invoke(GrantDetails grantDetails)
    {
        return View("GrantDetailsPanel", grantDetails);
    }
}
