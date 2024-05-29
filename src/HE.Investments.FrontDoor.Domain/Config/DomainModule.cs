using HE.Investments.Account.Shared.Config;
using HE.Investments.Common;
using HE.Investments.Common.Extensions;
using HE.Investments.Common.Utils;
using HE.Investments.FrontDoor.Domain.Project.Crm;
using HE.Investments.FrontDoor.Domain.Project.Crm.Mappers;
using HE.Investments.FrontDoor.Domain.Project.Repository;
using HE.Investments.FrontDoor.Domain.Services;
using HE.Investments.FrontDoor.Domain.Services.Strategies;
using HE.Investments.FrontDoor.Domain.Site.Crm;
using HE.Investments.FrontDoor.Domain.Site.Repository;
using HE.Investments.FrontDoor.Shared.Config;
using MediatR.Pipeline;
using Microsoft.Extensions.DependencyInjection;

namespace HE.Investments.FrontDoor.Domain.Config;

public static class DomainModule
{
    public static void AddDomainModule(this IServiceCollection services)
    {
        services.AddFrontDoorSharedModule();
        services.AddAccountSharedModule();
        services.AddScoped<IDateTimeProvider, DateTimeProvider>();
        services.AddTransient(typeof(IRequestExceptionHandler<,,>), typeof(DomainValidationHandler<,,>));

        services
            .AddProject()
            .AddSite()
            .AddEligibilityServiceWithStrategies()
            .AddProgramme();
    }

    private static IServiceCollection AddProject(this IServiceCollection services)
    {
        services.AddScoped<IProjectCrmContext, ProjectCrmContext>();
        services.Decorate<IProjectCrmContext, RequestCacheProjectCrmContextDecorator>();
        services.AddScoped<IProjectRepository, ProjectRepository>();
        services.AddSingleton<IProjectCrmMapper, ProjectCrmMapper>();

        return services;
    }

    private static IServiceCollection AddSite(this IServiceCollection services)
    {
        services.AddScoped<ISiteCrmContext, SiteCrmContext>();
        services.Decorate<ISiteCrmContext, RequestCacheSiteCrmContextDecorator>();
        services.AddScoped<ISiteRepository, SiteRepository>();
        services.AddScoped<IRemoveSiteRepository, SiteRepository>();

        return services;
    }

    private static IServiceCollection AddEligibilityServiceWithStrategies(this IServiceCollection services)
    {
        services.AddScoped<IProjectConversionStrategy, LoanApplicationConversionStrategy>();
        services.AddScoped<IProjectConversionStrategy, AhpProjectConversionStrategy>();
        services.AddScoped<IList<IProjectConversionStrategy>>(sp => sp.GetServices<IProjectConversionStrategy>().ToList());

        services.AddScoped<IEligibilityService, EligibilityService>();

        return services;
    }

    private static void AddProgramme(this IServiceCollection services)
    {
        services.AddScoped<IProgrammeAvailabilityService, ProgrammeAvailabilityService>();
        services.AddAppConfiguration<IProgrammeSettings, ProgrammeSettings>();
    }
}
