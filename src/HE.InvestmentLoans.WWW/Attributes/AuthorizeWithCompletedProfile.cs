using HE.InvestmentLoans.BusinessLogic.User;
using HE.InvestmentLoans.BusinessLogic.User.Entities;
using HE.InvestmentLoans.Common.Extensions;
using HE.InvestmentLoans.WWW.Controllers;
using HE.InvestmentLoans.WWW.Utils.ValueObjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace HE.InvestmentLoans.WWW.Attributes;

[AttributeUsage(AttributeTargets.All)]
public class AuthorizeWithCompletedProfile : AuthorizeAttribute, IAsyncActionFilter
{
    private readonly IEnumerable<string> _allowedFor;

    public AuthorizeWithCompletedProfile(string[] allowedFor = null)
        : base()
    {
        if (allowedFor.IsNotProvided())
        {
            _allowedFor = new[] { UserAccountRole.LimitedUser, UserAccountRole.Admin };
        }
        else
        {
            _allowedFor = allowedFor;
        }
    }

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var loanUserContext = context.HttpContext.RequestServices.GetRequiredService<ILoanUserContext>();

        var isProfileCompleted = await loanUserContext.IsProfileCompleted();

        if (!isProfileCompleted)
        {
            context.Result = new RedirectToActionResult(
                nameof(UserController.ProfileDetails),
                new ControllerName(nameof(UserController)).WithoutPrefix(),
                null);
            return;
        }

        if (!await loanUserContext.IsLinkedWithOrganization())
        {
            context.Result = new RedirectToActionResult(
                nameof(OrganizationController.SearchOrganization),
                new ControllerName(nameof(OrganizationController)).WithoutPrefix(),
                null);
            return;
        }

        var userRoles = (await loanUserContext.GetSelectedAccount()).Roles;
        if (!_allowedFor.Any(allowedRole => userRoles.Any(role => role.Role == allowedRole)))
        {
            throw new UnauthorizedAccessException();
        }

        await next();
    }
}
