using HE.Investments.Account.Shared.Config;
using HE.Investments.Common;
using HE.Investments.Common.Utils;
using HE.Investments.FrontDoor.Domain.Programme.Crm;
using HE.Investments.FrontDoor.Domain.Programme.Repository;
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
        services.AddProjectCrmContext();
        services.AddScoped<IProjectRepository, ProjectRepository>();

        services.AddSiteCrmContext();
        services.AddScoped<ISiteRepository, SiteRepository>();
        services.AddScoped<IRemoveSiteRepository, SiteRepository>();
        services.AddSingleton<IProjectCrmMapper, ProjectCrmMapper>();
        services.AddEligibilityServiceWithStrategies();
        services.AddScoped<IProgrammeCrmContext, ProgrammeCrmContext>();
        services.AddScoped<IProgrammeRepository, ProgrammeRepository>();
        services.AddScoped<IProgrammeAvailabilityService, ProgrammeAvailabilityService>();

        services.AddTransient(typeof(IRequestExceptionHandler<,,>), typeof(DomainValidationHandler<,,>));
    }

    private static void AddProjectCrmContext(this IServiceCollection services)
    {
        services.AddScoped<IProjectCrmContext, ProjectCrmContext>();
        services.Decorate<IProjectCrmContext, RequestCacheProjectCrmContextDecorator>();
    }

    private static void AddSiteCrmContext(this IServiceCollection services)
    {
        services.AddScoped<ISiteCrmContext, SiteCrmContext>();
        services.Decorate<ISiteCrmContext, RequestCacheSiteCrmContextDecorator>();
    }

    private static void AddEligibilityServiceWithStrategies(this IServiceCollection services)
    {
        services.AddScoped<IProjectConversionStrategy, LoanApplicationConversionStrategy>();
        services.AddScoped<IProjectConversionStrategy, AhpProjectConversionStrategy>();
        services.AddScoped<IList<IProjectConversionStrategy>>(sp => sp.GetServices<IProjectConversionStrategy>().ToList());

        services.AddScoped<IEligibilityService, EligibilityService>();
    }
}
