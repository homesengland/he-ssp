using Microsoft.AspNetCore.Mvc;

namespace HE.Investment.AHP.WWW.Views.Shared.Components.ProjectListCard;

public class ProjectListCard : ViewComponent
{
    public IViewComponentResult Invoke(ProjectListCardModel model)
    {
        return View("ProjectListCard", model);
    }
}
