using HE.Investments.Account.Contract.Users;
using HE.Investments.Account.Contract.Users.Queries;
using HE.Investments.Account.Domain.Organisation.Repositories;
using HE.Investments.Account.Domain.Users.Repositories;
using MediatR;

namespace HE.Investments.Account.Domain.Users.QueryHandlers;

public class GetUserDetailsQueryHandler : IRequestHandler<GetUserDetailsQuery, (string OrganisationName, UserDetails UserDetails)>
{
    private readonly IUserRepository _userRepository;
    private readonly ICurrentOrganisationRepository _organizationRepository;

    public GetUserDetailsQueryHandler(IUserRepository userRepository, ICurrentOrganisationRepository organizationRepository)
    {
        _userRepository = userRepository;
        _organizationRepository = organizationRepository;
    }

    public async Task<(string OrganisationName, UserDetails UserDetails)> Handle(GetUserDetailsQuery request, CancellationToken cancellationToken)
    {
        var organisation = await _organizationRepository.GetBasicInformation(cancellationToken);
        var user = await _userRepository.GetUser(request.Id, cancellationToken);

        var contract = new UserDetails(user.Id.Value, user.FirstName, user.LastName, user.Email, user.JobTitle, user.Role, null);

        return (organisation.RegisteredCompanyName, contract);
    }
}
