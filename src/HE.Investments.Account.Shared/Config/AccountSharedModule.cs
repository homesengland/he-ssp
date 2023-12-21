using HE.Investments.Account.Shared.Repositories;
using HE.Investments.Account.Shared.Routing;
using HE.Investments.Organisation.CrmRepository;
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
        services.AddScoped<IAccountRepository, AccountCrmRepository>();
        services.AddScoped<IOrganizationRepository, OrganizationRepository>();

        if (useAccountService)
        {
            services.AddScoped(x => x.GetRequiredService<IConfiguration>().GetSection("AppConfiguration:AccountService").Get<AccountConfig>());
            services.AddScoped<IAccountRoutes, HttpAccountRoutes>();
        }
    }
}
