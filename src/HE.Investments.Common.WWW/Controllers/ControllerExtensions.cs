using HE.Investments.Common.Contract.Validators;
using HE.Investments.Common.Validators;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HE.Investments.Common.WWW.Controllers;

public static class ControllerExtensions
{
    public static async Task<IActionResult> ExecuteCommand<TCommand>(
        this Controller controller,
        IMediator mediator,
        TCommand command,
        Func<Task<IActionResult>> onSuccess,
        Func<Task<IActionResult>> onError,
        CancellationToken cancellationToken)
        where TCommand : IRequest<OperationResult>
    {
        var result = await mediator.Send(command, cancellationToken);

        if (result.HasValidationErrors)
        {
            controller.ModelState.AddValidationErrors(result);
            var validationErrors = new List<KeyValuePair<string, string>>();
            foreach (var validationResult in result.Errors)
            {
                validationErrors.Add(new KeyValuePair<string, string>(validationResult.AffectedField, validationResult.ErrorMessage));
            }

            controller.ViewBag.ValidationErrors = validationErrors;

            return await onError();
        }

        return await onSuccess();
    }
}
