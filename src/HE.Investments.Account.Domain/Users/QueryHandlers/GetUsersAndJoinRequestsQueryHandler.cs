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
    private readonly IOrganizationRepository _organizationRepository;
    private readonly IAccountUserContext _accountUserContext;

    public GetUsersAndJoinRequestsQueryHandler(IUsersRepository usersRepository, IOrganizationRepository organizationRepository, IAccountUserContext accountUserContext)
    {
        _usersRepository = usersRepository;
        _organizationRepository = organizationRepository;
        this._accountUserContext = accountUserContext;
    }

    public async Task<UsersAndJoinRequests> Handle(GetUsersAndJoinRequestsQuery request, CancellationToken cancellationToken)
    {
        var organisation = await _organizationRepository.GetBasicInformation(await _accountUserContext.GetSelectedAccount(), cancellationToken);
        var users = await _usersRepository.GetUsers();

        return new UsersAndJoinRequests(
            organisation.RegisteredCompanyName,
            users,
            new List<UserDetails>
            {
                new("11", "Tomasz", "Kot", "kot@elo.pl", "nikt", "Limited", null),
                new("12", "trer", "erer", "kot@elo.pl", "nikt", "Limited", null),
            });
    }
}
