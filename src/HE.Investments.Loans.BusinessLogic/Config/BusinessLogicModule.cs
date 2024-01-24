extern alias Org;

using FluentValidation;
using FluentValidation.AspNetCore;
using HE.Investments.Account.Domain.Config;
using HE.Investments.Common.Utils;
using HE.Investments.Loans.BusinessLogic.CompanyStructure.Repositories;
using HE.Investments.Loans.BusinessLogic.Funding.Repositories;
using HE.Investments.Loans.BusinessLogic.LoanApplication.Repositories;
using HE.Investments.Loans.BusinessLogic.Projects.Repositories;
using HE.Investments.Loans.BusinessLogic.Security.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Org::HE.Investments.Organisation.LocalAuthorities.Repositories;

namespace HE.Investments.Loans.BusinessLogic.Config;

public static class BusinessLogicModule
{
    public static void AddBusinessLogic(this IServiceCollection services)
    {
        services.AddAccountModule();
        services.AddFluentValidationAutoValidation();
        services.AddValidatorsFromAssemblyContaining<LoanApplicationRepository>();

        services.AddScoped<ILoanApplicationRepository, LoanApplicationRepository>();
        services.AddScoped<ICanSubmitLoanApplication, LoanApplicationRepository>();
        services.AddScoped<IApplicationProjectsRepository, ApplicationProjectsRepository>();
        services.AddScoped<ILocalAuthorityRepository, LocalAuthorityRepository>();
        services.AddScoped<IDateTimeProvider, DateTimeProvider>();
        services.AddSingleton<ILoansDocumentSettings, LoansDocumentSettings>();

        services.AddSecuritySubmodule();
        services.AddCompanyStructureSubmodule();
        services.AddFundingSubmodule();
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
