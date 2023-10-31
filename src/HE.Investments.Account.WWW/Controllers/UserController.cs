using HE.InvestmentLoans.Common.Validation;
using HE.Investments.Account.Contract.User;
using HE.Investments.Account.Contract.User.Queries;
using HE.Investments.Account.Domain.User.Commands;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HE.Investments.Account.WWW.Controllers;

[Authorize]
[Route("user")]
public class UserController : Controller
{
    private readonly IMediator _mediator;

    public UserController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("profile-details")]
    public async Task<IActionResult> GetProfileDetails()
    {
        var viewModel = await _mediator.Send(new GetUserProfileDetailsQuery());
        return View("ProfileDetails", viewModel);
    }

    [HttpPost("profile-details")]
    public async Task<IActionResult> SaveProfileDetails(
        UserProfileDetailsModel viewModel,
        string callback,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(
            new SaveUserProfileDetailsCommand(
                viewModel.FirstName,
                viewModel.LastName,
                viewModel.JobTitle,
                viewModel.TelephoneNumber,
                viewModel.SecondaryTelephoneNumber),
            cancellationToken);

        if (result.HasValidationErrors)
        {
            ModelState.AddValidationErrors(result);

            return View("ProfileDetails", viewModel);
        }

        return Redirect(callback);
    }
}
