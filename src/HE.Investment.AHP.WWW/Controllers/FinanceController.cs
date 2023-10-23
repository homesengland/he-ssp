using HE.Investment.AHP.Contract.FinancialDetails.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HE.Investment.AHP.WWW.Controllers;

[Route("finance")]
public class FinanceController : Controller
{
    private readonly IMediator _mediator;

    public FinanceController(IMediator mediator)
    {
        _mediator = mediator;
    }

    public IActionResult Index()
    {
        return View();
    }

    [HttpGet("start")]
    public IActionResult Details()
    {
        return View("Index", Guid.NewGuid());
    }

    [HttpPost("start")]
    public async Task<IActionResult> StartProjectPost()
    {
        var result = await _mediator.Send(new CreateFinancialDetailsCommand());

        // TODO: redirect to next page
        return View("Index", result.ReturnedData.FinancialDetailsId);

        // return await Continue(new { id, projectId = result.ReturnedData.Value });
    }
}
