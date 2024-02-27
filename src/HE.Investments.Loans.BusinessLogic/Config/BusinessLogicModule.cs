extern alias Org;

using FluentValidation;
using FluentValidation.AspNetCore;
using HE.Investments.Account.Shared.Config;
using HE.Investments.Common.Utils;
using HE.Investments.Loans.BusinessLogic.CompanyStructure;
using HE.Investments.Loans.BusinessLogic.CompanyStructure.Repositories;
using HE.Investments.Loans.BusinessLogic.Files;
using HE.Investments.Loans.BusinessLogic.Funding.Repositories;
using HE.Investments.Loans.BusinessLogic.LoanApplication.Repositories;
using HE.Investments.Loans.BusinessLogic.Projects.Repositories;
using HE.Investments.Loans.BusinessLogic.Security.Repositories;
using HE.Investments.Loans.Contract.Application.ValueObjects;
using Microsoft.Extensions.DependencyInjection;
using Org::HE.Investments.Organisation.LocalAuthorities.Repositories;

namespace HE.Investments.Loans.BusinessLogic.Config;

public static class BusinessLogicModule
{
    public static void AddBusinessLogic(this IServiceCollection services)
    {
        services.AddAccountSharedModule();
        services.AddFluentValidationAutoValidation();
        services.AddValidatorsFromAssemblyContaining<LoanApplicationRepository>();
        services.AddScoped<ILoanApplicationRepository, LoanApplicationRepository>();
        services.AddScoped<ICanSubmitLoanApplication, LoanApplicationRepository>();
        services.AddScoped<IApplicationProjectsRepository, ApplicationProjectsRepository>();
        services.AddScoped<ILocalAuthorityRepository, LocalAuthorityRepository>();
        services.AddScoped<IDateTimeProvider, DateTimeProvider>();
        services.AddSingleton<ILoansDocumentSettings, LoansDocumentSettings>();
        services.AddScoped<IFileApplicationRepository, FileApplicationRepository>();

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
        services.AddScoped<ILoansFileLocationProvider<LoanApplicationId>>(x => x.GetRequiredService<ICompanyStructureRepository>());
        services.AddScoped<ILoansFileService<LoanApplicationId>, LoansFileService<LoanApplicationId>>();
        services.AddSingleton<ICompanyStructureFileFactory, CompanyStructureFileFactory>();
    }

    private static void AddFundingSubmodule(this IServiceCollection services)
    {
        services.AddScoped<IFundingRepository, FundingRepository>();
    }
}
