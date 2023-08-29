using HE.InvestmentLoans.BusinessLogic.LoanApplicationLegacy.Workflow;
using HE.InvestmentLoans.Contract.Application.Queries;
using HE.InvestmentLoans.WWW.Attributes;
using HE.InvestmentLoans.WWW.Routing;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HE.InvestmentLoans.WWW.Controllers;

public class HomeController : Controller
{
    private readonly IMediator _mediator;

    public HomeController(IMediator mediator)
    {
        _mediator = mediator;
    }

    public IActionResult Index()
    {
        if (User.Identity.IsAuthenticated)
        {
            return RedirectToAction("Dashboard", "Home");
        }
        else
        {
            return RedirectToAction("WhatTheHomeBuildingFundIs", "Guidance");
        }
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [AuthorizeWithCompletedProfile]
    [HttpGet("/dashboard")]
    [WorkflowState(LoanApplicationWorkflow.State.Dashboard)]
    public async Task<IActionResult> Dashboard()
    {
        return View(await _mediator.Send(new GetDashboardDataQuery()));
    }
}
