using HE.InvestmentLoans.BusinessLogic;
using HE.InvestmentLoans.BusinessLogic.ViewModel;
using HE.InvestmentLoans.Contract.Application.Commands;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.FeatureManagement.Mvc;

namespace HE.InvestmentLoans.WWW.Controllers;

[FeatureGate(LoansFeatureFlags.SaveApplicationDraftInCrm)]
[Route("application")]
[Authorize]
public class LoanApplicationV2Controller : Controller
{
    private readonly IMediator _mediator;

    public LoanApplicationV2Controller(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("start-new")]
    public async Task<IActionResult> StartNew(CancellationToken cancellationToken)
    {
        var loanApplicationId = await _mediator.Send(new StartApplicationCommand(), cancellationToken);
        return RedirectToAction("AboutLoan", new { id = loanApplicationId.Value });
    }

    [Route("{id}/about-loan")]
    public IActionResult AboutLoan(Guid id)
    {
        return View("~/Views/LoanApplication/AboutLoan.cshtml", new LoanApplicationViewModel() { ID = id });
    }
}
