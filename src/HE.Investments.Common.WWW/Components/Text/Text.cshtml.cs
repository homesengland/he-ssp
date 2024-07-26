using Microsoft.AspNetCore.Mvc;

namespace HE.Investments.Common.WWW.Components.Text;

public class Text : ViewComponent
{
    public IViewComponentResult Invoke(string value)
    {
        return View("Text", value);
    }
}
