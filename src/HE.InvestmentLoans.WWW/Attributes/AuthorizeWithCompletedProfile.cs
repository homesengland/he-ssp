using HE.InvestmentLoans.BusinessLogic.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace HE.InvestmentLoans.WWW.Attributes;

[AttributeUsage(AttributeTargets.All)]
public class AuthorizeWithCompletedProfile : AuthorizeAttribute, IAsyncActionFilter
{
    public AuthorizeWithCompletedProfile()
        : base()
    {
    }

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var loanUserContext = context.HttpContext.RequestServices.GetRequiredService<ILoanUserContext>();

        var isProfileComplete = await loanUserContext.IsProfileCompleted();

        if (!isProfileComplete)
        {
            context.Result = new RedirectToActionResult(
                "ProfileDetails",
                "User",
                null);
            return;
        }

        await next();
    }
}
