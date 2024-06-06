using HE.Investments.Account.Api.Contract.User;
using HE.Investments.Account.Shared.Repositories;
using HE.Investments.Common.Contract;
using HE.Investments.IntegrationTestsFramework;
using HE.Investments.IntegrationTestsFramework.Auth;

namespace HE.Investments.AHP.IntegrationTests.Prerequisites;

public class IsOrganisationAdminPrerequisite : IIntegrationTestPrerequisite
{
    private readonly IAccountRepository _accountRepository;

    public IsOrganisationAdminPrerequisite(IAccountRepository accountRepository)
    {
        _accountRepository = accountRepository;
    }

    public async Task<string?> Verify(ILoginData loginData)
    {
        var userAccounts = await _accountRepository.GetUserAccounts(UserGlobalId.From(loginData.UserGlobalId), loginData.Email);
        if (userAccounts.Count != 1)
        {
            return $"User {loginData.UserGlobalId} should be connected to single Organisation {loginData.OrganisationId}" +
                   $" but connected to {userAccounts.Count} Organisations: {string.Join(", ", userAccounts.Select(x => x.Organisation?.OrganisationId.Value))}";
        }

        var userAccount = userAccounts.Single();
        return userAccount.Role() == UserRole.Admin
            ? null
            : $"User {loginData.UserGlobalId} should be Organisation Admin but is {userAccount.Role()}";
    }
}
