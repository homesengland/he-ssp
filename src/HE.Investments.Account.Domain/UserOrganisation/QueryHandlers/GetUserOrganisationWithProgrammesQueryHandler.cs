using HE.Investments.Account.Api.Contract.User;
using HE.Investments.Account.Contract.UserOrganisation.Queries;
using HE.Investments.Account.Domain.Organisation.Repositories;
using HE.Investments.Account.Domain.User.Repositories;
using HE.Investments.Account.Domain.UserOrganisation.Repositories;
using HE.Investments.Consortium.Shared.UserContext;
using MediatR;

namespace HE.Investments.Account.Domain.UserOrganisation.QueryHandlers;

public class GetUserOrganisationWithProgrammesQueryHandler : IRequestHandler<GetUserOrganisationWithProgrammesQuery, GetUserOrganisationWithProgrammesQueryResponse>
{
    private readonly IConsortiumUserContext _accountUserContext;
    private readonly IProgrammeApplicationsRepository _programmeApplicationsRepository;
    private readonly IOrganizationRepository _organizationRepository;
    private readonly IProfileRepository _profileRepository;
    private readonly IProjectRepository _projectRepository;

    public GetUserOrganisationWithProgrammesQueryHandler(
        IOrganizationRepository organizationRepository,
        IProfileRepository profileRepository,
        IConsortiumUserContext accountUserContext,
        IProgrammeApplicationsRepository programmeApplicationsRepository,
        IProjectRepository projectRepository)
    {
        _accountUserContext = accountUserContext;
        _programmeApplicationsRepository = programmeApplicationsRepository;
        _organizationRepository = organizationRepository;
        _projectRepository = projectRepository;
        _profileRepository = profileRepository;
    }

    public async Task<GetUserOrganisationWithProgrammesQueryResponse> Handle(GetUserOrganisationWithProgrammesQuery request, CancellationToken cancellationToken)
    {
        var account = await _accountUserContext.GetSelectedAccount();
        var organisationDetails = await _organizationRepository.GetBasicInformation(account.SelectedOrganisationId(), cancellationToken);
        var userDetails = await _profileRepository.GetProfileDetails(_accountUserContext.UserGlobalId);
        var projects = await _projectRepository.GetUserProjects(account, cancellationToken);

        return new GetUserOrganisationWithProgrammesQueryResponse(
            organisationDetails,
            userDetails.FirstName?.Value,
            account.Roles.All(r => r == UserRole.Limited),
            projects,
            await _programmeApplicationsRepository.GetAllProgrammes(account, cancellationToken));
    }
}
