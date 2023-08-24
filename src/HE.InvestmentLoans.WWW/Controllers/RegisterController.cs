using FluentValidation;
using FluentValidation.AspNetCore;
using HE.InvestmentLoans.Common.Utils.Constants.ViewName;
using HE.InvestmentLoans.Contract.User;
using HE.InvestmentLoans.Contract.User.Commands;
using HE.InvestmentLoans.Contract.User.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HE.InvestmentLoans.WWW.Controllers;

[Authorize]
[Route("register")]
public class RegisterController : Controller
{
    private readonly IMediator _mediator;

    private readonly IValidator<UserDetailsViewModel> _validator;

    public RegisterController(IMediator mediator, IValidator<UserDetailsViewModel> validator)
    {
        _mediator = mediator;
        _validator = validator;
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
        var validationResult = await _validator.ValidateAsync(viewModel, opt => opt.IncludeRuleSets(RegisterView.ProfileDetails), cancellationToken);
        if (!validationResult.IsValid)
        {
            validationResult.AddToModelState(ModelState);
            return View(RegisterView.ProfileDetails, viewModel);
        }

        await _mediator.Send(
            new ProvideUserDetailsCommand(
            viewModel.FirstName,
            viewModel.Surname,
            viewModel.JobTitle,
            viewModel.TelephoneNumber,
            viewModel.SecondaryTelephoneNumber),
            cancellationToken);
        return RedirectToAction("SearchOrganization", "Organization");
    }

    [HttpGet("terms-and-conditions")]
    public IActionResult TermsAndConditions()
    {
        return View();
    }
}
