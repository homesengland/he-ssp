using HE.Investment.AHP.Domain.FinancialDetails.Repositories;
using HE.Investment.AHP.Domain.HomeTypes.Repositories;
using HE.Investment.AHP.Domain.Scheme;
using HE.InvestmentLoans.Common.Utils;
using MediatR.Pipeline;
using Microsoft.Extensions.DependencyInjection;

namespace HE.Investment.AHP.Domain.Config;

public static class DomainModule
{
    public static void AddDomainModule(this IServiceCollection services)
    {
        services.AddScoped<IDateTimeProvider, DateTimeProvider>();
        services.AddTransient(typeof(IRequestExceptionHandler<,,>), typeof(DomainValidationHandler<,,>));

        AddHomeTypes(services);
        AddFinancialDetails(services);
        AddScheme(services);
    }

    private static void AddHomeTypes(IServiceCollection services)
    {
        // TODO: change repository to scoped after introducing integration with CRM
        services.AddSingleton<IHomeTypeRepository, HomeTypeRepository>();
        services.AddSingleton<IHomeTypesRepository, HomeTypesRepository>();
    }

    private static void AddFinancialDetails(IServiceCollection services)
    {
        services.AddScoped<IFinancialDetailsRepository, FinancialDetailsRepository>();
    }

    private static void AddScheme(IServiceCollection services)
    {
        services.AddSingleton<ISchemeRepository, SchemeRepository>();
    }
}
