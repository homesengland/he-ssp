using HE.Investments.Account.Api.Contract.User;
using HE.Investments.Account.Shared.Routing;
using HE.Investments.Common.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;

namespace HE.Investments.Account.Shared.Authorization.Attributes;

[AttributeUsage(AttributeTargets.All)]
public class AuthorizeWithCompletedProfileAttribute : AuthorizeAttribute, IAsyncActionFilter
{
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
            AllowedFor = [

                UserRole.Admin,
                UserRole.Enhanced,
                UserRole.Input,
                UserRole.ViewOnly,
                UserRole.Limited,
            ];
        }
        else
        {
            AllowedFor = allowedFor!.ToList();
        }
    }

    public List<UserRole> AllowedFor { get; }

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var accountUserContext = context.HttpContext.RequestServices.GetRequiredService<IAccountUserContext>();
        var accountRoutes = context.HttpContext.RequestServices.GetRequiredService<IAccountRoutes>();
        var canAccessChecks = context.HttpContext.RequestServices.GetServices<ICanAccess>();

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
        if (!AllowedFor.Exists(allowedRole => userRoles.Any(role => role == allowedRole)))
        {
            throw new UnauthorizedAccessException();
        }

        foreach (var canAccessCheck in canAccessChecks)
        {
            if (!await canAccessCheck.CanAccess(AllowedFor))
            {
                throw new UnauthorizedAccessException();
            }
        }

        await next();
    }
}
