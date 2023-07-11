using FluentValidation;
using FluentValidation.AspNetCore;
using HE.InvestmentLoans.BusinessLogic.Application.Repositories;
using HE.InvestmentLoans.BusinessLogic.User.Repositories;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using HE.InvestmentLoans.BusinessLogic._LoanApplication.Validation;
using HE.InvestmentLoans.BusinessLogic._LoanApplication.Pipelines;
using HE.InvestmentLoans.BusinessLogic.User;

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

    }
}
