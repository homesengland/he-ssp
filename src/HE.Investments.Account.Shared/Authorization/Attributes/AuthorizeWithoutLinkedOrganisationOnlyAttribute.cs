using HE.Investments.Account.Shared.Routing;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;

namespace HE.Investments.Account.Shared.Authorization.Attributes;

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
public class AuthorizeWithoutLinkedOrganisationOnlyAttribute : AuthorizeAttribute, IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var accountUserContext = context.HttpContext.RequestServices.GetRequiredService<IAccountUserContext>();
        var accountRoutes = context.HttpContext.RequestServices.GetRequiredService<IAccountRoutes>();

        if (!accountUserContext.IsLogged)
        {
            context.Result = accountRoutes.LandingPageForNotLoggedUser();
        }

        if (!await accountUserContext.IsProfileCompleted())
        {
            context.Result = accountRoutes.NotCompleteProfile();
            return;
        }

        if (!await accountUserContext.IsLinkedWithOrganisation())
        {
            await next();
            return;
        }

        context.Result = accountRoutes.LandingPageForLoggedUser();
    }
}
