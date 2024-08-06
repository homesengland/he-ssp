using HE.Investments.AHP.Allocation.Domain.Allocation.Crm;
using HE.Investments.AHP.Allocation.Domain.Allocation.Mappers;
using HE.Investments.AHP.Allocation.Domain.Allocation.Repositories;
using HE.Investments.AHP.Allocation.Domain.Claims.Crm;
using HE.Investments.AHP.Allocation.Domain.Claims.Mappers;
using HE.Investments.AHP.Allocation.Domain.Claims.Repositories;
using HE.Investments.AHP.Allocation.Domain.Project.Crm;
using HE.Investments.AHP.Allocation.Domain.Project.Repositories;
using HE.Investments.AHP.Allocation.Domain.Site.Crm;
using HE.Investments.AHP.Allocation.Domain.Site.Repositories;
using HE.Investments.AHP.Allocation.Domain.UserContext;
using Microsoft.Extensions.DependencyInjection;

namespace HE.Investments.AHP.Allocation.Domain.Config;

public static class DomainModule
{
    public static void AddAllocationDomainModule(this IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(DomainModule).Assembly));
        services.AddCrmContexts();
        services.AddRepositories();
        services.AddMappers();
    }

    private static void AddMappers(this IServiceCollection services)
    {
        services.AddScoped<IPhaseCrmMapper, PhaseCrmMapper>();
        services.AddScoped<IAllocationBasicInfoMapper, AllocationBasicInfoMapper>();
        services.AddScoped<IAllocationContractMapper, AllocationContractMapper>();
        services.AddScoped<IPhaseContractMapper, PhaseContractMapper>();
        services.AddSingleton<IMilestoneClaimContractMapper, MilestoneClaimContractMapper>();
        services.AddSingleton<MilestoneStatusMapper>();
        services.AddSingleton<MilestoneTypeMapper>();
    }

    private static void AddCrmContexts(this IServiceCollection services)
    {
        services.AddScoped<IAllocationCrmContext, AllocationCrmContext>();
        services.Decorate<IAllocationCrmContext, RequestCacheAllocationCrmContextDecorator>();
        services.AddScoped<ISiteAllocationCrmContext, SiteAllocationCrmContext>();
        services.AddScoped<IProjectAllocationCrmContext, ProjectAllocationCrmContext>();
        services.Decorate<IProjectAllocationCrmContext, RequestCacheProjectAllocationCrmContextDecorator>();
        services.AddScoped<IAllocationAccessContext, AllocationAccessContext>();
    }

    private static void AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IAllocationRepository, AllocationRepository>();
        services.AddScoped<IPhaseRepository, PhaseRepository>();
        services.AddScoped<ISiteAllocationRepository, SiteAllocationRepository>();
        services.AddScoped<IProjectAllocationRepository, ProjectAllocationRepository>();
    }
}
