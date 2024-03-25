extern alias Org;

using FluentValidation;
using FluentValidation.AspNetCore;
using HE.Investments.Account.Shared.Config;
using HE.Investments.Common;
using HE.Investments.Common.Utils;
using HE.Investments.FrontDoor.Shared.Config;
using HE.Investments.Loans.BusinessLogic.CompanyStructure;
using HE.Investments.Loans.BusinessLogic.CompanyStructure.Repositories;
using HE.Investments.Loans.BusinessLogic.Files;
using HE.Investments.Loans.BusinessLogic.Funding.Repositories;
using HE.Investments.Loans.BusinessLogic.LoanApplication;
using HE.Investments.Loans.BusinessLogic.LoanApplication.Repositories;
using HE.Investments.Loans.BusinessLogic.LoanApplication.ValueObjects;
using HE.Investments.Loans.BusinessLogic.PrefillData.Crm;
using HE.Investments.Loans.BusinessLogic.PrefillData.Repositories;
using HE.Investments.Loans.BusinessLogic.Projects.Repositories;
using HE.Investments.Loans.BusinessLogic.Security.Repositories;
using HE.Investments.Loans.Contract.Application.ValueObjects;
using MediatR.Pipeline;
using Microsoft.Extensions.DependencyInjection;
using Org::HE.Investments.Organisation.LocalAuthorities.Repositories;

namespace HE.Investments.Loans.BusinessLogic.Config;

public static class BusinessLogicModule
{
    public static void AddBusinessLogic(this IServiceCollection services)
    {
        services.AddAccountSharedModule();
        services.AddFrontDoorSharedModule();
        services.AddTransient(typeof(IRequestExceptionHandler<,,>), typeof(DomainValidationHandler<,,>));
        services.AddFluentValidationAutoValidation();
        services.AddScoped<ILocalAuthorityRepository, LocalAuthorityRepository>();
        services.AddScoped<IApplicationProjectsRepository, ApplicationProjectsRepository>();
        services.AddScoped<IDateTimeProvider, DateTimeProvider>();
        services.AddSingleton<ILoanAppConfig, LoanAppConfig>();

        services.AddFilesSupport();
        services.AddLoanApplicationRepositories();
        services.AddSecuritySubmodule();
        services.AddCompanyStructureSubmodule();
        services.AddFundingSubmodule();
        services.AddPrefillDataSubmodule();
    }

    private static void AddLoanApplicationRepositories(this IServiceCollection services)
    {
        services.AddValidatorsFromAssemblyContaining<LoanApplicationRepository>();
        services.AddScoped<ILoanApplicationRepository, LoanApplicationRepository>();
        services.AddScoped<ICanSubmitLoanApplication, LoanApplicationRepository>();
        services.AddScoped<IChangeApplicationStatus, LoanApplicationRepository>();
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

    private static void AddFilesSupport(this IServiceCollection services)
    {
        services.AddSingleton<ILoansDocumentSettings, LoansDocumentSettings>();
        services.AddScoped<ILoansFileLocationProvider<SupportingDocumentsParams>>(x => x.GetRequiredService<ILoanApplicationRepository>());
        services.AddScoped<ILoansFileService<SupportingDocumentsParams>, LoansFileService<SupportingDocumentsParams>>();
        services.AddSingleton<ISupportingDocumentsFileFactory, SupportingDocumentsFileFactory>();
        services.AddScoped<IFileApplicationRepository, FileApplicationRepository>();
    }

    private static void AddFundingSubmodule(this IServiceCollection services)
    {
        services.AddScoped<IFundingRepository, FundingRepository>();
    }

    private static void AddPrefillDataSubmodule(this IServiceCollection services)
    {
        services.AddScoped<ILoanPrefillDataRepository, LoanPrefillDataRepository>();
        services.AddScoped<ILoanPrefillDataCrmContext, LoanPrefillDataCrmContext>();
    }
}
