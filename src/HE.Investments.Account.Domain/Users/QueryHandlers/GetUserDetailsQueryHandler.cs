using HE.Investments.Account.Contract.Users;
using HE.Investments.Account.Contract.Users.Queries;
using HE.Investments.Account.Domain.Organisation.Repositories;
using HE.Investments.Account.Domain.Users.Repositories;
using HE.Investments.Account.Shared;
using HE.Investments.Account.Shared.User;
using MediatR;

namespace HE.Investments.Account.Domain.Users.QueryHandlers;

public class GetUserDetailsQueryHandler : IRequestHandler<GetUserDetailsQuery, (string OrganisationName, UserDetails UserDetails)>
{
    private readonly IUserRepository _userRepository;

    private readonly IOrganizationRepository _organizationRepository;

    private readonly IAccountUserContext _accountUserContext;

    public GetUserDetailsQueryHandler(IUserRepository userRepository, IOrganizationRepository organizationRepository, IAccountUserContext accountUserContext)
    {
        _userRepository = userRepository;
        _organizationRepository = organizationRepository;
        _accountUserContext = accountUserContext;
    }

    public async Task<(string OrganisationName, UserDetails UserDetails)> Handle(GetUserDetailsQuery request, CancellationToken cancellationToken)
    {
        var userAccount = await _accountUserContext.GetSelectedAccount();
        var organisation = await _organizationRepository.GetBasicInformation(userAccount.SelectedOrganisationId(), cancellationToken);
        var user = await _userRepository.GetUser(new UserGlobalId(request.Id), userAccount.SelectedOrganisationId(), cancellationToken);

        var contract = new UserDetails(user.Id.Value, user.FirstName, user.LastName, user.Email, user.JobTitle, user.Role, null);

        return (organisation.RegisteredCompanyName, contract);
    }
}
