using HE.Investment.AHP.Contract.HomeTypes;
using HE.Investment.AHP.Domain.HomeTypes.Mappers;
using HE.Investment.AHP.Domain.HomeTypes.Repositories;
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
        // TODO: change repository to scoped after introducing integration with CRM
        services.AddSingleton<IHomeTypeRepository, HomeTypeRepository>();
        services.AddSingleton<IHomeTypesRepository, HomeTypesRepository>();

        services.AddSingleton<IHomeTypeSectionMapper<HousingTypeSection>, HousingTypeSectionMapper>();
    }
}
