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

        if (accountUserContext.IsLogged is false)
        {
            context.Result = accountRoutes.LandingPageForNotLoggedUser();
        }

        if (await accountUserContext.IsLinkedWithOrganisation() is false)
        {
            await next();
            return;
        }

        if (await accountUserContext.IsProfileCompleted() is false)
        {
            context.Result = accountRoutes.NotCompleteProfile();
            return;
        }

        context.Result = accountRoutes.LandingPageForLoggedUser();
    }
}
