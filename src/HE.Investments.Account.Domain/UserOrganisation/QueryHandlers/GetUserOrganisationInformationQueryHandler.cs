using HE.Investments.Account.Contract.Organisation.Queries;
using HE.Investments.Account.Contract.UserOrganisation;
using HE.Investments.Account.Contract.Users;
using HE.Investments.Account.Domain.Organisation.Repositories;
using HE.Investments.Account.Domain.User.Repositories;
using HE.Investments.Account.Domain.UserOrganisation.Repositories;
using HE.Investments.Account.Shared;
using HE.Investments.Account.Shared.User;
using HE.Investments.Common;
using MediatR;
using Microsoft.FeatureManagement;

namespace HE.Investments.Account.Domain.UserOrganisation.QueryHandlers;

public class GetUserOrganisationInformationQueryHandler : IRequestHandler<GetUserOrganisationInformationQuery, GetUserOrganisationInformationQueryResponse>
{
    private readonly IAccountUserContext _accountUserContext;
    private readonly IProgrammeRepository _programmeRepository;
    private readonly IOrganizationRepository _organizationRepository;
    private readonly IProfileRepository _profileRepository;
    private readonly IFeatureManager _featureManager;

    public GetUserOrganisationInformationQueryHandler(
        IOrganizationRepository organizationRepository,
        IProfileRepository profileRepository,
        IAccountUserContext accountUserContext,
        IProgrammeRepository programmeRepository,
        IFeatureManager featureManager)
    {
        _accountUserContext = accountUserContext;
        _programmeRepository = programmeRepository;
        _featureManager = featureManager;
        _organizationRepository = organizationRepository;
        _profileRepository = profileRepository;
    }

    public async Task<GetUserOrganisationInformationQueryResponse> Handle(GetUserOrganisationInformationQuery request, CancellationToken cancellationToken)
    {
        var account = await _accountUserContext.GetSelectedAccount();
        var organisationDetails = await _organizationRepository.GetBasicInformation(account, cancellationToken);
        var userDetails = await _profileRepository.GetProfileDetails(_accountUserContext.UserGlobalId);

        if (await _featureManager.IsEnabledAsync(FeatureFlags.AhpProgram, account.AccountId.ToString()) is false)
        {
            return new GetUserOrganisationInformationQueryResponse(
                organisationDetails,
                userDetails.FirstName?.Value,
                account.Roles.All(r => r == UserRole.Limited),
                await _programmeRepository.GetLoanProgrammes(account, cancellationToken),
                new List<ProgrammeType> { ProgrammeType.Loans });
        }

        return new GetUserOrganisationInformationQueryResponse(
            organisationDetails,
            userDetails.FirstName?.Value,
            account.Roles.All(r => r == UserRole.Limited),
            await _programmeRepository.GetAllProgrammes(account, cancellationToken),
            new List<ProgrammeType> { ProgrammeType.Loans, ProgrammeType.Ahp });
    }
}
