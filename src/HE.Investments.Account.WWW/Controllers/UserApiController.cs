using System.Net;
using HE.Investments.Account.Api.Contract.User;
using HE.Investments.Account.Contract.User.Queries;
using HE.Investments.Common;
using HE.Investments.Common.Contract;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.FeatureManagement;

namespace HE.Investments.Account.WWW.Controllers;

[AllowAnonymous]
[Route("api/user/{userId}")]
public class UserApiController : ControllerBase
{
    private readonly IMediator _mediator;

    private readonly IFeatureManager _featureManager;

    public UserApiController(IMediator mediator, IFeatureManager featureManager)
    {
        _mediator = mediator;
        _featureManager = featureManager;
    }

    [HttpGet("profile")]
    public async Task<ActionResult<ProfileDetails>> GetUserProfileDetails([FromRoute] string userId, CancellationToken cancellationToken)
    {
        if (!await _featureManager.IsEnabledAsync(FeatureFlags.AccountApiAccess))
        {
            return StatusCode((int)HttpStatusCode.MethodNotAllowed);
        }

        var profileDetails = await _mediator.Send(new GetUserProfileDetailsQuery(UserGlobalId.From(userId)), cancellationToken);
        return new ProfileDetails(
            profileDetails.FirstName,
            profileDetails.LastName,
            profileDetails.JobTitle,
            profileDetails.Email,
            profileDetails.TelephoneNumber,
            profileDetails.SecondaryTelephoneNumber,
            profileDetails.IsTermsAndConditionsAccepted);
    }

    [HttpGet("accounts")]
    public async Task<ActionResult<AccountDetails>> GetUserAccounts([FromRoute] string userId, [FromQuery] string userEmail, CancellationToken cancellationToken)
    {
        if (!await _featureManager.IsEnabledAsync(FeatureFlags.AccountApiAccess))
        {
            return StatusCode((int)HttpStatusCode.MethodNotAllowed);
        }

        return await _mediator.Send(new GetUserAccountsQuery(UserGlobalId.From(userId), userEmail), cancellationToken);
    }
}
