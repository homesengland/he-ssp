using HE.Investments.AHP.ProjectDashboard.Domain.Project.Crm;
using HE.Investments.AHP.ProjectDashboard.Domain.Project.Repositories;
using HE.Investments.AHP.ProjectDashboard.Domain.Site.Crm;
using HE.Investments.AHP.ProjectDashboard.Domain.Site.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace HE.Investments.AHP.ProjectDashboard.Domain.Config;

public static class DomainModule
{
    public static void AddProjectDashboardDomainModule(this IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(DomainModule).Assembly));
        services.AddCrmContexts();
        services.AddRepositories();
    }

    private static void AddCrmContexts(this IServiceCollection services)
    {
        services.AddScoped<IProjectCrmContext, ProjectCrmContext>();
        services.Decorate<IProjectCrmContext, RequestCacheProjectCrmContextDecorator>();
        services.AddScoped<ISiteAllocationCrmContext, SiteAllocationCrmContext>();
        services.AddScoped<IProjectAllocationCrmContext, ProjectAllocationCrmContext>();
        services.Decorate<IProjectAllocationCrmContext, RequestCacheProjectAllocationCrmContextDecorator>();
    }

    private static void AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IProjectRepository, ProjectRepository>();
        services.AddScoped<ISiteAllocationRepository, SiteAllocationRepository>();
        services.AddScoped<IProjectAllocationRepository, ProjectAllocationRepository>();
    }
}
