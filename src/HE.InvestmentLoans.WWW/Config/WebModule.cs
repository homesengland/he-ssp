using FluentValidation;
using HE.InvestmentLoans.BusinessLogic.Config;
using HE.InvestmentLoans.BusinessLogic.ViewModel;
using HE.InvestmentLoans.Common.Authorization;
using HE.InvestmentLoans.CRM.Extensions;
using HE.InvestmentLoans.WWW.Models;
using HE.Investments.Organisation.CompaniesHouse;
using HE.Investments.Organisation.Services;

namespace HE.InvestmentLoans.WWW.Config;

public static class WebModule
{
    public static void AddWebModule(this IServiceCollection serviceCollections)
    {
        serviceCollections.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(LoanApplicationViewModel).Assembly));

        serviceCollections.AddScoped<NonceModel>();
        serviceCollections.AddBusinessLogic();
        serviceCollections.AddCrmConnection();
        serviceCollections.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        serviceCollections.AddScoped<IUserContext, UserContext>(x => new UserContext(x.GetRequiredService<IHttpContextAccessor>()!.HttpContext!));
        serviceCollections.AddValidatorsFromAssemblyContaining<LoanPurposeModel>();
        serviceCollections.AddCompaniesHouseHttpClient();
        serviceCollections.AddScoped<IOrganisationSearchService, OrganisationSearchService>();
    }
}
