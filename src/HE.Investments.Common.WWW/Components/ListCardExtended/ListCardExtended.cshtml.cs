using HE.Investments.Common.WWW.Components.ListCard;
using Microsoft.AspNetCore.Mvc;

namespace HE.Investments.Common.WWW.Components.ListCardExtended;

public class ListCardExtended : ViewComponent
{
    public IViewComponentResult Invoke(IList<ListCardModel> model)
    {
        return View("ListCardExtended", model);
    }
}
