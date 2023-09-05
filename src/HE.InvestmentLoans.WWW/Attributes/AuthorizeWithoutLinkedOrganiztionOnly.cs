using HE.InvestmentLoans.BusinessLogic.User;
using HE.InvestmentLoans.WWW.Controllers;
using HE.InvestmentLoans.WWW.Utils.ValueObjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace HE.InvestmentLoans.WWW.Attributes;

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
public class AuthorizeWithoutLinkedOrganiztionOnly : AuthorizeAttribute, IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var loanUserContext = context.HttpContext.RequestServices.GetRequiredService<ILoanUserContext>();

        if (await loanUserContext.IsLinkedWithOrganization())
        {
            context.Result = new RedirectToActionResult(
                nameof(HomeController.Index),
                new ControllerName(nameof(HomeController)).WithoutPrefix(),
                null);
            return;
        }

        await next();
    }
}
