using HE.Investment.AHP.Contract.Application.Queries;
using HE.Investment.AHP.Contract.Delivery.Queries;
using HE.Investment.AHP.WWW.Models.Delivery;
using HE.Investments.Account.Shared;
using HE.Investments.Account.Shared.Authorization.Attributes;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HE.Investment.AHP.WWW.Controllers;

[AuthorizeWithCompletedProfile]
[Route("application/{applicationId}/delivery")]
public class DeliveryController : Controller
{
    private readonly IMediator _mediator;

    private readonly IAccountAccessContext _accountAccessContext;

    public DeliveryController(IMediator mediator, IAccountAccessContext accountAccessContext)
    {
        _mediator = mediator;
        _accountAccessContext = accountAccessContext;
    }

    [HttpGet("start")]
    public async Task<IActionResult> Start(Guid applicationId, CancellationToken cancellationToken)
    {
        var application = await _mediator.Send(new GetApplicationQuery(applicationId.ToString()), cancellationToken);
        return View("Index", application);
    }

    [HttpGet]
    public async Task<IActionResult> List([FromRoute] string applicationId, CancellationToken cancellationToken)
    {
        var deliveryPhases = await _mediator.Send(new GetDeliveryPhasesQuery(applicationId), cancellationToken);

        return View(new DeliveryListModel(deliveryPhases.ApplicationName)
        {
            IsEditable = await _accountAccessContext.CanEditApplication(),
            UnusedHomeTypesCount = deliveryPhases.UnusedHomeTypesCount,
            DeliveryPhases = deliveryPhases.DeliveryPhases
                .Select(x => new DeliveryPhaseItemModel(x.Id, x.Name, x.NumberOfHomes, x.Acquisition, x.StartOnSite, x.PracticalCompletion))
                .ToList(),
        });
    }

    [HttpPost]
    public IActionResult List(string applicationId, DeliveryListModel model)
    {
        // TODO: validation + complete section
        return RedirectToAction("TaskList", "Application", new { applicationId });
    }
}
