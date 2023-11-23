using Microsoft.AspNetCore.Mvc;

namespace HE.Investments.Common.WWW.Components.HelpPanel;

public class HelpPanel : ViewComponent
{
    public IViewComponentResult Invoke(string emailAddress, string phoneNumber)
    {
        return View("HelpPanel", (EmailAddress: emailAddress, PhoneNumber: phoneNumber));
    }
}
