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
using HE.Investments.Loans.Common.Routing;
using HE.Investments.Loans.Common.Utils.Constants.FormOption;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using ApplicationId = HE.Investment.AHP.Domain.Application.ValueObjects.ApplicationId;

namespace HE.Investment.AHP.WWW.Controllers;

[AuthorizeWithCompletedProfile]
[Route("application/{applicationId}/delivery")]
public class DeliveryController : WorkflowController<DeliveryWorkflowState>
{
    private readonly IMediator _mediator;
    private readonly IAccountAccessContext _accountAccessContext;

    public DeliveryController(
        IMediator mediator,
        IAccountAccessContext accountAccessContext)
    {
        _mediator = mediator;
        _accountAccessContext = accountAccessContext;
    }

    [HttpGet("start")]
    [WorkflowState(DeliveryWorkflowState.Index)]
    public async Task<IActionResult> Start(Guid applicationId, CancellationToken cancellationToken)
    {
        var application = await _mediator.Send(new GetApplicationQuery(applicationId.ToString()), cancellationToken);
        return View("Index", application);
    }

    [HttpPost("start")]
    [WorkflowState(DeliveryWorkflowState.Index)]
    public async Task<IActionResult> StartPost(Guid applicationId, CancellationToken cancellationToken)
    {
        return await Continue(new { applicationId });
    }

    [HttpGet("back")]
    public Task<IActionResult> Back(DeliveryWorkflowState currentPage, Guid applicationId)
    {
        return Back(currentPage, new { applicationId });
    }

    protected override async Task<IStateRouting<DeliveryWorkflowState>> Routing(DeliveryWorkflowState currentState, object? routeData = null)
    {
        var applicationId = Request.GetRouteValue("applicationId")
                            ?? routeData?.GetPropertyValue<string>("applicationId")
                            ?? throw new NotFoundException($"{nameof(DeliveryController)} required applicationId path parameter.");

        if (string.IsNullOrEmpty(applicationId))
        {
            throw new InvalidOperationException("Cannot find applicationId.");
        }

        var isReadOnly = !await _accountAccessContext.CanEditApplication();
        var delivery = new Delivery();
        return new DeliveryWorkflow(currentState, delivery, isReadOnly);
    }
}
