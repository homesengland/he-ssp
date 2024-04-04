using HE.Investment.AHP.Contract.Application;
using HE.Investment.AHP.Contract.Application.Queries;
using HE.Investment.AHP.Contract.Delivery.Commands;
using HE.Investment.AHP.Contract.Delivery.Enums;
using HE.Investment.AHP.Contract.Delivery.Queries;
using HE.Investment.AHP.WWW.Extensions;
using HE.Investment.AHP.WWW.Models.Delivery;
using HE.Investments.Account.Shared.Authorization.Attributes;
using HE.Investments.Common.Validators;
using HE.Investments.Common.WWW.Extensions;
using HE.Investments.Common.WWW.Helpers;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HE.Investment.AHP.WWW.Controllers;

[AuthorizeWithCompletedProfile]
[Route("application/{applicationId}/delivery")]
public class DeliveryController : Controller
{
    private readonly IMediator _mediator;

    public DeliveryController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("start")]
    public async Task<IActionResult> Start(Guid applicationId, CancellationToken cancellationToken)
    {
        var application = await _mediator.Send(new GetApplicationQuery(AhpApplicationId.From(applicationId)), cancellationToken);
        return View("Index", application);
    }

    [HttpGet]
    public async Task<IActionResult> List([FromRoute] string applicationId, CancellationToken cancellationToken)
    {
        var deliveryPhases = await _mediator.Send(new GetDeliveryPhasesQuery(AhpApplicationId.From(applicationId)), cancellationToken);

        return View(new DeliveryListModel(deliveryPhases.Application.Name)
        {
            AllowedOperations = deliveryPhases.Application.AllowedOperations,
            UnusedHomeTypesCount = deliveryPhases.UnusedHomeTypesCount,
            DeliveryPhases = deliveryPhases.DeliveryPhases
                .Select(x => new DeliveryPhaseItemModel(
                    x.Id,
                    x.Name,
                    x.NumberOfHomes,
                    DateHelper.DisplayAsUkFormatDate(x.Acquisition),
                    DateHelper.DisplayAsUkFormatDate(x.StartOnSite),
                    DateHelper.DisplayAsUkFormatDate(x.PracticalCompletion)))
                .ToList(),
        });
    }

    [HttpPost]
    public async Task<IActionResult> List(string applicationId, DeliveryListModel model, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new CompleteDeliverySectionCommand(AhpApplicationId.From(applicationId), IsDeliveryCompleted.Yes, true), cancellationToken);
        if (result.HasValidationErrors)
        {
            ModelState.AddValidationErrors(result);
            return View(model);
        }

        return HttpContext.Request.IsSaveAndReturnAction()
            ? Url.RedirectToTaskList(applicationId)
            : RedirectToAction("Complete", new { applicationId });
    }

    [HttpGet("complete")]
    public async Task<IActionResult> Complete(string applicationId, CancellationToken cancellationToken)
    {
        var application = await _mediator.Send(new GetApplicationQuery(AhpApplicationId.From(applicationId)), cancellationToken);
        return View(new CompleteDeliverySectionModel(application.Name));
    }

    [HttpPost("complete")]
    public async Task<IActionResult> Complete(string applicationId, CompleteDeliverySectionModel model, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new CompleteDeliverySectionCommand(AhpApplicationId.From(applicationId), model.IsDeliveryCompleted), cancellationToken);
        if (result.HasValidationErrors)
        {
            ModelState.AddValidationErrors(result);
            return View(model);
        }

        return HttpContext.Request.IsSaveAndReturnAction() || model.IsDeliveryCompleted == IsDeliveryCompleted.Yes
            ? Url.RedirectToTaskList(applicationId)
            : RedirectToAction("List", new { applicationId });
    }
}
