using HE.InvestmentLoans.Contract.Organization;
using HE.Investments.Account.Domain.Organisation.Repositories;
using HE.Investments.Account.Domain.User;
using MediatR;

namespace HE.Investments.Account.Domain.Organisation.QueryHandlers;

public class GetOrganisationDetailsQueryHandler : IRequestHandler<GetOrganisationDetailsQuery, GetOrganisationDetailsQueryResponse>
{
    private readonly IOrganizationRepository _organizationRepository;

    private readonly IAccountUserContext _loanUserContext;

    public GetOrganisationDetailsQueryHandler(IOrganizationRepository organizationRepository, IAccountUserContext loanUserContext)
    {
        _organizationRepository = organizationRepository;
        _loanUserContext = loanUserContext;
    }

    public async Task<GetOrganisationDetailsQueryResponse> Handle(GetOrganisationDetailsQuery request, CancellationToken cancellationToken)
    {
        var basicInformation = await _organizationRepository.GetBasicInformation(await _loanUserContext.GetSelectedAccount(), cancellationToken);
        var address = new List<string>()
        {
            basicInformation.Address.Line1,
            basicInformation.Address.Line2,
            basicInformation.Address.City,
            basicInformation.Address.PostalCode,
        };

        var organisationDataChangeRequestState = await _organizationRepository.GetOrganisationChangeRequestDetails(await _loanUserContext.GetSelectedAccount(), cancellationToken);

        return new GetOrganisationDetailsQueryResponse(
            new OrganisationDetailsViewModel(
                basicInformation.RegisteredCompanyName,
                basicInformation.ContactInformation.PhoneNUmber,
                address,
                basicInformation.CompanyRegistrationNumber,
                organisationDataChangeRequestState));
    }
}
