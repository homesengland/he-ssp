using FluentValidation;
using FluentValidation.AspNetCore;
using HE.InvestmentLoans.BusinessLogic.LoanApplication.ApplicationProject.Repositories;
using HE.InvestmentLoans.BusinessLogic.LoanApplication.Repositories;
using HE.InvestmentLoans.BusinessLogic.LoanApplicationLegacy.Pipelines;
using HE.InvestmentLoans.BusinessLogic.LoanApplicationLegacy.Validation;
using HE.InvestmentLoans.BusinessLogic.User;
using HE.InvestmentLoans.BusinessLogic.User.Repositories;
using HE.InvestmentLoans.Common.Utils;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace HE.InvestmentLoans.BusinessLogic.Config;

public static class BusinessLogicModule
{
    public static void AddBusinessLogic(this IServiceCollection services)
    {
        services.AddFluentValidationAutoValidation();
        services.AddValidatorsFromAssemblyContaining<LoanApplicationValidator>();
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestValidationBehavior<,>));
        services.AddScoped<ILoanApplicationRepository, LoanApplicationRepository>();
        services.AddScoped<ILoanUserRepository, LoanUserRepository>();
        services.AddScoped<ILoanUserContext, LoanUserContext>();
        services.AddScoped<IApplicationProjectsRepository, ApplicationProjectsRepository>();
        services.AddScoped<IDateTimeProvider, DateTimeProvider>();
    }
}
