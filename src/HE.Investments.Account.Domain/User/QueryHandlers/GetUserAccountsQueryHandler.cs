using HE.Investments.Account.Api.Contract.Organisation;
using HE.Investments.Account.Api.Contract.User;
using HE.Investments.Account.Contract.User.Queries;
using HE.Investments.Account.Shared.Repositories;
using HE.Investments.Common.Extensions;
using MediatR;

namespace HE.Investments.Account.Domain.User.QueryHandlers;

public class GetUserAccountsQueryHandler : IRequestHandler<GetUserAccountsQuery, AccountDetails>
{
    private readonly IAccountRepository _accountRepository;

    public GetUserAccountsQueryHandler(IAccountRepository accountRepository)
    {
        _accountRepository = accountRepository;
    }

    public async Task<AccountDetails> Handle(GetUserAccountsQuery request, CancellationToken cancellationToken)
    {
        var userAccounts = await _accountRepository.GetUserAccounts(request.UserGlobalId, request.UserEmailAddress);

        return new AccountDetails(
            request.UserGlobalId.Value,
            request.UserEmailAddress,
            userAccounts.Select(MapUserOrganisation).Where(x => x.IsProvided()).Select(x => x!).ToList());
    }

    private static Api.Contract.User.UserOrganisation? MapUserOrganisation(Shared.User.UserAccount userAccount)
    {
        if (userAccount.Organisation.IsProvided())
        {
            return new Api.Contract.User.UserOrganisation(
                new OrganisationDetails(
                    userAccount.Organisation!.OrganisationId.ToString(),
                    userAccount.OrganisationName,
                    userAccount.Organisation.IsUnregisteredBody),
                userAccount.Roles);
        }

        return null;
    }
}
