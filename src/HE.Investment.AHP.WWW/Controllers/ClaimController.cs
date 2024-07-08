using HE.Investments.Account.Shared.Authorization.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace HE.Investment.AHP.WWW.Controllers;

[Route("{organisationId}/allocation/{allocationId}/claim")]
public class ClaimController : Controller
{
    [HttpGet]
    [Route("/")]
    [Route("summary")]
    [AuthorizeWithCompletedProfile]
    public IActionResult Summary()
    {
        return View();
    }
}
