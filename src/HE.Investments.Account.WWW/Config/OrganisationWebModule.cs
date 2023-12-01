using HE.Investments.Account.Domain.Config;
using HE.Investments.Account.Domain.User.QueryHandlers;
using HE.Investments.Account.Shared.Routing;
using HE.Investments.Account.WWW.Middlewares;
using HE.Investments.Account.WWW.Notifications;
using HE.Investments.Account.WWW.Routing;
using HE.Investments.Account.WWW.Utils;
using HE.Investments.Common.Config;
using HE.Investments.Common.CRM;
using HE.Investments.Common.WWW.Infrastructure.Authorization;
using HE.Investments.Loans.Common.Infrastructure;
using HE.Investments.Loans.Common.Models.App;
using HE.Investments.Loans.Common.Utils;

namespace HE.Investments.Account.WWW.Config;

public static class OrganisationWebModule
{
    public static void AddWebModule(this IServiceCollection services)
    {
        services.AddScoped<NonceModel>();
        AddConfiguration(services);
        AddMiddlewares(services);
        AddCommonModule(services);
        services.AddHttpUserContext();
        services.AddCrmConnection();
        services.AddAccountModule();
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(GetUserProfileInformationQueryHandler).Assembly));
        services.AddNotifications(typeof(ChangeOrganisationDetailsRequestedDisplayNotificationFactory).Assembly);
        services.AddScoped<IAccountRoutes, AccountRoutes>();
    }

    private static void AddConfiguration(IServiceCollection services)
    {
        services.AddSingleton<IOrganisationAppConfig, OrganisationAppConfig>(x => x.GetRequiredService<IConfiguration>().GetSection("AppConfiguration").Get<OrganisationAppConfig>());
        services.AddSingleton(x => x.GetRequiredService<IConfiguration>().GetSection("AppConfiguration:ProgrammeUrl").Get<ProgrammeUrlConfig>());
        services.AddSingleton<IProgrammes, Programmes>();
        services.AddSingleton<IDataverseConfig, DataverseConfig>(x => x.GetRequiredService<IConfiguration>().GetSection("AppConfiguration:Dataverse").Get<DataverseConfig>());
    }

    private static void AddMiddlewares(IServiceCollection services)
    {
        services.AddSingleton<PageNotFoundMiddleware>();
    }

    private static void AddCommonModule(IServiceCollection services)
    {
        services.AddScoped<IDateTimeProvider, DateTimeProvider>();
    }
}
