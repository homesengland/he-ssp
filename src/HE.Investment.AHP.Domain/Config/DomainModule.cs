using HE.Investment.AHP.Contract.HomeTypes;
using HE.Investment.AHP.Domain.HomeTypes;
using HE.Investment.AHP.Domain.HomeTypes.Mappers;
using Microsoft.Extensions.DependencyInjection;

namespace HE.Investment.AHP.Domain.Config;

public static class DomainModule
{
    public static void AddDomainModule(this IServiceCollection services)
    {
        AddHomeTypes(services);
    }

    private static void AddHomeTypes(IServiceCollection services)
    {
        services.AddSingleton<IHomeTypeRepository, HomeTypeRepository>();
        services.AddSingleton<IHomeTypeSectionMapper<HousingTypeSection>, HousingTypeSectionMapper>();
    }
}
