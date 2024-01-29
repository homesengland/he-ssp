using HE.Investments.Account.Shared.Repositories;
using HE.Investments.Account.Shared.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HE.Investments.Account.Shared.Config;

public static class AccountSharedModule
{
    public static void AddAccountSharedModule(this IServiceCollection services, bool useAccountService = false)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(AccountSharedModule).Assembly));
        services.AddScoped<IAccountUserContext, AccountUserContext>();
        services.AddScoped<IAccountAccessContext, AccountAccessContext>();
        services.AddHttpClient<AccountHttpRepository>().ConfigureHttpClient(SetupHttpClient);
        services.AddScoped<Func<AccountHttpRepository>>(x => x.GetRequiredService<AccountHttpRepository>);
        services.AddScoped<AccountCrmRepository>();
        services.AddScoped<Func<AccountCrmRepository>>(x => x.GetRequiredService<AccountCrmRepository>);
        services.AddScoped<IAccountRepository, AccountRepositoryDecorator>();

        if (useAccountService)
        {
            services.AddSingleton(x => x.GetRequiredService<IConfiguration>().GetSection("AppConfiguration:AccountService").Get<AccountConfig>());
            services.AddScoped<IAccountRoutes, HttpAccountRoutes>();
        }
    }

    private static void SetupHttpClient(IServiceProvider services, HttpClient httpClient)
    {
        var config = services.GetRequiredService<IConfiguration>().GetSection("AppConfiguration:AccountService").Get<AccountConfig>();
        httpClient.BaseAddress = new Uri(config.Url);
    }
}
