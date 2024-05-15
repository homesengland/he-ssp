using Microsoft.AspNetCore.Mvc;

namespace HE.Investments.Common.WWW.Components.ListCard;

public class ListCard : ViewComponent
{
    public IViewComponentResult Invoke(ListCardModel model)
    {
        return View("ListCard", model);
    }
}
