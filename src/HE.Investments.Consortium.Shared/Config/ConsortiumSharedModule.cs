using HE.Investments.Consortium.Shared.Repositories;
using HE.Investments.Consortium.Shared.UserContext;
using Microsoft.Extensions.DependencyInjection;

namespace HE.Investments.Consortium.Shared.Config;

public static class ConsortiumSharedModule
{
    public static void AddConsortiumSharedModule(this IServiceCollection services)
    {
        services.AddScoped<IConsortiumUserContext, ConsortiumUserContext>();
        services.AddScoped<IConsortiumAccessContext, ConsortiumAccessContext>();
        services.AddScoped<IConsortiumRepository, ConsortiumCrmRepository>();
    }
}
