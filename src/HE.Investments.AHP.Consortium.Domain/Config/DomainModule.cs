extern alias Org;

using HE.Investments.AHP.Consortium.Domain.Crm;
using HE.Investments.AHP.Consortium.Domain.Repositories;
using Microsoft.Extensions.DependencyInjection;
using ServiceCollectionExtensions = Org::HE.Investments.Organisation.Config.ServiceCollectionExtensions;

namespace HE.Investments.AHP.Consortium.Domain.Config;

public static class DomainModule
{
    public static void AddConsortiumDomainModule(this IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(DomainModule).Assembly));
        services.AddScoped<IConsortiumCrmContext, ConsortiumCrmContext>();
        services.AddScoped<IConsortiumRepository, ConsortiumRepository>();
        services.AddScoped<IDraftConsortiumRepository, DraftConsortiumRepository>();
        ServiceCollectionExtensions.AddOrganisationCrmModule(services);
    }
}
