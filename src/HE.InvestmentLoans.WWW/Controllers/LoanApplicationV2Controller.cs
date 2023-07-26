using HE.InvestmentLoans.Contract.Application.Commands;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HE.InvestmentLoans.WWW.Controllers;

[Route("application")]
[Authorize]
public class LoanApplicationV2Controller : Controller
{
    private readonly IMediator _mediator;

    public LoanApplicationV2Controller(IMediator mediator)
    {
        _mediator = mediator;
    }

    [Route("")]
    public IActionResult StartApplication()
    {
        return View("StartApplication");
    }

    [HttpPost("start-now")]
    public IActionResult StartNow()
    {
        return RedirectToAction("AboutLoan");
    }

    [Route("about-loan")]
    public IActionResult AboutLoan()
    {
        return View("AboutLoan");
    }

    [HttpPost("about-loan")]
    public async Task<IActionResult> AboutLoanPost(CancellationToken cancellationToken)
    {
        var loanApplicationId = await _mediator.Send(new StartApplicationCommand(), cancellationToken);
        return RedirectToAction("Workflow", "LoanApplication", new { id = loanApplicationId.Value });
    }

    [Route("back")]
    public IActionResult AboutLoanBack()
    {
        return RedirectToAction(nameof(StartApplication));
    }
}
