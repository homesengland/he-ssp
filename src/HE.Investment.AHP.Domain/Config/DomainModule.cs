using HE.Investment.AHP.Contract.HomeTypes;
using HE.Investment.AHP.Domain.FinancialDetails.Repositories;
using HE.Investment.AHP.Domain.HomeTypes;
using HE.Investment.AHP.Domain.HomeTypes.Mappers;
using HE.InvestmentLoans.BusinessLogic.Projects.Repositories;
using HE.InvestmentLoans.Common.Utils;
using Microsoft.Extensions.DependencyInjection;

namespace HE.Investment.AHP.Domain.Config;

public static class DomainModule
{
    public static void AddDomainModule(this IServiceCollection services)
    {
        services.AddScoped<IDateTimeProvider, DateTimeProvider>();

        AddHomeTypes(services);
        AddFinancialDetails(services);
    }

    private static void AddHomeTypes(IServiceCollection services)
    {
        services.AddSingleton<IHomeTypeRepository, HomeTypeRepository>();
        services.AddSingleton<IHomeTypeSectionMapper<HousingTypeSection>, HousingTypeSectionMapper>();
    }

    private static void AddFinancialDetails(IServiceCollection services)
    {
        services.AddScoped<IFinancialDetailsRepository, FinancialDetailsRepository>();
    }
}
