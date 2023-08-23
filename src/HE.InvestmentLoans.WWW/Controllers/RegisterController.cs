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

    public RegisterController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("profile-details")]
    public async Task<IActionResult> ProfileDetails()
    {
        var response = await _mediator.Send(new GetUserDetailsQuery());

        return View(response.ViewModel);
    }

    [HttpPost("profile-details")]
    public async Task<IActionResult> ProfileDetails(UserDetailsViewModel userDetailsViewModel)
    {
        await _mediator.Send(new ProvideUserDetailsCommand(userDetailsViewModel));

        return RedirectToAction("SearchOrganization", "Organization");
    }

    [HttpGet("terms-and-conditions")]
    public IActionResult TermsAndConditions()
    {
        return View();
    }
}
