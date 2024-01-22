using Microsoft.AspNetCore.Mvc;

namespace HE.Investments.Common.WWW.Components.DynamicComponent;

public class DynamicComponent : ViewComponent
{
    public IViewComponentResult Invoke(DynamicComponentViewModel component)
    {
        return View("DynamicComponent", component);
    }
}
