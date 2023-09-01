using HE.InvestmentLoans.BusinessLogic.LoanApplicationLegacy.Workflow;
using HE.InvestmentLoans.BusinessLogic.User;
using HE.InvestmentLoans.Contract.Application.Queries;
using HE.InvestmentLoans.WWW.Attributes;
using HE.InvestmentLoans.WWW.Routing;
using HE.InvestmentLoans.WWW.Utils.ValueObjects;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HE.InvestmentLoans.WWW.Controllers;

public class HomeController : Controller
{
    private readonly IMediator _mediator;
    private readonly ILoanUserContext _userContext;

    public HomeController(IMediator mediator, ILoanUserContext userContext)
    {
        _mediator = mediator;
        _userContext = userContext;
    }

    [Authorize]
    public async Task<IActionResult> Index()
    {
        if (!await _userContext.IsProfileCompleted())
        {
            return RedirectToAction(nameof(UserController.ProfileDetails), new ControllerName(nameof(UserController)).WithoutPrefix());
        }

        if (!await _userContext.IsLinkedWithOrganization())
        {
            return RedirectToAction(nameof(OrganizationController.SearchOrganization), new ControllerName(nameof(OrganizationController)).WithoutPrefix());
        }

        return RedirectToAction("Dashboard", "Home");
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [HttpGet("/dashboard")]
    [AuthorizeWithCompletedProfile]
    [WorkflowState(LoanApplicationWorkflow.State.Dashboard)]
    public async Task<IActionResult> Dashboard()
    {
        return View(await _mediator.Send(new GetDashboardDataQuery()));
    }
}
