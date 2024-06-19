using System.Reflection;
using HE.Investments.Common.Contract.Validators;
using HE.Investments.Common.Validators;
using HE.Investments.Common.WWW.Attributes;
using HE.Investments.Common.WWW.Extensions;
using HE.Investments.Common.WWW.Utils;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HE.Investments.Common.WWW.Controllers;

public static class ControllerExtensions
{
    public static async Task<IActionResult> ExecuteCommand<TModel>(
        this Controller controller,
        IMediator mediator,
        IRequest<OperationResult> command,
        Func<Task<IActionResult>> onSuccess,
        Func<Task<IActionResult>> onError,
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(command, cancellationToken);

        if (result.HasValidationErrors)
        {
            controller.ModelState.Clear();
            controller.ModelState.AddValidationErrors(result);

            var orderedProperties = GetOrderedPropertiesNames<TModel>();
            controller.ViewBag.validationErrors = controller.ViewData.ModelState.GetOrderedErrors([.. orderedProperties]);

            return await onError();
        }

        return await onSuccess();
    }

    public static RedirectToActionResult RedirectToActionWithOrganisationId(
        this Controller controller,
        string actionName,
        string? controllerName = null,
        object? routeValues = null)
    {
        var organisationId = controller.HttpContext.GetOrganisationIdFromRoute();
        return controller.RedirectToAction(
            actionName,
            controllerName ?? new ControllerName(controller.GetType().Name).WithoutPrefix(),
            routeValues.ExpandRouteValues(new { organisationId }));
    }

    public static void AddOrderedErrors<T>(this Controller controller, OperationResult operationResult)
    {
        controller.ModelState.AddValidationErrors(operationResult);
        var orderedProperties = GetOrderedPropertiesNames<T>();
        controller.ViewBag.validationErrors = controller.ViewData.ModelState.GetOrderedErrors([.. orderedProperties]);
    }

    private static List<string> GetOrderedPropertiesNames<T>()
    {
        var type = typeof(T);
        var order = new Dictionary<string, ErrorSummaryOrderAttribute?>();

        if (IsRecord(type))
        {
            var recordParams = type
                .GetConstructors()
                .Single()
                .GetParameters();

            foreach (var param in recordParams)
            {
                var orderAttribute = param.GetCustomAttributes<ErrorSummaryOrderAttribute>().FirstOrDefault();
                order.Add(param.Name!, orderAttribute);
            }
        }
        else
        {
            var props = type.GetProperties();
            foreach (var property in props)
            {
                var orderAttribute = property.GetCustomAttributes<ErrorSummaryOrderAttribute>().FirstOrDefault();
                order.Add(property.Name, orderAttribute);
            }
        }

        if (order.Values.All(v => v == null))
        {
            return [.. order.Keys];
        }

        return order
            .ToDictionary(k => k.Key, v => v.Value?.Order ?? int.MaxValue)
            .OrderBy(i => i.Value)
            .Select(i => i.Key)
            .ToList();
    }

    private static bool IsRecord(Type type)
    {
        // https://stackoverflow.com/questions/64809750/how-to-check-if-type-is-a-record
        return Array.Exists(type.GetMethods(), m => m.Name == "<Clone>$");
    }
}
