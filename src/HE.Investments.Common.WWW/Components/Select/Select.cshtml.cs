using Microsoft.AspNetCore.Mvc;

namespace HE.Investments.Common.WWW.Components.Select;

public class Select : ViewComponent
{
    public IViewComponentResult Invoke(string labelText, string name, List<SelectOptionModel> options, string? title = null, bool isLabelHidden = false)
    {
        return View("Select", (labelText, name, options, title, isLabelHidden));
    }
}
