using HE.Investments.Account.Domain.User;
using HE.Investments.Account.Domain.User.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace HE.Investments.Account.Domain.Config;

public static class AccountModule
{
    public static void AddAccountModule(this IServiceCollection services)
    {
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IAccountUserContext, AccountUserContext>();
    }
}
