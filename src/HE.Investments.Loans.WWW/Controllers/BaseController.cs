using HE.Investments.Common.Validators;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HE.Investments.Loans.WWW.Controllers;

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
            var validationErrors = new List<KeyValuePair<string, string>>();
            foreach (var validationResult in result.Errors)
            {
                validationErrors.Add(new KeyValuePair<string, string>(validationResult.AffectedField, validationResult.ErrorMessage));
            }

            ViewBag.ValidationErrors = validationErrors;

            return onError();
        }

        return onSuccess();
    }
}
