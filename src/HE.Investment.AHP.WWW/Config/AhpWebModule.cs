using HE.Investment.AHP.Domain.Config;
using HE.Investment.AHP.Domain.HomeTypes.CommandHandlers;
using HE.Investment.AHP.WWW.Notifications;
using HE.InvestmentLoans.Common.Infrastructure;
using HE.InvestmentLoans.Common.Models.App;
using HE.Investments.Common.Config;
using HE.Investments.Common.Infrastructure.Events;
using HE.Investments.Common.WWW.Infrastructure.Authorization;
using HE.Investments.Organisation.Config;

namespace HE.Investment.AHP.WWW.Config;

public static class AhpWebModule
{
    public static void AddWebModule(this IServiceCollection service, IConfiguration configuration)
    {
        service.AddOrganisationCrmModule();
        service.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(SaveHomeTypeDetailsCommandHandler).Assembly));
        service.AddScoped<NonceModel>();

        AddConfiguration(service, configuration);
        service.AddHttpUserContext();
        service.AddDomainModule();
        service.AddEventInfrastructure();
        service.AddNotifications(typeof(HomeTypeHasBeenCreatedDisplayNotificationFactory).Assembly);
    }

    private static void AddConfiguration(IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IAhpAppConfig, AhpAppConfig>(x => x.GetRequiredService<IConfiguration>().GetSection("AppConfiguration").Get<AhpAppConfig>());
        services.Configure<ContactInfoOptions>(configuration.GetSection("AppConfiguration:ContactInfo"));
        services.AddSingleton<IDataverseConfig, DataverseConfig>(x =>
            x.GetRequiredService<IConfiguration>().GetSection("AppConfiguration:Dataverse").Get<DataverseConfig>());
    }
}
