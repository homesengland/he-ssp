using Microsoft.AspNetCore.Mvc;

#pragma warning disable CA1716
namespace HE.Investments.Common.WWW.Components.HelpPanel;
#pragma warning restore CA1716

public class HelpPanel : ViewComponent
{
    public IViewComponentResult Invoke(string emailAddress, string phoneNumber)
    {
        return View("HelpPanel", (EmailAddress: emailAddress, PhoneNumber: phoneNumber));
    }
}
