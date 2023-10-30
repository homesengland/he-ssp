using HE.Investment.AHP.Contract.FinancialDetails.Commands;
using HE.Investment.AHP.Contract.FinancialDetails.Models;
using HE.Investment.AHP.Contract.FinancialDetails.Queries;
using HE.Investment.AHP.Contract.FinancialDetails.ValueObjects;
using HE.InvestmentLoans.BusinessLogic.Projects.ValueObjects;
using HE.InvestmentLoans.Common.Validation;
using HE.InvestmentLoans.Contract.Application.ValueObjects;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HE.Investment.AHP.WWW.Controllers;

[Route("fiancial-details")]
public class FinancialDetailsController : Controller
{
    private readonly IMediator _mediator;

    public FinancialDetailsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("start")]
    public IActionResult Start(Guid financialSchemeId, Guid financialDetailsId)
    {
        return View("Index", financialSchemeId);
    }

    [HttpPost("start")]
    public async Task<IActionResult> StartPost(Guid financialSchemeId, [FromQuery] Guid? financialDetailsId)
    {
        if (financialDetailsId.HasValue)
        {
            return RedirectToAction(nameof(LandStatus), new { financialDetailsId = financialDetailsId.Value });
        }

        var result = await _mediator.Send(new StartFinancialDetailsCommand(financialSchemeId));

        return RedirectToAction(nameof(LandStatus), new { financialDetailsId = result.ReturnedData.FinancialDetailsId });
    }

    [HttpGet("{financialDetailsId}/land-status")]
    public async Task<IActionResult> LandStatus(Guid financialDetailsId)
    {
        var financialDetails = await _mediator.Send(new GetFinancialDetailsQuery(FinancialDetailsId.From(financialDetailsId)));
        return View(financialDetails);
    }

    [HttpPost("{financialDetailsId}/land-status")]
    public async Task<IActionResult> LandStatus(Guid id, FinancialDetailsViewModel model)
    {
        var result = await _mediator.Send(new ProvidePurchasePriceCommand(FinancialDetailsId.From(id), model.PurchasePrice, model.IsPurchasePriceKnown ?? false));

        if (result.HasValidationErrors)
        {
            ModelState.AddValidationErrors(result);

            return View("LandStatus", model);
        }

        return View("LandValue", id);
    }
}
