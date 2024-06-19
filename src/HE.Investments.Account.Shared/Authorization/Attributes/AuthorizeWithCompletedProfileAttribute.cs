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
    public AuthorizeWithCompletedProfileAttribute(string allowedFor, Type? policy = null)
        : this(string.IsNullOrEmpty(allowedFor) ? null : allowedFor.Split(',').Select(x => (UserRole)Enum.Parse(typeof(UserRole), x)).ToArray(), policy)
    {
    }

    public AuthorizeWithCompletedProfileAttribute(UserRole allowedFor, Type? policy = null)
        : this(allowedFor.ToString(), policy)
    {
    }

    public AuthorizeWithCompletedProfileAttribute(UserRole[]? allowedFor = null, Type? policy = null)
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

        AccessPolicy = policy;
    }

    public List<UserRole> AllowedFor { get; }

    public Type? AccessPolicy { get; }

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var effectiveAuthorizePolicy = context.FindEffectivePolicy<AuthorizeWithCompletedProfileAttribute>();
        if (effectiveAuthorizePolicy?.AllowedFor.IsNotTheSameAs(AllowedFor) ?? false)
        {
            await next();
            return;
        }

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
        if (!AllowedFor.Exists(allowedRole => userRoles.Any(role => role == allowedRole)))
        {
            throw new UnauthorizedAccessException();
        }

        if (AccessPolicy is not null)
        {
            var canAccessChecks = context.HttpContext.RequestServices.GetServices(AccessPolicy).Cast<IAccessPolicy>();

            foreach (var canAccessCheck in canAccessChecks)
            {
                if (!await canAccessCheck.CanAccess(AllowedFor))
                {
                    throw new UnauthorizedAccessException();
                }
            }
        }

        await next();
    }
}
