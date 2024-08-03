using Microsoft.AspNetCore.Mvc;

namespace HE.Investments.Common.WWW.Components.ContactInformation;

public class ContactInformation : ViewComponent
{
    public IViewComponentResult Invoke()
    {
        return View("ContactInformation");
    }
}
