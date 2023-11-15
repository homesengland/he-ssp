using HE.InvestmentLoans.BusinessLogic.LoanApplication;
using HE.InvestmentLoans.BusinessLogic.User;
using HE.InvestmentLoans.Common.Utils.Enums;
using HE.InvestmentLoans.Contract.Application.Queries;
using HE.InvestmentLoans.WWW.Models;
using HE.Investments.Account.Contract.User.Commands;
using HE.Investments.Account.Shared;
using HE.Investments.Account.Shared.Authorization.Attributes;
using HE.Investments.Common.User;
using HE.Investments.Common.Validators;
using HE.Investments.Common.WWW.Routing;
using HE.Investments.Common.WWW.Utils;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HE.InvestmentLoans.WWW.Controllers;

public class HomeController : Controller
{
    private readonly IMediator _mediator;
    private readonly IAccountUserContext _loanUserContext;
    private readonly IUserContext _userContext;

    public HomeController(IMediator mediator, IUserContext userContext, IAccountUserContext loanUserContext)
    {
        _mediator = mediator;
        _userContext = userContext;
        _loanUserContext = loanUserContext;
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

        if (!await _loanUserContext.IsLinkedWithOrganisation())
        {
            return RedirectToAction(nameof(OrganizationController.SearchOrganization), new ControllerName(nameof(OrganizationController)).WithoutPrefix());
        }

        return RedirectToAction(nameof(UserOrganisationController.Index), new ControllerName(nameof(UserOrganisationController)).WithoutPrefix());
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
        return View(await _mediator.Send(new GetDashboardDataQuery()));
    }

    [HttpGet("/accept-he-terms-and-conditions")]
    public IActionResult AcceptHeTermsAndConditions()
    {
        return View();
    }

    [HttpPost("/accept-he-terms-and-conditions")]
    public async Task<IActionResult> AcceptHeTermsAndConditionsPost(
        AcceptHeTermsAndConditionsModel model,
        RedirectOption redirect,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(
            new ProvideAcceptHeTermsAndConditionsCommand(model.AcceptHeTermsAndConditions),
            cancellationToken);

        if (result.HasValidationErrors)
        {
            ModelState.AddValidationErrors(result);
            return View("AcceptHeTermsAndConditions", model);
        }

        if (redirect == RedirectOption.SignIn)
        {
            return RedirectToAction("SignIn", "HeIdentity");
        }

        return RedirectToAction("SignUp", "HeIdentity");
    }
}
