using HE.InvestmentLoans.BusinessLogic;
using HE.InvestmentLoans.Contract.Application.Commands;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.FeatureManagement.Mvc;

namespace HE.InvestmentLoans.WWW.Controllers;

[FeatureGate(LoansFeatureFlags.SaveApplicationDraftInCrm)]
[Route("loan-application")]
[Authorize]
public class LoanApplicationV2Controller : Controller
{
    private readonly IMediator _mediator;

    public LoanApplicationV2Controller(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("start-new")]
    public async Task<IActionResult> StartNew()
    {
        var loanApplicationId = await _mediator.Send(new StartApplicationCommand());
        return RedirectToAction("Workflow", "LoanApplication", new { id = loanApplicationId, ending = "AboutLoan" });
    }
}
