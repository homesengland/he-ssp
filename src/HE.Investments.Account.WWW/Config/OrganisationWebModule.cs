using HE.Investments.Account.Domain.Config;
using HE.Investments.Account.Domain.User.QueryHandlers;
using HE.Investments.Account.Shared.Routing;
using HE.Investments.Account.WWW.Notifications;
using HE.Investments.Account.WWW.Routing;
using HE.Investments.Account.WWW.Utils;
using HE.Investments.Common.Config;
using HE.Investments.Common.CRM;
using HE.Investments.Common.CRM.Config;
using HE.Investments.Common.Infrastructure.Events;
using HE.Investments.Common.Utils;
using HE.Investments.Common.WWW.Infrastructure.Authorization;
using HE.Investments.Common.WWW.Infrastructure.ErrorHandling;
using HE.Investments.Loans.Common.Infrastructure;

namespace HE.Investments.Account.WWW.Config;

public static class OrganisationWebModule
{
    public static void AddWebModule(this IServiceCollection services)
    {
        services.AddScoped<NonceModel>();
        AddConfiguration(services);
        AddCommonModule(services);
        services.AddHttpUserContext();
        services.AddCrmConnection();
        services.AddAccountModule();
        services.AddEventInfrastructure();
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(GetUserProfileInformationQueryHandler).Assembly));
        services.AddNotifications("Account", typeof(ChangeOrganisationDetailsRequestedDisplayNotificationFactory).Assembly);
        services.AddScoped<IAccountRoutes, AccountRoutes>();
        services.AddSingleton<IErrorViewPaths, AccountErrorViewPaths>();
    }

    private static void AddConfiguration(IServiceCollection services)
    {
        services.AddSingleton<IOrganisationAppConfig, OrganisationAppConfig>(x => x.GetRequiredService<IConfiguration>().GetSection("AppConfiguration").Get<OrganisationAppConfig>());
        services.AddSingleton(x => x.GetRequiredService<IConfiguration>().GetSection("AppConfiguration").Get<ProgrammeUrlConfig>());
        services.AddScoped<IProgrammes, Programmes>();
        services.AddSingleton<IDataverseConfig, DataverseConfig>(x => x.GetRequiredService<IConfiguration>().GetSection("AppConfiguration:Dataverse").Get<DataverseConfig>());
    }

    private static void AddCommonModule(IServiceCollection services)
    {
        services.AddScoped<IDateTimeProvider, DateTimeProvider>();
    }
}
