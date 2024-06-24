using HE.Investments.Common.Contract;
using Microsoft.AspNetCore.Mvc;

namespace HE.Investments.Common.WWW.Components.ApplicationStatusTagComponent;

public class ApplicationStatusTagComponent : ViewComponent
{
    public IViewComponentResult Invoke(ApplicationStatus applicationStatus, string additionalClasses = "")
    {
        return View("ApplicationStatusTagComponent", (applicationStatus, additionalClasses));
    }
}
