using HE.Investments.Account.Contract.Organisation.Queries;
using HE.Investments.Account.Contract.User;
using HE.Investments.Account.Contract.User.Commands;
using HE.Investments.Account.Contract.User.Queries;
using HE.Investments.Common.Validators;
using HE.Investments.Common.WWW.Utils;
using HE.Investments.Loans.Contract.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HE.Investments.Loans.WWW.Controllers;

[Authorize]
[Route("user")]
public class UserController : Controller
{
    private readonly IMediator _mediator;

    public UserController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [Route("dashboard")]
    public async Task<GetDashboardDataQueryResponse> Dashboard()
    {
        return await _mediator.Send(new GetDashboardDataQuery());
    }

    [Route("organization-details")]
    public async Task<GetOrganizationBasicInformationQueryResponse> OrganizationDetails()
    {
        return await _mediator.Send(new GetOrganizationBasicInformationQuery());
    }

    [HttpGet("profile-details")]
    public async Task<IActionResult> ProfileDetails()
    {
        var response = await _mediator.Send(new GetUserProfileDetailsQuery());

        return View(response);
    }

    [HttpPost("profile-details")]
    public async Task<IActionResult> ProfileDetails(UserProfileDetailsModel viewModel, string callback, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(
            new SaveUserProfileDetailsCommand(
                viewModel.FirstName ?? string.Empty,
                viewModel.LastName ?? string.Empty,
                viewModel.JobTitle ?? string.Empty,
                viewModel.TelephoneNumber ?? string.Empty,
                viewModel.SecondaryTelephoneNumber ?? string.Empty),
            cancellationToken);

        if (result.HasValidationErrors)
        {
            ModelState.AddValidationErrors(result);
            var validationErrors = new List<KeyValuePair<string, string>>();
            foreach (var validationResult in result.Errors)
            {
                validationErrors.Add(new KeyValuePair<string, string>(validationResult.AffectedField, validationResult.ErrorMessage));
            }

            ViewBag.ValidationErrors = validationErrors;

            return View("ProfileDetails", viewModel);
        }

        if (callback == nameof(LoanApplicationV2Controller.CheckYourDetails))
        {
            return RedirectToAction(
                nameof(LoanApplicationV2Controller.CheckYourDetails),
                new ControllerName(nameof(LoanApplicationV2Controller)).WithoutPrefix());
        }

        return RedirectToAction(
            nameof(OrganizationController.SearchOrganization),
            new ControllerName(nameof(OrganizationController)).WithoutPrefix());
    }
}
