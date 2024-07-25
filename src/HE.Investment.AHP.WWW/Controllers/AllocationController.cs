using HE.Investments.Account.Shared.Authorization.Attributes;
using HE.Investments.AHP.Allocation.Contract;
using HE.Investments.AHP.Allocation.Contract.Overview;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HE.Investment.AHP.WWW.Controllers;

[AuthorizeWithCompletedProfile]
[Route("{organisationId}/allocation/{allocationId}")]
public class AllocationController : Controller
{
    private readonly IMediator _mediator;

    public AllocationController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("overview")]
    [HttpGet("")]
    public async Task<IActionResult> Overview(string allocationId)
    {
        return View("Overview", await _mediator.Send(new GetAllocationOverviewQuery(AllocationId.From(allocationId))));
    }
}
