using HE.Investments.Account.Domain.Config;
using HE.Investments.Account.Domain.User.QueryHandlers;
using HE.Investments.Account.Shared.Routing;
using HE.Investments.Account.WWW.Notifications;
using HE.Investments.Account.WWW.Routing;
using HE.Investments.Account.WWW.Utils;
using HE.Investments.Common.Config;
using HE.Investments.Common.Contract.Enum;
using HE.Investments.Common.CRM;
using HE.Investments.Common.Extensions;
using HE.Investments.Common.Infrastructure.Events;
using HE.Investments.Common.Models.App;
using HE.Investments.Common.Utils;
using HE.Investments.Common.WWW.Config;
using HE.Investments.Common.WWW.Infrastructure.Authorization;
using HE.Investments.Common.WWW.Infrastructure.ErrorHandling;
using HE.Investments.Common.WWW.Infrastructure.Middlewares;

namespace HE.Investments.Account.WWW.Config;

public static class OrganisationWebModule
{
    public static void AddWebModule(this IServiceCollection services)
    {
        services.AddAppConfiguration<IMvcAppConfig, MvcAppConfig>();
        services.AddScoped<NonceModel>();
        AddConfiguration(services);
        AddCommonModule(services);
        services.AddHttpUserContext();
        services.AddCrmConnection();
        services.AddAccountModule();
        services.AddEventInfrastructure();
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(GetUserProfileInformationQueryHandler).Assembly));
        services.AddNotificationPublisher(ApplicationType.Account);
        services.AddNotificationConsumer(ApplicationType.Account, typeof(ChangeOrganisationDetailsRequestedDisplayNotificationFactory).Assembly);
        services.AddScoped<IAccountRoutes, AccountRoutes>();
        services.AddSingleton<IErrorViewPaths, AccountErrorViewPaths>();
    }

    private static void AddConfiguration(IServiceCollection services)
    {
        services.AddAppConfiguration<ProgrammeUrlConfig>();
        services.AddScoped<IProgrammes, Programmes>();
        services.AddAppConfiguration<IDataverseConfig, DataverseConfig>("Dataverse");
    }

    private static void AddCommonModule(IServiceCollection services)
    {
        services.AddScoped<IDateTimeProvider, DateTimeProvider>();
    }
}
