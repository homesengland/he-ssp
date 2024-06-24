using HE.Investment.AHP.Contract.Application;
using HE.Investment.AHP.Contract.Application.Queries;
using HE.Investment.AHP.Contract.Delivery.Commands;
using HE.Investment.AHP.Contract.Delivery.Enums;
using HE.Investment.AHP.Contract.Delivery.Queries;
using HE.Investment.AHP.WWW.Extensions;
using HE.Investment.AHP.WWW.Models.Delivery;
using HE.Investments.Common.Validators;
using HE.Investments.Common.WWW.Controllers;
using HE.Investments.Common.WWW.Extensions;
using HE.Investments.Common.WWW.Helpers;
using HE.Investments.Consortium.Shared.Authorization;
using HE.Investments.Consortium.Shared.UserContext;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HE.Investment.AHP.WWW.Controllers;

[ConsortiumAuthorize(ConsortiumAccessContext.Edit)]
[Route("{organisationId}/application/{applicationId}/delivery")]
public class DeliveryController : Controller
{
    private readonly IMediator _mediator;

    public DeliveryController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("start")]
    public async Task<IActionResult> Start(string applicationId, CancellationToken cancellationToken)
    {
        var application = await _mediator.Send(new GetApplicationQuery(AhpApplicationId.From(applicationId)), cancellationToken);
        return View("Index", application);
    }

    [ConsortiumAuthorize]
    [HttpGet]
    public async Task<IActionResult> List([FromRoute] string applicationId, CancellationToken cancellationToken)
    {
        var deliveryPhases = await _mediator.Send(new GetDeliveryPhasesQuery(AhpApplicationId.From(applicationId)), cancellationToken);

        return View(new DeliveryListModel(deliveryPhases.Application.Name)
        {
            ProjectId = deliveryPhases.Application.ProjectId.ToString(),
            AllowedOperations = deliveryPhases.Application.AllowedOperations.ToArray(),
            UnusedHomeTypesCount = deliveryPhases.UnusedHomeTypesCount,
            IsUnregisteredBody = deliveryPhases.IsUnregisteredBody,
            DeliveryPhases = deliveryPhases.DeliveryPhases
                .Select(x => new DeliveryPhaseItemModel(
                    x.Id,
                    x.Name,
                    x.NumberOfHomes,
                    DateHelper.DisplayAsUkFormatDate(x.Acquisition),
                    DateHelper.DisplayAsUkFormatDate(x.StartOnSite),
                    DateHelper.DisplayAsUkFormatDate(x.PracticalCompletion),
                    x.IsOnlyCompletionMilestone))
                .ToList(),
        });
    }

    [HttpPost]
    public async Task<IActionResult> List(string applicationId, DeliveryListModel model, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(
            new CompleteDeliverySectionCommand(AhpApplicationId.From(applicationId), IsDeliveryCompleted.Yes, true),
            cancellationToken);
        if (result.HasValidationErrors)
        {
            ModelState.AddValidationErrors(result);
            return View(model);
        }

        return HttpContext.Request.IsSaveAndReturnAction()
            ? Url.RedirectToTaskList(applicationId)
            : this.OrganisationRedirectToAction("Complete", routeValues: new { applicationId });
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
        var result = await _mediator.Send(
            new CompleteDeliverySectionCommand(AhpApplicationId.From(applicationId), model.IsDeliveryCompleted),
            cancellationToken);
        if (result.HasValidationErrors)
        {
            ModelState.AddValidationErrors(result);
            return View(model);
        }

        return HttpContext.Request.IsSaveAndReturnAction() || model.IsDeliveryCompleted == IsDeliveryCompleted.Yes
            ? Url.RedirectToTaskList(applicationId)
            : this.OrganisationRedirectToAction("List", routeValues: new { applicationId });
    }
}
