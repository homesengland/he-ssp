using FluentValidation;
using FluentValidation.AspNetCore;
using HE.InvestmentLoans.BusinessLogic.Application.Repositories;
using HE.InvestmentLoans.BusinessLogic.User.Repositories;
using HE.InvestmentLoans.Common.Authorization;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using HE.InvestmentLoans.BusinessLogic._LoanApplication.Validation;
using HE.InvestmentLoans.BusinessLogic._LoanApplication.Pipelines;
using HE.InvestmentLoans.BusinessLogic.Application.Project.Repositories;

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
    }
}
