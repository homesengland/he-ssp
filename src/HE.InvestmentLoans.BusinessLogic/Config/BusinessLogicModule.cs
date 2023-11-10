extern alias Org;

using FluentValidation;
using FluentValidation.AspNetCore;
using HE.InvestmentLoans.BusinessLogic.CompanyStructure.Repositories;
using HE.InvestmentLoans.BusinessLogic.Funding.Repositories;
using HE.InvestmentLoans.BusinessLogic.LoanApplication.Repositories;
using HE.InvestmentLoans.BusinessLogic.Projects.Repositories;
using HE.InvestmentLoans.BusinessLogic.Security.Repositories;
using HE.InvestmentLoans.BusinessLogic.User;
using HE.InvestmentLoans.BusinessLogic.User.Repositories;
using HE.InvestmentLoans.Common.Utils;
using HE.Investments.Account.Domain.Config;
using Microsoft.Extensions.DependencyInjection;
using Org::HE.Investments.Organisation.CrmRepository;
using ContactRepository = HE.InvestmentLoans.BusinessLogic.Organization.Repositories.ContactRepository;
using IContactRepository = HE.InvestmentLoans.BusinessLogic.Organization.Repositories.IContactRepository;

namespace HE.InvestmentLoans.BusinessLogic.Config;

public static class BusinessLogicModule
{
    public static void AddBusinessLogic(this IServiceCollection services)
    {
        services.AddAccountModule();
        services.AddFluentValidationAutoValidation();
        services.AddValidatorsFromAssemblyContaining<LoanApplicationRepository>();

        services.AddScoped<ILoanApplicationRepository, LoanApplicationRepository>();
        services.AddScoped<ICanSubmitLoanApplication, LoanApplicationRepository>();
        services.AddScoped<ILoanUserRepository, LoanUserRepository>();
        services.AddScoped<ILoanUserContext, LoanUserContext>();
        services.AddScoped<IApplicationProjectsRepository, ApplicationProjectsRepository>();
        services.AddScoped<ILocalAuthorityRepository, ApplicationProjectsRepository>();
        services.AddScoped<IDateTimeProvider, DateTimeProvider>();

        services.AddOrganizationSubmodule();
        services.AddSecuritySubmodule();
        services.AddCompanyStructureSubmodule();
        services.AddFundingSubmodule();
    }

    private static void AddOrganizationSubmodule(this IServiceCollection services)
    {
        services.AddScoped<IOrganizationRepository, OrganizationRepository>();
        services.AddScoped<IContactRepository, ContactRepository>();
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
