using HE.Investments.Account.Contract.User.Commands;
using HE.Investments.Account.Contract.User.Queries;
using HE.Investments.Account.Shared.Routing;
using HE.Investments.Account.WWW.Models.User;
using HE.Investments.Account.WWW.Routing;
using HE.Investments.Common.Contract.Validators;
using HE.Investments.Common.Extensions;
using HE.Investments.Common.Messages;
using HE.Investments.Common.User;
using HE.Investments.Common.Validators;
using HE.Investments.Common.WWW.Controllers;
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

    private readonly ProgrammeUrlConfig _programmeUrlConfig;

    private readonly IUserContext _userContext;

    public UserController(IUserContext userContext, IMediator mediator, ProgrammeUrlConfig programmeUrlConfig)
    {
        _userContext = userContext;
        _mediator = mediator;
        _programmeUrlConfig = programmeUrlConfig;
    }

    [AllowAnonymous]
    [HttpGet(UserAccountEndpoints.InformationAboutYourAccount)]
    public IActionResult InformationAboutYourAccount()
    {
        if (_userContext.IsAuthenticated)
        {
            return RedirectToAction("Index", "UserOrganisation");
        }

        return View("InformationAboutHomesEnglandAccount", new AcceptHeTermsAndConditionsModel());
    }

    [AllowAnonymous]
    [HttpPost(UserAccountEndpoints.InformationAboutYourAccount)]
    public IActionResult InformationAboutYourAccount(AcceptHeTermsAndConditionsModel model)
    {
        if (model.AcceptHeTermsAndConditions != "true")
        {
            ModelState.Clear();
            ModelState.AddValidationErrors(OperationResult.New()
                .AddValidationError(nameof(model.AcceptHeTermsAndConditions), ValidationErrorMessage.AcceptTermsAndConditionsAndContinue));
            return View("InformationAboutHomesEnglandAccount", model);
        }

        return RedirectToAction("SignUp", "HeIdentity");
    }

    [HttpGet(UserAccountEndpoints.ProfileDetailsSuffix)]
    public async Task<IActionResult> GetProfileDetails(string? programme, string? callback)
    {
        var userProfile = await _mediator.Send(new GetUserProfileDetailsQuery());
        return View(
            "ProfileDetails",
            new UserProfileDetailsModel
            {
                FirstName = userProfile.FirstName,
                LastName = userProfile.LastName,
                JobTitle = userProfile.JobTitle,
                TelephoneNumber = userProfile.TelephoneNumber,
                SecondaryTelephoneNumber = userProfile.SecondaryTelephoneNumber,
                CallbackUrl = BuildCallbackUrl(programme, callback),
            });
    }

    [HttpPost(UserAccountEndpoints.ProfileDetailsSuffix)]
    public async Task<IActionResult> SaveProfileDetails(UserProfileDetailsModel viewModel, CancellationToken cancellationToken)
    {
        return await this.ExecuteCommand<UserProfileDetailsModel>(
            _mediator,
            new SaveUserProfileDetailsCommand(
                viewModel.FirstName,
                viewModel.LastName,
                viewModel.JobTitle,
                viewModel.TelephoneNumber,
                viewModel.SecondaryTelephoneNumber),
            async () =>
            {
                if (viewModel.CallbackUrl.IsNotProvided())
                {
                    return await Task.FromResult(RedirectToAction(
                        nameof(OrganisationController.SearchOrganisation),
                        new ControllerName(nameof(OrganisationController)).WithoutPrefix()));
                }

                return await Task.FromResult(Redirect(viewModel.CallbackUrl!));
            },
            async () => await Task.FromResult<IActionResult>(View("ProfileDetails", viewModel)),
            cancellationToken);
    }

    private string? BuildCallbackUrl(string? programme, string? callback)
    {
        if (callback.IsNotProvided())
        {
            return null;
        }

        if (programme.IsProvided() && _programmeUrlConfig.ProgrammeUrl.TryGetValue(programme!, out var programmeUrl))
        {
            return $"{programmeUrl.TrimEnd('/')}/{callback!.TrimStart('/')}";
        }

        return callback;
    }
}
