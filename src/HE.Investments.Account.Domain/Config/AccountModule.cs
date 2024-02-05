using HE.Investments.Account.Domain.Data;
using HE.Investments.Account.Domain.Organisation.Repositories;
using HE.Investments.Account.Domain.User.Repositories;
using HE.Investments.Account.Domain.UserOrganisation.Repositories;
using HE.Investments.Account.Domain.Users.Repositories;
using HE.Investments.Account.Shared;
using HE.Investments.Account.Shared.Config;
using HE.Investments.Account.Shared.Repositories;
using HE.Investments.Organisation.Config;
using Microsoft.Extensions.DependencyInjection;

namespace HE.Investments.Account.Domain.Config;

public static class AccountModule
{
    public static void AddAccountModule(this IServiceCollection services)
    {
        services.AddOrganizationsModule();
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(AccountModule).Assembly));
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(AccountSharedModule).Assembly));
        services.AddScoped<IProfileRepository, ProfileRepository>();
        services.AddScoped<IContactRepository, ContactRepository>();
        services.AddScoped<IAccountRepository, AccountCrmRepository>();
        services.AddScoped<IAccountUserContext, AccountUserContext>();
        services.AddScoped<IAccountAccessContext, AccountAccessContext>();
        services.AddScoped<IOrganizationRepository, OrganizationRepository>();
        services.AddScoped<IUsersCrmContext, UsersCrmContext>();
        services.AddScoped<IUsersRepository, UsersRepository>();
        services.AddScoped<IUserRepository, UserRepository>();

        AddUserOrganisations(services);
    }

    private static void AddUserOrganisations(IServiceCollection services)
    {
        services.AddScoped<IProgrammeApplicationsRepository, ProgrammeApplicationsRepository>();
        services.AddScoped<IOrganisationUsersRepository, OrganisationUsersRepository>();
    }
}
