using HE.Investments.Assessment.Domain.Config;
using HE.Investments.Common.Infrastructure.Events;
using HE.Investments.Common.Models.App;
using HE.Investments.Common.WWW.Infrastructure.Authorization;
using HE.Investments.Common.WWW.Infrastructure.ErrorHandling;
using HE.Investments.Common.WWW.Infrastructure.Middlewares;
using HE.Investments.FrontDoor.WWW.Routing;
using HE.Investments.Organisation.Config;

namespace HE.Investments.FrontDoor.WWW.Config;

public static class FrontDoorWebModule
{
    public static void AddWebModule(this IServiceCollection service, IConfiguration configuration)
    {
        service.AddOrganisationCrmModule();
        service.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(DomainModule).Assembly));
        service.AddScoped<NonceModel>();

        AddConfiguration(service, configuration);
        service.AddDomainModule();
        service.AddHttpUserContext();
        service.AddEventInfrastructure();
    }

    private static void AddConfiguration(IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IErrorViewPaths, FrontDoorErrorViewPaths>();
        services.AddSingleton<IFrontDoorAppConfig, FrontDoorAppConfig>(x => x.GetRequiredService<IConfiguration>().GetSection("AppConfiguration").Get<FrontDoorAppConfig>());
        services.Configure<ContactInfoOptions>(configuration.GetSection("AppConfiguration:ContactInfo"));
        services.AddSingleton<IDataverseConfig, DataverseConfig>(x =>
            x.GetRequiredService<IConfiguration>().GetSection("AppConfiguration:Dataverse").Get<DataverseConfig>());
    }
}
