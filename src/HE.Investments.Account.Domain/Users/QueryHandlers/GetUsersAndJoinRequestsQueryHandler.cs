using HE.Investments.Account.Contract.Users;
using HE.Investments.Account.Contract.Users.Queries;
using HE.Investments.Account.Domain.Organisation.Repositories;
using HE.Investments.Account.Domain.Users.Repositories;
using HE.Investments.Account.Shared;
using HE.Investments.Common.Extensions;
using HE.Investments.Common.Utils.Pagination;
using MediatR;

namespace HE.Investments.Account.Domain.Users.QueryHandlers;

public class GetUsersAndJoinRequestsQueryHandler : IRequestHandler<GetUsersAndJoinRequestsQuery, UsersAndJoinRequests>
{
    private readonly IUsersRepository _usersRepository;

    private readonly IOrganizationRepository _organizationRepository;

    private readonly IAccountUserContext _accountUserContext;

    public GetUsersAndJoinRequestsQueryHandler(
        IUsersRepository usersRepository,
        IOrganizationRepository organizationRepository,
        IAccountUserContext accountUserContext)
    {
        _usersRepository = usersRepository;
        _organizationRepository = organizationRepository;
        _accountUserContext = accountUserContext;
    }

    public async Task<UsersAndJoinRequests> Handle(GetUsersAndJoinRequestsQuery request, CancellationToken cancellationToken)
    {
        var userAccount = await _accountUserContext.GetSelectedAccount();
        var organisation = await _organizationRepository.GetBasicInformation(userAccount.SelectedOrganisationId(), cancellationToken);
        var allUsers = await _usersRepository.GetUsers(userAccount.SelectedOrganisationId());

        var users = allUsers.Where(u => u.Role != UserRole.Limited).ToList();
        var limited = allUsers.Where(u => u.Role == UserRole.Limited).ToList();

        return new UsersAndJoinRequests(
            organisation.RegisteredCompanyName,
            new PaginationResult<UserDetails>(users.TakePage(request.UsersPaging).ToList(), request.UsersPaging.Page, request.UsersPaging.ItemsPerPage, users.Count),
            limited);
    }
}
