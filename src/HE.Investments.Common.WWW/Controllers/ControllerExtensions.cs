using HE.Investments.Common.Exceptions;
using HE.Investments.Common.Validators;
using HE.Investments.Common.WWW.Extensions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HE.Investments.Common.WWW.Controllers;

public static class ControllerExtensions
{
    public static async Task<IActionResult> ExecuteCommand<TCommand>(
        this Controller controller,
        IMediator mediator,
        TCommand command,
        Func<IActionResult> onSuccess,
        Func<IActionResult> onError,
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

            return onError();
        }

        return onSuccess();
    }

    public static string GetApplicationIdFromRoute(this Controller controller)
    {
        return controller.Request.GetRouteValue("applicationId") ?? throw new NotFoundException("Missing required applicationId path parameter.");
    }
}
