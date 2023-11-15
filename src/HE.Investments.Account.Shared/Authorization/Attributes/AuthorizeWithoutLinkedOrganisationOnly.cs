using HE.Investments.Account.Shared.Routing;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;

namespace HE.Investments.Account.Shared.Authorization.Attributes;

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
public class AuthorizeWithoutLinkedOrganisationOnly : AuthorizeAttribute, IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var accountUserContext = context.HttpContext.RequestServices.GetRequiredService<IAccountUserContext>();
        var accountRoutes = context.HttpContext.RequestServices.GetRequiredService<IAccountRoutes>();

        if (!await accountUserContext.IsLinkedWithOrganisation())
        {
            await next();
            return;
        }

        if (accountUserContext.IsLogged is false)
        {
            context.Result = accountRoutes.NotLoggedUser();
        }

        if (await accountUserContext.IsProfileCompleted() is false)
        {
            context.Result = accountRoutes.NotCompleteProfile();
            return;
        }

        if (await accountUserContext.IsLinkedWithOrganisation() is false)
        {
            context.Result = accountRoutes.NotLinkedOrganisation();
            return;
        }

        context.Result = accountRoutes.LandingPageForLoggedUser();
    }
}
