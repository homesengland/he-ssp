using HE.Investments.Programme.Domain.Crm;
using HE.Investments.Programme.Domain.Mappers;
using HE.Investments.Programme.Domain.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace HE.Investments.Programme.Domain.Config;

public static class DomainModule
{
    public static void AddProgrammeSubdomainModule(this IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(DomainModule).Assembly));
        services.AddScoped<IProgrammeRepository, ProgrammeRepository>();
        services.AddScoped<IProgrammeCrmContext, ProgrammeCrmContext>();
        services.Decorate<IProgrammeCrmContext, CacheProgrammeCrmContextDecorator>();
        services.Decorate<IProgrammeCrmContext, RequestCacheProgrammeCrmContextDecorator>();
        services.AddScoped<IProgrammeMapper, ProgrammeMapper>();
    }
}
