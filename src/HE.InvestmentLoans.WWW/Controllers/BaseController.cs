using HE.InvestmentLoans.Common.Validation;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HE.InvestmentLoans.WWW.Controllers;

public abstract class BaseController : Controller
{
    private readonly IMediator _mediator;

    protected BaseController(IMediator mediator)
    {
        _mediator = mediator;
    }

    protected async Task<IActionResult> ExecuteCommand<TCommand>(
        TCommand command,
        Func<IActionResult> onSuccess,
        Func<IActionResult> onError,
        CancellationToken cancellationToken)
        where TCommand : IRequest<OperationResult>
    {
        var result = await _mediator.Send(command, cancellationToken);

        if (result.HasValidationErrors)
        {
            ModelState.AddValidationErrors(result);

            return onError();
        }

        return onSuccess();
    }
}
