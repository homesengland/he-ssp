using HE.Investment.AHP.DataLayer.Repositories;
using HE.Investment.AHP.Domain.Application.Repositories;
using HE.Investment.AHP.Domain.Scheme.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace HE.Investment.AHP.DataLayer.Config;

public static class DataLayerModule
{
    public static void AddDataLayerModule(this IServiceCollection services)
    {
        AddRepositories(services);
    }

    private static void AddRepositories(IServiceCollection services)
    {
        services.AddSingleton<IApplicationRepository, ApplicationRepository>();
        services.AddSingleton<ISchemeRepository, SchemeRepository>();
    }
}
