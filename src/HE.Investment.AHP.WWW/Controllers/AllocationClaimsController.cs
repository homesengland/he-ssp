using HE.Investment.AHP.Contract.Application;
using HE.Investments.Account.Shared.Authorization.Attributes;
using HE.Investments.AHP.Allocation.Contract;
using HE.Investments.AHP.Allocation.Contract.Claims;
using HE.Investments.AHP.Allocation.Contract.Claims.Enum;
using HE.Investments.Common.Contract;
using Microsoft.AspNetCore.Mvc;

namespace HE.Investment.AHP.WWW.Controllers;

[AuthorizeWithCompletedProfile]
[Route("{organisationId}/allocation/{allocationId}/claims")]
public class AllocationClaimsController : Controller
{
    [HttpGet("summary")]
    public IActionResult Summary()
    {
        return View();
    }

    [HttpGet("{phaseId}/overview")]
    public IActionResult Overview()
    {
        var model = new Phase(
            "Phase one",
            new AllocationBasicInfo(new AllocationId("allocation-id"), "Allocation one", "G0001231", "Oxford", "AHP 21-26 CME", Tenure.SharedOwnership),
            new[]
            {
                new MilestoneClaim(MilestoneType.Acquisition, MilestoneStatus.Submitted, 3000000, 0.3m, new DateDetails("01", "04", "2025"), null),
                new MilestoneClaim(MilestoneType.StartOnSite, MilestoneStatus.Due, 4000000, 0.4m, new DateDetails("02", "05", "2026"), null),
                new MilestoneClaim(MilestoneType.Completion, MilestoneStatus.DueSoon, 3000000, 0.3m, new DateDetails("03", "06", "2027"), null),
            });

        return View(model);
    }
}
