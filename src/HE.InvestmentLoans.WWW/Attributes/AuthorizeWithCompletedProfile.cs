using Auth0.ManagementApi.Models;
using HE.InvestmentLoans.BusinessLogic;
using HE.InvestmentLoans.BusinessLogic.User;
using HE.InvestmentLoans.WWW.Controllers;
using HE.InvestmentLoans.WWW.Utils.ValueObjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.FeatureManagement;

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
        var featureManager = context.HttpContext.RequestServices.GetRequiredService<IFeatureManager>();

        if (await featureManager.IsEnabledAsync(LoansFeatureFlags.DisplayProfileDetailsPage))
        {
            var loanUserContext = context.HttpContext.RequestServices.GetRequiredService<ILoanUserContext>();

            var isProfileComplete = await loanUserContext.IsProfileCompleted();

            if (!isProfileComplete)
            {
                context.Result = new RedirectToActionResult(
                    nameof(UserController.ProfileDetails),
                    new ControllerName(nameof(UserController)).WithoutPrefix(),
                    null);
                return;
            }

            if (!await loanUserContext.IsLinkedWithOrganization())
            {
                context.Result = new RedirectToActionResult(
                    nameof(OrganizationController.SearchOrganization),
                    new ControllerName(nameof(OrganizationController)).WithoutPrefix(),
                    null);
                return;
            }
        }

        await next();
    }
}
