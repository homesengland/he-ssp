using HE.Investment.AHP.Contract.Application;
using HE.Investment.AHP.Contract.Project;
using HE.Investments.Account.Shared.Authorization.Attributes;
using HE.Investments.Common.Contract;
using Microsoft.AspNetCore.Mvc;

namespace HE.Investment.AHP.WWW.Controllers;

[AuthorizeWithCompletedProfile]
[Route("project")]
public class ProjectController : Controller
{
    private static readonly IList<ApplicationProjectModel> MockedApplications =
    [
        new(new AhpApplicationId("1"), "TestName", ApplicationStatus.Draft),
        new(new AhpApplicationId("2"), "TestName2", ApplicationStatus.Rejected),
        new(new AhpApplicationId("3"), "TestName3", ApplicationStatus.Approved),
        new(new AhpApplicationId("4"), "TestName4", ApplicationStatus.Draft),
        new(new AhpApplicationId("5"), "TestName5", ApplicationStatus.Draft),
        new(new AhpApplicationId("6"), "TestName6", ApplicationStatus.Draft),
        new(new AhpApplicationId("7"), "TestName7", ApplicationStatus.Draft),
    ];

    private static readonly ProjectDetailsModel MockedProjectDetailsModel = new("123", "TestName", "AHP", "Morales", MockedApplications, false);

    [HttpGet("{projectId}")]
    public IActionResult Details()
    {
        return View("Details", MockedProjectDetailsModel);
    }

    [HttpGet("{projectId}/applications")]
    public IActionResult Applications()
    {
        return View("ListOfApplications", MockedProjectDetailsModel);
    }
}
