using HE.Investment.AHP.Contract.Application;
using HE.Investments.Account.Shared.Authorization.Attributes;
using HE.Investments.AHP.Allocation.Contract;
using HE.Investments.AHP.Allocation.Contract.Claims;
using HE.Investments.AHP.Allocation.Contract.Claims.Enum;
using HE.Investments.Common.Contract;
using HE.Investments.Consortium.Shared.UserContext;
using Microsoft.AspNetCore.Mvc;

namespace HE.Investment.AHP.WWW.Controllers;

[AuthorizeWithCompletedProfile]
[Route("{organisationId}/allocation/{allocationId}/claims")]
public class AllocationClaimsController : Controller
{
    private readonly IConsortiumAccessContext _accountAccessContext;

    public AllocationClaimsController(IConsortiumAccessContext accountAccessContext)
    {
        _accountAccessContext = accountAccessContext;
    }

    [HttpGet("summary")]
    public IActionResult Summary()
    {
        return View();
    }

    [HttpGet("{phaseId}/overview")]
    public async Task<IActionResult> Overview()
    {
        var canClaimMilestone = await _accountAccessContext.CanEditApplication();
        var milestones = new[]
        {
            new MilestoneClaim(
                MilestoneType.Acquisition,
                MilestoneStatus.Submitted,
                3000000,
                0.3m,
                new DateDetails("01", "04", "2025"),
                new DateDetails("01", "04", "2025"),
                new DateDetails("20", "03", "2025"),
                false),
            new MilestoneClaim(MilestoneType.StartOnSite, MilestoneStatus.Due, 4000000, 0.4m, new DateDetails("02", "05", "2026"), null, null, true),
            new MilestoneClaim(
                MilestoneType.Completion,
                MilestoneStatus.DueSoon,
                3000000,
                0.3m,
                new DateDetails("03", "06", "2027"),
                null,
                null,
                false),
        };
        var phase = new Phase(
            "Phase one",
            new AllocationBasicInfo(new AllocationId("allocation-id"), "Allocation one", "G0001231", "Oxford", "AHP 21-26 CME", Tenure.SharedOwnership),
            milestones);

        return View((phase, canClaimMilestone));
    }
}
