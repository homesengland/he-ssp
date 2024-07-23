using HE.Investments.AHP.Allocation.Contract.Claims;
using Microsoft.AspNetCore.Mvc;

namespace HE.Investment.AHP.WWW.Views.Shared.Components.PhaseDetailsCard;

public class PhaseDetailsCard : ViewComponent
{
    public IViewComponentResult Invoke(Phase phase, string phaseUrl)
    {
        return View("PhaseDetailsCard", (phase, phaseUrl));
    }
}
