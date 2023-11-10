using HE.InvestmentLoans.Contract.Application.Queries;
using HE.InvestmentLoans.Contract.User;
using HE.InvestmentLoans.Contract.User.Commands;
using HE.InvestmentLoans.Contract.User.Queries;
using HE.Investments.Account.Contract.Organisation.Queries;
using HE.Investments.Common.Validators;
using HE.Investments.Common.WWW.Utils;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HE.InvestmentLoans.WWW.Controllers;

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
        var response = await _mediator.Send(new GetUserDetailsQuery());

        return View(response.ViewModel);
    }

    [HttpPost("profile-details")]
    public async Task<IActionResult> ProfileDetails(UserDetailsViewModel viewModel, string callback, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(
            new ProvideUserDetailsCommand(
                viewModel.FirstName,
                viewModel.LastName,
                viewModel.JobTitle,
                viewModel.TelephoneNumber,
                viewModel.SecondaryTelephoneNumber),
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
