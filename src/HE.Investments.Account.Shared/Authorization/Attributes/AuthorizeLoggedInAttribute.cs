using HE.Investments.Account.Shared.Routing;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;

namespace HE.Investments.Account.Shared.Authorization.Attributes;

[AttributeUsage(AttributeTargets.All)]
public class AuthorizeLoggedInAttribute : AuthorizeAttribute, IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var accountUserContext = context.HttpContext.RequestServices.GetRequiredService<IAccountUserContext>();
        var accountRoutes = context.HttpContext.RequestServices.GetRequiredService<IAccountRoutes>();

        if (!accountUserContext.IsLogged)
        {
            context.Result = accountRoutes.LandingPageForNotLoggedUser();
            return;
        }

        await accountUserContext.RefreshUserData();
        await next();
    }
}
