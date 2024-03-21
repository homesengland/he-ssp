using HE.Investments.Common;
using HE.Investments.Common.Config;
using HE.Investments.Common.Infrastructure.Events;
using HE.Investments.Common.Models.App;
using HE.Investments.Common.Services.Notifications;
using HE.Investments.Common.WWW.Infrastructure.Authorization;
using HE.Investments.Common.WWW.Infrastructure.ErrorHandling;
using HE.Investments.Common.WWW.Infrastructure.Middlewares;
using HE.Investments.FrontDoor.Domain.Config;
using HE.Investments.FrontDoor.WWW.Models.Factories;
using HE.Investments.FrontDoor.WWW.Routing;
using HE.Investments.Organisation.Config;
using HE.Investments.Organisation.LocalAuthorities.Repositories;

namespace HE.Investments.FrontDoor.WWW.Config;

public static class FrontDoorWebModule
{
    public static void AddWebModule(this IServiceCollection service, IConfiguration configuration)
    {
        service.AddOrganisationCrmModule();
        service.AddScoped<ILocalAuthorityRepository, LocalAuthorityRepository>();

        service.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(DomainModule).Assembly));
        service.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(DomainValidationHandler<,,>).Assembly));
        service.AddScoped<NonceModel>();
        service.AddNotificationPublisher(ApplicationType.FrontDoor);

        AddConfiguration(service, configuration);
        service.AddDomainModule();
        service.AddHttpUserContext();
        service.AddEventInfrastructure();
    }

    private static void AddConfiguration(IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IExternalLinks, ExternalLinks>();
        services.AddSingleton<IErrorViewPaths, FrontDoorErrorViewPaths>();
        services.AddSingleton<IFrontDoorAppConfig, FrontDoorAppConfig>(x => x.GetRequiredService<IConfiguration>().GetSection("AppConfiguration").Get<FrontDoorAppConfig>());
        services.Configure<ContactInfoOptions>(configuration.GetSection("AppConfiguration:ContactInfo"));
        services.AddSingleton<IDataverseConfig, DataverseConfig>(x =>
            x.GetRequiredService<IConfiguration>().GetSection("AppConfiguration:Dataverse").Get<DataverseConfig>());
        services.AddScoped<IProjectSummaryViewModelFactory, ProjectSummaryViewModelFactory>();
        services.AddSingleton(x => x.GetRequiredService<IConfiguration>().GetSection("AppConfiguration:LoanApplicationService").Get<LoanApplicationConfig>());
    }
}
