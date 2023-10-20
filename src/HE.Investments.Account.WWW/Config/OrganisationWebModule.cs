using HE.InvestmentLoans.Common.Infrastructure;
using HE.InvestmentLoans.Common.Utils;
using HE.Investments.Account.Domain.Config;
using HE.Investments.Account.Domain.User.QueryHandlers;
using HE.Investments.Account.WWW.Middlewares;

namespace HE.Investments.Account.WWW.Config;

public static class OrganisationWebModule
{
    public static void AddWebModule(this IServiceCollection services)
    {
        services.AddScoped<NonceModel>();
        AddConfiguration(services);
        AddMiddlewares(services);
        AddCommonModule(services);

        services.AddAccountModule();
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(GetUserProfileDetailsQueryHandler).Assembly));
    }

    private static void AddConfiguration(IServiceCollection services)
    {
        services.AddSingleton<IOrganisationAppConfig, OrganisationAppConfig>(x => x.GetRequiredService<IConfiguration>().GetSection("AppConfiguration").Get<OrganisationAppConfig>());
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
