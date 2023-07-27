using FluentValidation;
using HE.InvestmentLoans.BusinessLogic.LoanApplicationLegacy.Workflow;
using HE.InvestmentLoans.BusinessLogic.ViewModel;
using HE.InvestmentLoans.Common.Routing;
using HE.InvestmentLoans.Contract.Application.Commands;
using HE.InvestmentLoans.Contract.Application.Enums;
using HE.InvestmentLoans.Contract.Organization;
using HE.InvestmentLoans.Contract.User;
using HE.InvestmentLoans.WWW.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HE.InvestmentLoans.WWW.Controllers;

[Route("application")]
[Authorize]
public class LoanApplicationV2Controller : Controller
{
    private readonly IMediator _mediator;

    private readonly IValidator<LoanPurposeModel> _validator;

    public LoanApplicationV2Controller(IMediator mediator, IValidator<LoanPurposeModel> validator)
    {
        _mediator = mediator;
        _validator = validator;
    }

    [HttpGet("")]
    public IActionResult StartApplication()
    {
        return View("StartApplication");
    }

    [HttpPost("start-now")]
    public IActionResult StartNow()
    {
        return RedirectToAction("AboutLoan");
    }

    [HttpGet("about-loan")]
    public IActionResult AboutLoan()
    {
        return View("AboutLoan");
    }

    [HttpPost("about-loan")]
    public IActionResult AboutLoanPost()
    {
        return RedirectToAction("CheckYourDetails");
    }

    [HttpGet("check-your-details")]
    public async Task<IActionResult> CheckYourDetails(CancellationToken cancellationToken)
    {
        var organizationBasicInformationResponse = await _mediator.Send(new GetOrganizationBasicInformationQuery(), cancellationToken);
        var userDetails = await _mediator.Send(new GetUserDetailsQuery(), cancellationToken);
        return View(
            "CheckYourDetails",
            new CheckYourDetailsModel
            {
                OrganizationBasicInformation = organizationBasicInformationResponse.OrganizationBasicInformation,
                LoanApplicationContactEmail = userDetails.Email,
            });
    }

    [HttpPost("check-your-details")]
    public IActionResult CheckYourDetailsPost()
    {
        return RedirectToAction("LoanPurpose");
    }

    [HttpGet("loan-purpose")]
    public IActionResult LoanPurpose()
    {
        return View("LoanPurpose", new LoanPurposeModel());
    }

    [HttpPost("loan-purpose")]
    public async Task<IActionResult> LoanPurposePost(LoanPurposeModel model, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(model, cancellationToken);
        if (!validationResult.IsValid)
        {
            return View("LoanPurpose", model);
        }

        if (model.FundingPurpose == FundingPurpose.BuildingNewHomes)
        {
            var loanApplicationId = await _mediator.Send(new StartApplicationCommand(), cancellationToken);
            return RedirectToAction("Workflow", "LoanApplication", new { id = loanApplicationId.Value });
        }

        return RedirectToAction("Ineligible");
    }

    [HttpGet("ineligible")]
    public IActionResult Ineligible()
    {
        return View("Ineligible");
    }

    [HttpGet("back")]
    public async Task<IActionResult> Back(LoanApplicationWorkflow.State currentPage)
    {
        var workflow = new LoanApplicationWorkflow(new LoanApplicationViewModel() { State = currentPage, GoodChangeMode = true }, null!);
        var targetState = await workflow.NextState(Trigger.Back);

        return targetState switch
        {
            LoanApplicationWorkflow.State.AboutLoan => RedirectToAction("AboutLoan"),
            LoanApplicationWorkflow.State.CheckYourDetails => RedirectToAction("CheckYourDetails"),
            LoanApplicationWorkflow.State.LoanPurpose => RedirectToAction("LoanPurpose"),
            _ => RedirectToAction("StartApplication"),
        };
    }
}
