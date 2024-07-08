using HE.Investments.Account.Shared.Authorization.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace HE.Investment.AHP.WWW.Controllers;

[Route("{organisationId}/allocation/{allocationId}/claims")]
public class AllocationClaimsController : Controller
{
    [HttpGet]
    [Route("summary")]
    [AuthorizeWithCompletedProfile]
    public IActionResult Summary()
    {
        return View();
    }

    [HttpGet]
    [Route("{phaseId}/overview")]
    [AuthorizeWithCompletedProfile]
    public IActionResult Overview()
    {
        return View();
    }
}
