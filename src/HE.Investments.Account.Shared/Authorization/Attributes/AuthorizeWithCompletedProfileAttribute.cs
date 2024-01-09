using HE.Investments.Account.Contract.Users;
using HE.Investments.Account.Shared.Routing;
using HE.Investments.Common.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;

namespace HE.Investments.Account.Shared.Authorization.Attributes;

[AttributeUsage(AttributeTargets.All)]
public class AuthorizeWithCompletedProfileAttribute : AuthorizeAttribute, IAsyncActionFilter
{
    private readonly IEnumerable<UserRole> _allowedFor;

    public AuthorizeWithCompletedProfileAttribute(string allowedFor)
        : this(allowedFor.Split(',').Select(x => (UserRole)Enum.Parse(typeof(UserRole), x)).ToArray())
    {
    }

    public AuthorizeWithCompletedProfileAttribute(UserRole allowedFor)
        : this(allowedFor.ToString())
    {
    }

    public AuthorizeWithCompletedProfileAttribute(UserRole[]? allowedFor = null)
    {
        if (allowedFor.IsNotProvided())
        {
            _allowedFor = new[]
            {
                UserRole.Admin,
                UserRole.Enhanced,
                UserRole.Input,
                UserRole.ViewOnly,
                UserRole.Limited,
            };
        }
        else
        {
            _allowedFor = allowedFor!;
        }
    }

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var accountUserContext = context.HttpContext.RequestServices.GetRequiredService<IAccountUserContext>();
        var accountRoutes = context.HttpContext.RequestServices.GetRequiredService<IAccountRoutes>();

        if (!accountUserContext.IsLogged)
        {
            context.Result = accountRoutes.LandingPageForNotLoggedUser();
            return;
        }

        if (!await accountUserContext.IsProfileCompleted())
        {
            context.Result = accountRoutes.NotCompleteProfile();
            return;
        }

        if (!await accountUserContext.IsLinkedWithOrganisation())
        {
            context.Result = accountRoutes.NotLinkedOrganisation();
            return;
        }

        var userRoles = (await accountUserContext.GetSelectedAccount()).Roles;
        if (!_allowedFor.Any(allowedRole => userRoles.Any(role => role == allowedRole)))
        {
            throw new UnauthorizedAccessException();
        }

        await next();
    }
}
