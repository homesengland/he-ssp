using HE.Investments.Account.Contract.UserOrganisation.Queries;
using HE.Investments.Account.Domain.Organisation.Repositories;
using HE.Investments.Consortium.Shared.UserContext;
using HE.Investments.Organisation.Contract;
using MediatR;

namespace HE.Investments.Account.Domain.UserOrganisation.QueryHandlers;

public class GetUserOrganisationListQueryHandler : IRequestHandler<GetUserOrganisationListQuery, IList<OrganisationDetails>>
{
    private readonly IConsortiumUserContext _accountUserContext;
    private readonly IOrganizationRepository _organizationRepository;

    public GetUserOrganisationListQueryHandler(
        IOrganizationRepository organizationRepository,
        IConsortiumUserContext accountUserContext)
    {
        _accountUserContext = accountUserContext;
        _organizationRepository = organizationRepository;
    }

    public async Task<IList<OrganisationDetails>> Handle(GetUserOrganisationListQuery request, CancellationToken cancellationToken)
    {
        // todo this is a temporary solution to get the organisation details for the user -> to be updated in the next pr AB#95737
        var account = await _accountUserContext.GetSelectedAccount();
        var organisationDetails = await _organizationRepository.GetBasicInformation(account.SelectedOrganisationId(), cancellationToken);

        return
        [
            new(
                organisationDetails.RegisteredCompanyName,
                organisationDetails.Address.Line1,
                organisationDetails.Address.City,
                organisationDetails.Address.PostalCode,
                organisationDetails.CompanyRegistrationNumber,
                Guid.NewGuid().ToString()),
        ];
    }
}
