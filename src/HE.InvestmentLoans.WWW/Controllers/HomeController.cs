using HE.InvestmentLoans.BusinessLogic.LoanApplicationLegacy.Workflow;
using HE.InvestmentLoans.BusinessLogic.User;
using HE.InvestmentLoans.Common.Authorization;
using HE.InvestmentLoans.Common.Services;
using HE.InvestmentLoans.Common.Services.Interfaces;
using HE.InvestmentLoans.Contract.Application.Queries;
using HE.InvestmentLoans.WWW.Attributes;
using HE.InvestmentLoans.WWW.Models;
using HE.InvestmentLoans.WWW.Routing;
using HE.InvestmentLoans.WWW.Utils.ValueObjects;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HE.InvestmentLoans.WWW.Controllers;

public class HomeController : Controller
{
    private readonly IMediator _mediator;
    private readonly ILoanUserContext _loanUserContext;
    private readonly IUserContext _userContext;
    private readonly INotificationService _notificationService;

    public HomeController(IMediator mediator, IUserContext userContext, ILoanUserContext loanUserContext, INotificationService notificationService)
    {
        _mediator = mediator;
        _userContext = userContext;
        _loanUserContext = loanUserContext;
        _notificationService = notificationService;
    }

    public async Task<IActionResult> Index()
    {
        if (!_userContext.IsAuthenticated)
        {
            return RedirectToAction(nameof(GuidanceController.WhatTheHomeBuildingFundIs), new ControllerName(nameof(GuidanceController)).WithoutPrefix());
        }

        if (!await _loanUserContext.IsProfileCompleted())
        {
            return RedirectToAction(nameof(UserController.ProfileDetails), new ControllerName(nameof(UserController)).WithoutPrefix());
        }

        if (!await _loanUserContext.IsLinkedWithOrganization())
        {
            return RedirectToAction(nameof(OrganizationController.SearchOrganization), new ControllerName(nameof(OrganizationController)).WithoutPrefix());
        }

        return RedirectToAction(nameof(Dashboard));
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [HttpGet("/dashboard")]
    [AuthorizeWithCompletedProfile]
    [WorkflowState(LoanApplicationWorkflow.State.UserDashboard)]
    public async Task<IActionResult> Dashboard()
    {
        var dashboardData = await _mediator.Send(new GetDashboardDataQuery());

        var viewModel = new DashboardModel
        {
            DashboardData = dashboardData,
            NotificationService = _notificationService,
        };

        return View(viewModel);
    }
}
