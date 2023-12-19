using HE.Investments.Account.Contract.Users;
using HE.Investments.Account.Contract.Users.Queries;
using HE.Investments.Account.Domain.Organisation.Repositories;
using HE.Investments.Account.Domain.Users.Repositories;
using HE.Investments.Common.Extensions;
using HE.Investments.Common.Utils.Pagination;
using MediatR;

namespace HE.Investments.Account.Domain.Users.QueryHandlers;

public class GetUsersAndJoinRequestsQueryHandler : IRequestHandler<GetUsersAndJoinRequestsQuery, UsersAndJoinRequests>
{
    private readonly IUsersRepository _usersRepository;
    private readonly ICurrentOrganisationRepository _organizationRepository;

    public GetUsersAndJoinRequestsQueryHandler(IUsersRepository usersRepository, ICurrentOrganisationRepository organizationRepository)
    {
        _usersRepository = usersRepository;
        _organizationRepository = organizationRepository;
    }

    public async Task<UsersAndJoinRequests> Handle(GetUsersAndJoinRequestsQuery request, CancellationToken cancellationToken)
    {
        var organisation = await _organizationRepository.GetBasicInformation(cancellationToken);
        var allUsers = await _usersRepository.GetUsers();

        var users = allUsers.Where(u => u.Role != UserRole.Limited).ToList();
        var limited = allUsers.Where(u => u.Role == UserRole.Limited).ToList();

        return new UsersAndJoinRequests(
            organisation.RegisteredCompanyName,
            new PaginationResult<UserDetails>(users.TakePage(request.UsersPaging).ToList(), request.UsersPaging.Page, request.UsersPaging.ItemsPerPage, users.Count),
            limited);
    }
}
