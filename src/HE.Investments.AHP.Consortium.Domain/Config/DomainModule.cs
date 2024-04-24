using Microsoft.Extensions.DependencyInjection;

namespace HE.Investments.AHP.Consortium.Domain.Config;

public static class DomainModule
{
    public static void AddConsortiumDomainModule(this IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(DomainModule).Assembly));
    }
}
