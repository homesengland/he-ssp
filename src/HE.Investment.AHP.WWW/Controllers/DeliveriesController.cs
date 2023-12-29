using HE.Investment.AHP.Contract.Application;
using HE.Investment.AHP.Contract.Application.Queries;
using HE.Investment.AHP.Contract.Delivery;
using HE.Investments.Account.Shared;
using HE.Investments.Account.Shared.Authorization.Attributes;
using HE.Investments.Common.Extensions;
using HE.Investments.Common.Validators;
using HE.Investments.Common.WWW.Extensions;
using HE.Investments.Common.WWW.Routing;
using HE.Investments.Loans.Common.Exceptions;
using HE.Investments.Loans.Common.Utils.Constants.FormOption;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using ApplicationId = HE.Investment.AHP.Domain.Application.ValueObjects.ApplicationId;

namespace HE.Investment.AHP.WWW.Controllers;

[AuthorizeWithCompletedProfile]
[Route("application/{applicationId}/deliveries")]
public class DeliveriesController : WorkflowController<DeliveriesWorkflowState>
{
    private readonly IMediator _mediator;
    private readonly IAccountAccessContext _accountAccessContext;

    public DeliveriesController(
        IMediator mediator,
        IAccountAccessContext accountAccessContext)
    {
        _mediator = mediator;
        _accountAccessContext = accountAccessContext;
    }

    [HttpGet("start")]
    [WorkflowState(DeliveriesWorkflowState.Index)]
    public async Task<IActionResult> Start(Guid applicationId, CancellationToken cancellationToken)
    {
        var application = await _mediator.Send(new GetApplicationQuery(applicationId.ToString()), cancellationToken);
        return View("Index", application);
    }

    [HttpGet("delivery-list")]
    [WorkflowState(DeliveriesWorkflowState.Index)]
    public async Task<IActionResult> List(Guid applicationId, CancellationToken cancellationToken)
    {
        var application = await _mediator.Send(new GetApplicationQuery(applicationId.ToString()), cancellationToken);
        return View("Index", application);
    }

    [HttpGet("back")]
    public Task<IActionResult> Back(DeliveriesWorkflowState currentPage, Guid applicationId)
    {
        return Back(currentPage, new { applicationId });
    }

    protected override async Task<IStateRouting<DeliveriesWorkflowState>> Routing(DeliveriesWorkflowState currentState, object? routeData = null)
    {
        var applicationId = Request.GetRouteValue("applicationId")
                            ?? routeData?.GetPropertyValue<string>("applicationId")
                            ?? throw new NotFoundException($"{nameof(DeliveriesController)} required applicationId path parameter.");

        if (string.IsNullOrEmpty(applicationId))
        {
            throw new InvalidOperationException("Cannot find applicationId.");
        }

        var isReadOnly = !await _accountAccessContext.CanEditApplication();
        var delivery = new Delivery();
        return new DeliveriesWorkflow(currentState, delivery, isReadOnly);
    }
}
