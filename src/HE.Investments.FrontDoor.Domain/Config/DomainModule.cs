using HE.Investments.Account.Shared.Config;
using HE.Investments.Common;
using HE.Investments.Common.Utils;
using HE.Investments.FrontDoor.Domain.Project.Crm;
using HE.Investments.FrontDoor.Domain.Project.Repository;
using HE.Investments.FrontDoor.Domain.Services;
using HE.Investments.FrontDoor.Domain.Site.Crm;
using HE.Investments.FrontDoor.Domain.Site.Repository;
using MediatR.Pipeline;
using Microsoft.Extensions.DependencyInjection;

namespace HE.Investments.FrontDoor.Domain.Config;

public static class DomainModule
{
    public static void AddDomainModule(this IServiceCollection services)
    {
        services.AddAccountSharedModule();
        services.AddScoped<IDateTimeProvider, DateTimeProvider>();
        services.AddProjectCrmContext();
        services.AddScoped<IProjectRepository, ProjectRepository>();

        services.AddSiteCrmContext();
        services.AddScoped<ISiteRepository, SiteRepository>();
        services.AddScoped<IRemoveSiteRepository, SiteRepository>();

        services.AddScoped<IEligibilityService, EligibilityService>();

        services.AddTransient(typeof(IRequestExceptionHandler<,,>), typeof(DomainValidationHandler<,,>));
    }

    private static void AddProjectCrmContext(this IServiceCollection services)
    {
        services.AddScoped<ProjectCrmContext>();
        services.AddScoped<IProjectCrmContext>(x => new CacheProjectCrmContext(x.GetRequiredService<ProjectCrmContext>()));
    }

    private static void AddSiteCrmContext(this IServiceCollection services)
    {
        services.AddScoped<SiteCrmContext>();
        services.AddScoped<ISiteCrmContext>(x => new CacheSiteCrmContext(x.GetRequiredService<SiteCrmContext>()));
    }
}
