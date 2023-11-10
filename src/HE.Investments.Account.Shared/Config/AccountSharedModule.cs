using HE.Investments.Account.Shared.Repositories;
using HE.Investments.Organisation.CrmRepository;
using Microsoft.Extensions.DependencyInjection;

namespace HE.Investments.Account.Shared.Config;

public static class AccountSharedModule
{
    public static void AddAccountSharedModule(this IServiceCollection services)
    {
        services.AddScoped<IAccountUserContext, AccountUserContext>();
        services.AddScoped<IAccountRepository, AccountCrmRepository>();

        services.AddScoped<IOrganizationRepository, OrganizationRepository>();
    }
}
