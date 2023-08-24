using HE.InvestmentLoans.BusinessLogic.User;
using HE.InvestmentLoans.Common.Utils.Constants.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace HE.InvestmentLoans.WWW.Attributes;

[AttributeUsage(AttributeTargets.All)]
public class CheckProfileCompletionAttribute : Attribute, IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var loanUserContext = context.HttpContext.RequestServices.GetRequiredService<ILoanUserContext>();

        var selectedAccount = await loanUserContext.GetSelectedAccount();

        var profileCompletionStatusFromCache = loanUserContext.GetUserDetailsStatusFromCache(selectedAccount.UserGlobalId);

        if (profileCompletionStatusFromCache != ProfileCompletionStatus.Complete)
        {
            var isProfileComplete = await loanUserContext.IsProfileCompleted();

            if (!isProfileComplete)
            {
                context.Result = new RedirectToActionResult(
                    "ProfileDetails",
                    "Register",
                    null);
                return;
            }
        }

        await next();
    }
}
