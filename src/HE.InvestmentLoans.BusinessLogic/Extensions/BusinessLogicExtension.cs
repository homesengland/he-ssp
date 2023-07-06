using FluentValidation;
using FluentValidation.AspNetCore;
using HE.InvestmentLoans.BusinessLogic.Exceptions;
using HE.InvestmentLoans.BusinessLogic.Pipelines;
using HE.InvestmentLoans.BusinessLogic.Repositories;
using HE.InvestmentLoans.BusinessLogic.User.Repositories;
using HE.InvestmentLoans.Common.Authorization;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace HE.InvestmentLoans.BusinessLogic.Extensions;

public static class BusinessLogicExtension
{
    public static void AddBusinessLogic(this IServiceCollection services)
    {
        services.AddFluentValidationAutoValidation();
        services.AddValidatorsFromAssemblyContaining<NotFoundException>();
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestValidationBehavior<,>));
        services.AddScoped<ILoanApplicationRepository, LoanApplicationRepository>();
        services.AddScoped<ILoanUserRepository, LoanUserRepository>();
        services.AddScoped<ILoanUserContext, LoanUserContext>();

    }
}
