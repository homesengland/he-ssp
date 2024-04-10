using HE.Investments.Account.Contract.Organisation;
using HE.Investments.Account.Contract.Organisation.Queries;
using HE.Investments.Account.Domain.Organisation.Repositories;
using HE.Investments.Account.Domain.UserOrganisation.Repositories;
using HE.Investments.Account.Shared;
using MediatR;

namespace HE.Investments.Account.Domain.Organisation.QueryHandlers;

public class GetOrganisationDetailsQueryHandler : IRequestHandler<GetOrganisationDetailsQuery, OrganisationDetails>
{
    private readonly IOrganizationRepository _organizationRepository;

    private readonly IAccountUserContext _accountUserContext;

    private readonly IProgrammeApplicationsRepository _applicationsRepository;

    public GetOrganisationDetailsQueryHandler(
        IOrganizationRepository organizationRepository,
        IAccountUserContext accountUserContext,
        IProgrammeApplicationsRepository applicationsRepository)
    {
        _organizationRepository = organizationRepository;
        _accountUserContext = accountUserContext;
        _applicationsRepository = applicationsRepository;
    }

    public async Task<OrganisationDetails> Handle(GetOrganisationDetailsQuery request, CancellationToken cancellationToken)
    {
        var account = await _accountUserContext.GetSelectedAccount();
        var basicInformation = await _organizationRepository.GetBasicInformation(account.SelectedOrganisationId(), cancellationToken);
        var address = new List<string>
        {
            basicInformation.Address.Line1,
            basicInformation.Address.Line2,
            basicInformation.Address.City,
            basicInformation.Address.PostalCode,
        };

        var organisationDataChangeRequestState = await _organizationRepository.GetOrganisationChangeRequestDetails(account.SelectedOrganisationId(), cancellationToken);
        var hasAnyAhpApplication = await _applicationsRepository.HasAnyAhpApplication(account, cancellationToken);

        return new OrganisationDetails(
            basicInformation.RegisteredCompanyName,
            basicInformation.ContactInformation.PhoneNUmber,
            address,
            basicInformation.CompanyRegistrationNumber,
            organisationDataChangeRequestState,
            basicInformation.InvestmentPartnerStatus,
            hasAnyAhpApplication);
    }
}
