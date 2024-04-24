extern alias Org;

using Microsoft.Extensions.DependencyInjection;
using ServiceCollectionExtensions = Org::HE.Investments.Organisation.Config.ServiceCollectionExtensions;

namespace HE.Investments.AHP.Consortium.Domain.Config;

public static class DomainModule
{
    public static void AddConsortiumDomainModule(this IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(DomainModule).Assembly));
        ServiceCollectionExtensions.AddOrganizationsModule(services);
    }
}
