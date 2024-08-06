using HE.Investments.AHP.Allocation.Contract;
using HE.Investments.AHP.Allocation.Contract.Overview;
using HE.Investments.AHP.Allocation.Domain.UserContext;
using HE.Investments.Common;
using HE.Investments.Common.Contract.Exceptions;
using HE.Investments.Consortium.Shared.Authorization;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.FeatureManagement;

namespace HE.Investment.AHP.WWW.Controllers;

[ConsortiumAuthorize(AllocationAccessContext.ViewAllocation)]
[Route("{organisationId}/allocation/{allocationId}")]
public class AllocationController : Controller
{
    private readonly IMediator _mediator;

    private readonly IFeatureManager _featureManager;

    public AllocationController(IMediator mediator, IFeatureManager featureManager)
    {
        _mediator = mediator;
        _featureManager = featureManager;
    }

    [HttpGet("overview")]
    [HttpGet("")]
    public async Task<IActionResult> Overview(string allocationId, CancellationToken cancellationToken)
    {
        return View(await _mediator.Send(new GetAllocationOverviewQuery(AllocationId.From(allocationId)), cancellationToken));
    }

    [ConsortiumAuthorize(AllocationAccessContext.ManageAllocation)]
    [HttpGet("manage")]
    public async Task<IActionResult> Manage(string organisationId, string allocationId, CancellationToken cancellationToken)
    {
        if (!await _featureManager.IsEnabledAsync(FeatureFlags.AhpVariations))
        {
            throw new NotFoundException("Page not found");
        }

        var allocation = await _mediator.Send(new GetAllocationOverviewQuery(AllocationId.From(allocationId)), cancellationToken);

        return RedirectToAction(allocation.IsDraft ? "TaskList" : nameof(ChangeDeliveryPlan), new { organisationId, allocationId });
    }

    [ConsortiumAuthorize(AllocationAccessContext.ManageAllocation)]
    [HttpGet("change-delivery-plan")]
    public async Task<IActionResult> ChangeDeliveryPlan(string allocationId, CancellationToken cancellationToken)
    {
        if (!await _featureManager.IsEnabledAsync(FeatureFlags.AhpVariations))
        {
            throw new NotFoundException("Page not found");
        }

        return View(await _mediator.Send(new GetAllocationOverviewQuery(AllocationId.From(allocationId)), cancellationToken));
    }

    [ConsortiumAuthorize(AllocationAccessContext.ManageAllocation)]
    [HttpGet("task-list")]
    public async Task<IActionResult> TaskList()
    {
        if (!await _featureManager.IsEnabledAsync(FeatureFlags.AhpVariations))
        {
            throw new NotFoundException("Page not found");
        }

        return View(nameof(TaskList));
    }
}
