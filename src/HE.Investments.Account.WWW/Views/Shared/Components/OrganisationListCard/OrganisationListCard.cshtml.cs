using Microsoft.AspNetCore.Mvc;

namespace HE.Investments.Account.WWW.Views.Shared.Components.OrganisationListCard;

public class OrganisationListCard : ViewComponent
{
    public IViewComponentResult Invoke(OrganisationListCardModel model)
    {
        return View("OrganisationListCard", model);
    }
}
