using HE.Investments.Common.WWW.Components.ListCard;
using Microsoft.AspNetCore.Mvc;

namespace HE.Investments.Common.WWW.Components.ListCardContent;

public class ListCardContent : ViewComponent
{
    public IViewComponentResult Invoke(ListCardModel model)
    {
        return View("ListCardContent", model);
    }
}
