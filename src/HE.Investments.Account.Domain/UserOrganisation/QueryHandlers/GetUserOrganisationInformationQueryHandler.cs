using HE.InvestmentLoans.Contract.Application.ValueObjects;
using HE.InvestmentLoans.Contract.User.ValueObjects;
using HE.Investments.Account.Contract.Organisation.Queries;
using HE.Investments.Account.Contract.UserOrganisation;
using HE.Investments.Account.Domain.Organisation.Repositories;
using HE.Investments.Account.Domain.User;
using HE.Investments.Account.Domain.User.Repositories;
using HE.Investments.Account.Domain.UserOrganisation.Repositories;
using HE.Investments.Account.Shared;
using HE.Investments.Account.Shared.User;
using HE.Investments.Common.User;
using MediatR;

namespace HE.Investments.Account.Domain.UserOrganisation.QueryHandlers;

public class GetUserOrganisationInformationQueryHandler : IRequestHandler<GetUserOrganisationInformationQuery, GetUserOrganisationInformationQueryResponse>
{
    private readonly IAccountUserContext _accountUserContext;
    private readonly IProgrammeRepository _programmeRepository;
    private readonly IOrganizationRepository _organizationRepository;
    private readonly IUserRepository _userRepository;

    public GetUserOrganisationInformationQueryHandler(
        IOrganizationRepository organizationRepository,
        IUserRepository userRepository,
        IAccountUserContext accountUserContext,
        IProgrammeRepository programmeRepository)
    {
        _accountUserContext = accountUserContext;
        _programmeRepository = programmeRepository;
        _organizationRepository = organizationRepository;
        _userRepository = userRepository;
    }

    public async Task<GetUserOrganisationInformationQueryResponse> Handle(GetUserOrganisationInformationQuery request, CancellationToken cancellationToken)
    {
        var account = await _accountUserContext.GetSelectedAccount();
        var organisationDetails = await _organizationRepository.GetBasicInformation(account, cancellationToken);
        var userDetails = await _userRepository.GetProfileDetails(_accountUserContext.UserGlobalId);

        var userProgrammes = await _programmeRepository.GetAllProgrammes(account, cancellationToken);

        return new GetUserOrganisationInformationQueryResponse(
            organisationDetails,
            userDetails.FirstName?.Value,
            account.Roles.All(r => r.Role == UserAccountRole.LimitedUser),
            userProgrammes,
            new List<ProgrammeType> { ProgrammeType.Loans, ProgrammeType.Ahp });
    }
}
