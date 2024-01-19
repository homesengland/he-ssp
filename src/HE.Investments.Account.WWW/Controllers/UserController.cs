using HE.Investments.Account.Contract.User;
using HE.Investments.Account.Contract.User.Commands;
using HE.Investments.Account.Contract.User.Queries;
using HE.Investments.Account.Shared.Routing;
using HE.Investments.Common.Extensions;
using HE.Investments.Common.Validators;
using HE.Investments.Common.WWW.Utils;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HE.Investments.Account.WWW.Controllers;

[Authorize]
[Route(UserAccountEndpoints.Controller)]
public class UserController : Controller
{
    private readonly IMediator _mediator;

    public UserController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet(UserAccountEndpoints.ProfileDetailsSuffix)]
    public async Task<IActionResult> GetProfileDetails()
    {
        var viewModel = await _mediator.Send(new GetUserProfileDetailsQuery());
        return View("ProfileDetails", viewModel);
    }

    [HttpPost(UserAccountEndpoints.ProfileDetailsSuffix)]
    public async Task<IActionResult> SaveProfileDetails(
        UserProfileDetailsModel viewModel,
        string? callback,
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

        if (callback.IsNotProvided())
        {
            return RedirectToAction(
                nameof(OrganisationController.SearchOrganization),
                new ControllerName(nameof(OrganisationController)).WithoutPrefix());
        }

        return Redirect(callback!);
    }
}
