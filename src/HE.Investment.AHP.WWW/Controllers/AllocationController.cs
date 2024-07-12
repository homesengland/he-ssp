using HE.Investments.Account.Shared.Authorization.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace HE.Investment.AHP.WWW.Controllers;

[AuthorizeWithCompletedProfile]
[Route("{organisationId}/allocation/{allocationId}")]
public class AllocationController : Controller
{
    [HttpGet("overview")]
    public IActionResult Overview()
    {
        return View();
    }
}