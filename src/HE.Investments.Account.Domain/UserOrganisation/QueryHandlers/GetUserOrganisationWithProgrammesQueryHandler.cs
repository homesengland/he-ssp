using HE.Investments.Account.Contract.UserOrganisation;
using HE.Investments.Account.Contract.UserOrganisation.Queries;
using HE.Investments.Account.Contract.Users;
using HE.Investments.Account.Domain.Organisation.Repositories;
using HE.Investments.Account.Domain.User.Repositories;
using HE.Investments.Account.Domain.UserOrganisation.Repositories;
using HE.Investments.Account.Shared;
using HE.Investments.Common;
using MediatR;
using Microsoft.FeatureManagement;

namespace HE.Investments.Account.Domain.UserOrganisation.QueryHandlers;

public class GetUserOrganisationWithProgrammesQueryHandler : IRequestHandler<GetUserOrganisationWithProgrammesQuery, GetUserOrganisationWithProgrammesQueryResponse>
{
    private readonly IAccountUserContext _accountUserContext;
    private readonly IProgrammeApplicationsRepository _programmeApplicationsRepository;
    private readonly IOrganizationRepository _organizationRepository;
    private readonly IProfileRepository _profileRepository;
    private readonly IFeatureManager _featureManager;

    public GetUserOrganisationWithProgrammesQueryHandler(
        IOrganizationRepository organizationRepository,
        IProfileRepository profileRepository,
        IAccountUserContext accountUserContext,
        IProgrammeApplicationsRepository programmeApplicationsRepository,
        IFeatureManager featureManager)
    {
        _accountUserContext = accountUserContext;
        _programmeApplicationsRepository = programmeApplicationsRepository;
        _featureManager = featureManager;
        _organizationRepository = organizationRepository;
        _profileRepository = profileRepository;
    }

    public async Task<GetUserOrganisationWithProgrammesQueryResponse> Handle(GetUserOrganisationWithProgrammesQuery request, CancellationToken cancellationToken)
    {
        var account = await _accountUserContext.GetSelectedAccount();
        var organisationDetails = await _organizationRepository.GetBasicInformation(account.SelectedOrganisationId(), cancellationToken);
        var userDetails = await _profileRepository.GetProfileDetails(_accountUserContext.UserGlobalId);

        if (!await _featureManager.IsEnabledAsync(FeatureFlags.AhpProgram, account.SelectedOrganisationId().ToString()))
        {
            return new GetUserOrganisationWithProgrammesQueryResponse(
                organisationDetails,
                userDetails.FirstName?.Value,
                account.Roles.All(r => r == UserRole.Limited),
                await _programmeApplicationsRepository.GetLoanProgrammes(account, cancellationToken),
                new List<ProgrammeType> { ProgrammeType.Loans });
        }

        return new GetUserOrganisationWithProgrammesQueryResponse(
            organisationDetails,
            userDetails.FirstName?.Value,
            account.Roles.All(r => r == UserRole.Limited),
            await _programmeApplicationsRepository.GetAllProgrammes(account, cancellationToken),
            new List<ProgrammeType> { ProgrammeType.Loans, ProgrammeType.Ahp });
    }
}
