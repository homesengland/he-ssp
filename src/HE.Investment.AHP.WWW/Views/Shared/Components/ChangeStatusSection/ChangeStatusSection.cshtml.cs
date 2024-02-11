using HE.Investments.Common.Gds;
using Microsoft.AspNetCore.Mvc;

namespace HE.Investment.AHP.WWW.Views.Shared.Components.ChangeStatusSection;

public class ChangeStatusSection : ViewComponent
{
    public IViewComponentResult Invoke(
        string title,
        string paragraph,
        string actionUrl,
        string linkButtonName,
        ButtonType buttonType,
        string testId,
        bool isDisabled = false)
    {
        return View("ChangeStatusSection", (title, paragraph, actionUrl, linkButtonName, buttonType, isDisabled, testId));
    }
}
