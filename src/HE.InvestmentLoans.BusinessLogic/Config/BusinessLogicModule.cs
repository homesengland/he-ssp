using FluentValidation;
using FluentValidation.AspNetCore;
using HE.InvestmentLoans.BusinessLogic.CompanyStructure.Repositories;
using HE.InvestmentLoans.BusinessLogic.Funding.Repositories;
using HE.InvestmentLoans.BusinessLogic.LoanApplication.ApplicationProject.Repositories;
using HE.InvestmentLoans.BusinessLogic.LoanApplication.Repositories;
using HE.InvestmentLoans.BusinessLogic.LoanApplicationLegacy.Pipelines;
using HE.InvestmentLoans.BusinessLogic.Organization.Repositories;
using HE.InvestmentLoans.BusinessLogic.Security.Repositories;
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
        services.AddValidatorsFromAssemblyContaining<LoanApplicationRepository>();
        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(RequestValidationBehavior<,>));
        services.AddScoped<ILoanApplicationRepository, LoanApplicationRepository>();
        services.AddScoped<ICanSubmitLoanApplication, LoanApplicationRepository>();
        services.AddScoped<ILoanUserRepository, LoanUserRepository>();
        services.AddScoped<ILoanUserContext, LoanUserContext>();
        services.AddScoped<IApplicationProjectsRepository, ApplicationProjectsRepository>();
        services.AddScoped<IDateTimeProvider, DateTimeProvider>();

        services.AddOrganizationSubmodule();
        services.AddSecuritySubmodule();
        services.AddCompanyStructureSubmodule();
        services.AddFundingSubmodule();
    }

    private static void AddOrganizationSubmodule(this IServiceCollection services)
    {
        services.AddScoped<IOrganizationRepository, OrganizationRepository>();
    }

    private static void AddSecuritySubmodule(this IServiceCollection services)
    {
        services.AddScoped<ISecurityRepository, SecurityRepository>();
    }

    private static void AddCompanyStructureSubmodule(this IServiceCollection services)
    {
        services.AddScoped<ICompanyStructureRepository, CompanyStructureRepository>();
    }

    private static void AddFundingSubmodule(this IServiceCollection services)
    {
        services.AddScoped<IFundingRepository, FundingRepository>();
    }
}
