using FluentValidation;
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

    // private readonly IValidator<UserDetailsViewModel> _validator;

    // public RegisterController(IMediator mediator, IValidator<UserDetailsViewModel> validator)
    public RegisterController(IMediator mediator)
    {
        _mediator = mediator;

        // _validator = validator;
    }

    [HttpGet("profile-details")]
    public async Task<IActionResult> ProfileDetails()
    {
        var response = await _mediator.Send(new GetUserDetailsQuery());

        return View(response);
    }

    [HttpPost("profile-details")]
    public async Task<IActionResult> ProfileDetails(GetUserDetailsResponse getUserDetailsResponse)
    {
        await _mediator.Send(new ProvideUserDetailsCommand(
            getUserDetailsResponse.FirstName,
            getUserDetailsResponse.Surname,
            getUserDetailsResponse.JobTitle,
            getUserDetailsResponse.TelephoneNumber,
            getUserDetailsResponse.SecondaryTelephoneNumber));

        return RedirectToAction("SearchOrganization", "Organization");
    }

    [HttpGet("terms-and-conditions")]
    public IActionResult TermsAndConditions()
    {
        return View();
    }
}
