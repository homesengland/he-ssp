using FluentValidation;
using FluentValidation.AspNetCore;
using HE.InvestmentLoans.BusinessLogic.LoanApplication.QueryHandlers;
using HE.InvestmentLoans.Common.Utils.Constants.ViewName;
using HE.InvestmentLoans.Contract.Application.Queries;
using HE.InvestmentLoans.Contract.Application.ValueObjects;
using HE.InvestmentLoans.Contract.Organization;
using HE.InvestmentLoans.Contract.User;
using HE.InvestmentLoans.Contract.User.Commands;
using HE.InvestmentLoans.Contract.User.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HE.InvestmentLoans.WWW.Controllers;

[Authorize]
[Route("user")]
public class UserController : Controller
{
    private readonly IMediator _mediator;

    private readonly IValidator<UserDetailsViewModel> _validator;

    public UserController(IMediator mediator, IValidator<UserDetailsViewModel> validator)
    {
        _mediator = mediator;
        _validator = validator;
    }

    public async Task<GetUserAccountResponse> Index()
    {
        return await _mediator.Send(new GetUserAccountQuery());
    }

    [Route("dashboard")]
    public async Task<GetDashboardDataQueryResponse> Dashboard()
    {
        return await _mediator.Send(new GetDashboardDataQuery());
    }

    [Route("application-loan/{id}")]
    public async Task<GetLoanApplicationQueryResponse> ApplicationLoan(string id)
    {
        return await _mediator.Send(new GetLoanApplicationQuery(LoanApplicationId.From(id)));
    }

    [Route("organization-details")]
    public async Task<GetOrganizationBasicInformationQueryResponse> OrganizationDetails()
    {
        return await _mediator.Send(new GetOrganizationBasicInformationQuery());
    }

    [HttpGet("profile-details")]
    public async Task<IActionResult> ProfileDetails()
    {
        var response = await _mediator.Send(new GetUserDetailsQuery());

        return View(response.ViewModel);
    }

    [HttpPost("profile-details")]
    public async Task<IActionResult> ProfileDetails(UserDetailsViewModel viewModel, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(viewModel, opt => opt.IncludeRuleSets(UserView.ProfileDetails), cancellationToken);
        if (!validationResult.IsValid)
        {
            validationResult.AddToModelState(ModelState);
            return View(UserView.ProfileDetails, viewModel);
        }

        await _mediator.Send(
            new ProvideUserDetailsCommand(
            viewModel.FirstName,
            viewModel.Surname,
            viewModel.JobTitle,
            viewModel.TelephoneNumber,
            viewModel.SecondaryTelephoneNumber),
            cancellationToken);

        // return RedirectToAction("SearchOrganization", "Organization");
        // temporary change -> waitin for completion of SearchOrganization
        return RedirectToAction("Dashboard", "Home");
    }
}
