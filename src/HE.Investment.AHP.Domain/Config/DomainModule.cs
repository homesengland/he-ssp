using HE.Investment.AHP.Domain.Application.Repositories;
using HE.Investment.AHP.Domain.Data;
using HE.Investment.AHP.Domain.FinancialDetails.Repositories;
using HE.Investment.AHP.Domain.HomeTypes.Repositories;
using HE.Investment.AHP.Domain.HomeTypes.Services;
using HE.Investment.AHP.Domain.Scheme.Repositories;
using HE.InvestmentLoans.Common.Utils;
using HE.Investments.Account.Shared.Config;
using MediatR.Pipeline;
using Microsoft.Extensions.DependencyInjection;

namespace HE.Investment.AHP.Domain.Config;

public static class DomainModule
{
    public static void AddDomainModule(this IServiceCollection services)
    {
        services.AddAccountSharedModule(true);
        services.AddScoped<IDateTimeProvider, DateTimeProvider>();
        services.AddTransient(typeof(IRequestExceptionHandler<,,>), typeof(DomainValidationHandler<,,>));

        services.AddScoped<IApplicationCrmContext, ApplicationCrmContext>();

        AddHomeTypes(services);
        AddFinancialDetails(services);
        AddApplication(services);
        AddScheme(services);
    }

    private static void AddHomeTypes(IServiceCollection services)
    {
        services.AddScoped<IHomeTypeRepository, HomeTypeRepository>();

        // TODO: change service do scoped after introducing integration with IHttpDocumentService
        services.AddSingleton<IDesignFileService, DesignFileService>();
    }

    private static void AddFinancialDetails(IServiceCollection services)
    {
        services.AddScoped<IFinancialDetailsRepository, FinancialDetailsRepository>();
    }

    private static void AddApplication(IServiceCollection services)
    {
        services.AddScoped<IApplicationRepository, ApplicationRepository>();
    }

    private static void AddScheme(IServiceCollection services)
    {
        services.AddScoped<ISchemeRepository, SchemeRepository>();
    }
}
