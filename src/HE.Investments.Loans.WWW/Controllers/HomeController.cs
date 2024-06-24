using HE.Investments.Account.Shared;
using HE.Investments.Account.Shared.Authorization.Attributes;
using HE.Investments.Common.Contract.Validators;
using HE.Investments.Common.Messages;
using HE.Investments.Common.User;
using HE.Investments.Common.Validators;
using HE.Investments.Common.WWW.Routing;
using HE.Investments.Common.WWW.Utils;
using HE.Investments.Loans.BusinessLogic.LoanApplication;
using HE.Investments.Loans.Common.Utils.Enums;
using HE.Investments.Loans.Contract.Application.Queries;
using HE.Investments.Loans.WWW.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HE.Investments.Loans.WWW.Controllers;

public class HomeController : Controller
{
    private readonly IMediator _mediator;
    private readonly IUserContext _userContext;
    private readonly IAccountUserContext _accountUserContext;

    public HomeController(IMediator mediator, IUserContext userContext, IAccountUserContext accountUserContext)
    {
        _mediator = mediator;
        _userContext = userContext;
        _accountUserContext = accountUserContext;
    }

    public async Task<IActionResult> Index()
    {
        if (!_userContext.IsAuthenticated)
        {
            return RedirectToAction(nameof(GuidanceController.WhatTheHomeBuildingFundIs), new ControllerName(nameof(GuidanceController)).WithoutPrefix());
        }

        var userAccount = await _accountUserContext.GetSelectedAccount();
        var organisationId = userAccount.SelectedOrganisationId();
        return RedirectToAction("Dashboard", new { organisationId });
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [HttpGet("{organisationId}/dashboard")]
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
    public IActionResult AcceptHeTermsAndConditionsPost(AcceptHeTermsAndConditionsModel model, RedirectOption redirect)
    {
        if (model.AcceptHeTermsAndConditions != "checked")
        {
            ModelState.AddValidationErrors(OperationResult.New()
                .AddValidationError(nameof(model.AcceptHeTermsAndConditions), ValidationErrorMessage.AcceptTermsAndConditions));
            return View("AcceptHeTermsAndConditions", model);
        }

        if (redirect == RedirectOption.SignIn)
        {
            return RedirectToAction("SignIn", "HeIdentity");
        }

        return RedirectToAction("SignUp", "HeIdentity");
    }
}
