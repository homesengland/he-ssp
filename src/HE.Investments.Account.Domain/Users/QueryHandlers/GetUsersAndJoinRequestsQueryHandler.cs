using HE.Investments.Account.Contract.Users;
using HE.Investments.Account.Contract.Users.Queries;
using HE.Investments.Account.Domain.Organisation.Repositories;
using HE.Investments.Account.Domain.Users.Repositories;
using HE.Investments.Account.Shared;
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
        var users = await _usersRepository.GetUsers();

        return new UsersAndJoinRequests(
            organisation.RegisteredCompanyName,
            users.Where(u => u.Role != UserRole.Limited).ToList(),
            users.Where(u => u.Role == UserRole.Limited).ToList());
    }
}
