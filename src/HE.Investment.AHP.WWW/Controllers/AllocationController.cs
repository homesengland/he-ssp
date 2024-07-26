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
    public async Task<IActionResult> Overview(string allocationId, CancellationToken cancellationToken)
    {
        return View(await _mediator.Send(new GetAllocationOverviewQuery(AllocationId.From(allocationId)), cancellationToken));
    }

    [HttpGet("manage")]
    public async Task<IActionResult> Manage(string organisationId, string allocationId, CancellationToken cancellationToken)
    {
        var allocation = await _mediator.Send(new GetAllocationOverviewQuery(AllocationId.From(allocationId)), cancellationToken);

        return RedirectToAction(allocation.IsDraft ? "TaskList" : nameof(ChangeDeliveryPlan), new { organisationId, allocationId });
    }

    [HttpGet("change-delivery-plan")]
    public async Task<IActionResult> ChangeDeliveryPlan(string allocationId, CancellationToken cancellationToken)
    {
        return View(await _mediator.Send(new GetAllocationOverviewQuery(AllocationId.From(allocationId)), cancellationToken));
    }

    [HttpGet("task-list")]
    public IActionResult TaskList()
    {
        return View(nameof(TaskList));
    }
}
